#!/usr/bin/env bash
# Compila e hospeda o AJA. Execute como usuário comum com sudo.
set -Eeuo pipefail
umask 027

if (( EUID != 0 )); then
    echo 'Execute com sudo: sudo bash scripts/03-publicar-aplicacao.sh' >&2
    exit 1
fi

SCRIPT_DIR=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)
PROJECT_ROOT=$(cd -- "$SCRIPT_DIR/.." && pwd)
BUILD_USER="${SUDO_USER:-}"
LOG_FILE=/var/log/aja/publicacao-aplicacao.log
install -d -o root -g adm -m 0750 /var/log/aja
touch "$LOG_FILE"
chown root:adm "$LOG_FILE"
chmod 0640 "$LOG_FILE"
exec > >(tee -a "$LOG_FILE") 2>&1

log() {
    printf '%s [%s] %s\n' "$(date --iso-8601=seconds)" "$1" "$2"
}
finish() {
    local result=$?
    if (( result != 0 )); then
        log ERRO "Publicação interrompida (código $result). Log: $LOG_FILE"
        log INFO 'Para erros da API: sudo journalctl -u aja-api -n 60 --no-pager'
    fi
}
trap finish EXIT

log INICIO 'Publicação da aplicação AJA.'
[[ -n "$BUILD_USER" && "$BUILD_USER" != root ]] || {
    log ERRO 'Entre com seu usuário comum e execute o script usando sudo.'
    exit 1
}
id aja >/dev/null 2>&1 || { log ERRO 'Execute primeiro o script de infraestrutura.'; exit 1; }
for command in dotnet mysql nginx curl python3 runuser; do
    command -v "$command" >/dev/null || { log ERRO "Dependência ausente: $command. Execute o script 02."; exit 1; }
done
for file in app/frontend/package-lock.json app/backend/src/Aja.HelperApp.Api/Aja.HelperApp.Api.csproj config/aja-api.service config/nginx-aja.conf; do
    [[ -f "$PROJECT_ROOT/$file" ]] || { log ERRO "Arquivo ausente: $PROJECT_ROOT/$file. Copie app, config e scripts para a mesma pasta."; exit 1; }
done
# Estes destinos pertencem à publicação AJA; recuse sobrescrever diretórios reais.
for target in /opt/aja/api /var/www/aja; do
    [[ ! -e "$target" || -L "$target" ]] || { log ERRO "Destino não é um link gerenciado pelo AJA: $target"; exit 1; }
done
exec 9>/run/lock/aja-infra.lock
flock -n 9 || { log ERRO 'Outro provisionamento AJA está em execução.'; exit 1; }

export DEBIAN_FRONTEND=noninteractive
log INFO 'Instalando Node.js e npm do Ubuntu para compilar o frontend.'
apt-get -o DPkg::Lock::Timeout=300 -o APT::Update::Error-Mode=any update
apt-get -o DPkg::Lock::Timeout=300 install -y nodejs npm
node --version
npm --version
node -e 'const [major,minor]=process.versions.node.split(".").map(Number); if (!((major===22 && minor>=13)||(major===24 && minor>=3)||major>=26)) { throw new Error("Node incompatível com Expo SDK 58"); }'

log INFO 'Compilando frontend como usuário comum; esta etapa pode demorar.'
runuser -u "$BUILD_USER" -- env CI=1 EXPO_PUBLIC_API_URL=/ bash -c '
    set -e
    cd -- "$1"
    npm ci --no-audit --no-fund
    npm run typecheck
    npm run lint
    node node_modules/expo/bin/cli export --platform web --max-workers 1
' _ "$PROJECT_ROOT/app/frontend"
[[ -s "$PROJECT_ROOT/app/frontend/dist/index.html" ]]

log INFO 'Compilando API como usuário comum.'
runuser -u "$BUILD_USER" -- env DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1 bash -c '
    set -e
    cd -- "$1"
    dotnet publish src/Aja.HelperApp.Api --configuration Release --output .publish -p:UseAppHost=false
' _ "$PROJECT_ROOT/app/backend"
[[ -s "$PROJECT_ROOT/app/backend/.publish/Aja.HelperApp.Api.dll" ]]

log INFO 'Preparando credenciais locais; a senha não será exibida nem incluída no repositório.'
install -d -o root -g root -m 0750 /etc/aja
if [[ ! -f /etc/aja/database-password ]]; then
    python3 - <<'PY'
from pathlib import Path
import secrets
path = Path('/etc/aja/database-password')
with path.open('x') as stream:
    stream.write(secrets.token_hex(24))
path.chmod(0o600)
PY
fi
DB_PASSWORD=$(cat /etc/aja/database-password)
[[ "$DB_PASSWORD" =~ ^[0-9a-f]{48}$ ]] || { log ERRO 'Arquivo de credencial existente tem formato inesperado.'; exit 1; }
chmod 0600 /etc/aja/database-password
systemctl enable --now mysql

