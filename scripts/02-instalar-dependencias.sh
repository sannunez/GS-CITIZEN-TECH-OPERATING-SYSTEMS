#!/usr/bin/env bash
# Instala as dependências no Ubuntu Server 26.04; ainda não publica o AJA.
set -Eeuo pipefail
umask 027

if (( EUID != 0 )); then
    echo 'Execute com sudo: sudo bash scripts/02-instalar-dependencias.sh' >&2
    exit 1
fi

install -d -o root -g adm -m 0750 /var/log/aja
LOG_FILE=/var/log/aja/instalacao-dependencias.log
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
        log ERRO "Instalação interrompida (código $result). Consulte $LOG_FILE."
        log INFO 'Resolva o erro apresentado e execute o script novamente; não é necessário recriar a VM.'
    fi
}
trap finish EXIT

log INICIO 'Instalação das dependências da aplicação AJA.'
# Arquivo de identificação fornecido pelo sistema operacional.
source /etc/os-release
if [[ "$ID" != ubuntu || "$VERSION_ID" != 26.04 ]]; then
    log ERRO 'Este script foi preparado para Ubuntu Server 26.04 LTS.'
    exit 1
fi
id aja >/dev/null 2>&1 || {
    log ERRO 'Execute primeiro 01-provisionar-infra.sh.'
    exit 1
}
exec 9>/run/lock/aja-infra.lock
flock -n 9 || { log ERRO 'Outro script de provisionamento AJA está em execução.'; exit 1; }

export DEBIAN_FRONTEND=noninteractive
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

log INFO 'Atualizando o catálogo de pacotes dos repositórios configurados no Ubuntu.'
apt-get -o DPkg::Lock::Timeout=300 -o APT::Update::Error-Mode=any update

# .NET 10 está disponível no repositório do Ubuntu 26.04.
# Não adicionamos feeds externos nem alteramos os pacotes da aplicação Mobile.
log INFO 'Instalando SDK .NET 10, runtime ASP.NET Core 10, MySQL, Nginx e ferramentas de transferência.'
apt-get -o DPkg::Lock::Timeout=300 install -y \
    dotnet-sdk-10.0 \
    aspnetcore-runtime-10.0 \
    mysql-server \
    nginx \
    curl \
    ca-certificates \
    unzip

log INFO 'Habilitando MySQL e Nginx para iniciar junto com o Ubuntu.'
nginx -t
systemctl enable --now mysql nginx
systemctl is-active --quiet mysql
systemctl is-active --quiet nginx

log INFO 'Verificando SDK e runtime exigidos pelo backend.'
dotnet --list-sdks
dotnet --list-sdks | grep -Eq '^10\.0\.'
dotnet --list-runtimes
dotnet --list-runtimes | grep -Eq '^Microsoft\.AspNetCore\.App 10\.0\.'

log INFO 'Verificando acesso administrativo local ao MySQL, sem criar banco ou contas ainda.'
mysql --protocol=socket --batch --skip-column-names -e 'SELECT VERSION();'

log INFO 'Verificando resposta HTTP local do Nginx.'
curl --fail --silent --show-error --max-time 15 --output /dev/null http://127.0.0.1/

log INFO 'Versões dos pacotes instalados:'
dpkg-query -W -f='${binary:Package} ${Version}\n' \
    dotnet-sdk-10.0 aspnetcore-runtime-10.0 mysql-server nginx

log OK 'Dependências instaladas; MySQL e Nginx ativos e habilitados no boot.'
log INFO 'O site AJA e a API serão configurados na próxima etapa. O Nginx ainda serve sua página padrão.'
log FIM "Instalação concluída. Log: $LOG_FILE"