log INFO 'Criando banco aja_helperapp e usuário exclusivo com permissões apenas nesse banco.'
mysql --protocol=socket <<SQL
CREATE DATABASE IF NOT EXISTS aja_helperapp CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER IF NOT EXISTS 'aja_app'@'127.0.0.1' IDENTIFIED BY '$DB_PASSWORD';
ALTER USER 'aja_app'@'127.0.0.1' IDENTIFIED BY '$DB_PASSWORD';
GRANT ALL PRIVILEGES ON aja_helperapp.* TO 'aja_app'@'127.0.0.1';
SQL
DB_VERSION=$(mysql --protocol=socket --batch --skip-column-names -e 'SELECT VERSION();')
DB_VERSION=${DB_VERSION%%-*}
[[ "$DB_VERSION" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]

# Apenas root lê a credencial; o systemd fornece o ambiente ao processo da API.
cat > /etc/aja/api.env <<ENV
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://127.0.0.1:5080
ConnectionStrings__DefaultConnection="Server=127.0.0.1;Port=3306;Database=aja_helperapp;User ID=aja_app;Password=$DB_PASSWORD"
Database__ServerVersion=$DB_VERSION
DataProtection__KeysDirectory=/var/lib/aja/keys
DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV
chmod 0600 /etc/aja/api.env
chown root:root /etc/aja/api.env
unset DB_PASSWORD
log OK "Banco preparado; versão real do MySQL: $DB_VERSION."

RELEASE=$(date -u +%Y%m%dT%H%M%SZ)-$$
API_RELEASE="/opt/aja/releases/$RELEASE"
WEB_RELEASE="/var/www/aja-releases/$RELEASE"
install -d -o root -g aja -m 0750 /opt/aja/releases "$API_RELEASE"
cp -a "$PROJECT_ROOT/app/backend/.publish/." "$API_RELEASE/"
chown -R root:aja "$API_RELEASE"
find "$API_RELEASE" -type d -exec chmod 0750 {} +
find "$API_RELEASE" -type f -exec chmod 0640 {} +
install -d -o root -g root -m 0755 /var/www/aja-releases "$WEB_RELEASE"
cp -a "$PROJECT_ROOT/app/frontend/dist/." "$WEB_RELEASE/"
chown -R root:root "$WEB_RELEASE"
find "$WEB_RELEASE" -type d -exec chmod 0755 {} +
find "$WEB_RELEASE" -type f -exec chmod 0644 {} +

BACKUP_DIR=$(mktemp -d /var/backups/aja-publicacao-XXXXXXXX)
chmod 0700 "$BACKUP_DIR"
for file in /etc/systemd/system/aja-api.service /etc/nginx/sites-available/aja /etc/nginx/sites-enabled/default; do
    if [[ -e "$file" || -L "$file" ]]; then
        cp -a "$file" "$BACKUP_DIR/$(basename "$file")"
    fi
done
readlink /opt/aja/api > "$BACKUP_DIR/api-anterior" || true
readlink /var/www/aja > "$BACKUP_DIR/web-anterior" || true
log INFO "Configurações anteriores preservadas em $BACKUP_DIR."

ln -sfn "$API_RELEASE" /opt/aja/api
ln -sfn "$WEB_RELEASE" /var/www/aja
install -o root -g root -m 0644 "$PROJECT_ROOT/config/aja-api.service" /etc/systemd/system/aja-api.service
install -o root -g root -m 0644 "$PROJECT_ROOT/config/nginx-aja.conf" /etc/nginx/sites-available/aja
if [[ -L /etc/nginx/sites-enabled/default ]]; then
    unlink /etc/nginx/sites-enabled/default
fi
ln -sfn /etc/nginx/sites-available/aja /etc/nginx/sites-enabled/aja
nginx -t

log INFO 'Iniciando API; as migrações criam as tabelas e os dados demonstrativos.'
systemctl daemon-reload
systemctl enable aja-api nginx
systemctl reset-failed aja-api
systemctl restart aja-api
API_READY=0
for (( attempt=1; attempt<=60; attempt++ )); do
    if curl --fail --silent --max-time 5 http://127.0.0.1:5080/health >/dev/null; then
        API_READY=1
        break
    fi
    sleep 2
done
if (( API_READY != 1 )); then
    log ERRO 'A API não ficou disponível. Confira o journal antes de prosseguir.'
    exit 1
fi

systemctl reload nginx
curl --fail --silent --show-error --max-time 15 http://127.0.0.1/health
printf '\n'
curl --fail --silent --show-error --max-time 15 --output /dev/null http://127.0.0.1/
log OK 'Frontend e API publicados; rota /health confirma conexão com o banco.'
log INFO 'API: sudo systemctl status aja-api --no-pager'
log INFO 'Logs da API: sudo journalctl -u aja-api -n 60 --no-pager'
log INFO 'No VirtualBox, encaminhe 127.0.0.1:8080 do Windows para a porta 80 da VM.'
log FIM "Publicação concluída. Acesse http://127.0.0.1:8080 após configurar o encaminhamento. Log: $LOG_FILE"
