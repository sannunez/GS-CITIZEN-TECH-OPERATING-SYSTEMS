#!/usr/bin/env bash
# Ambiente alvo: Ubuntu Server, VirtualBox NAT no adaptador 1.
set -Eeuo pipefail
umask 027

if (( EUID != 0 )); then
    echo 'Execute com sudo: sudo bash scripts/01-provisionar-infra.sh' >&2
    exit 1
fi

INTERFACE="${AJA_INTERFACE:-enp0s3}"
ADDRESS="${AJA_ADDRESS:-10.0.2.15/24}"
GATEWAY="${AJA_GATEWAY:-10.0.2.2}"
DNS="${AJA_DNS:-10.0.2.3}"
SERVER_NAME="${AJA_HOSTNAME:-aja-server}"
NETWORK_FILE=/etc/netplan/90-aja-infra.yaml
LOG_FILE=/var/log/aja/provisionamento-infra.log
ROLLBACK_NEEDED=0
BACKUP_DIR=''
HAD_NETWORK_FILE=0

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
    trap - EXIT
    set +e
    if (( ROLLBACK_NEEDED == 1 )); then
        log AVISO 'Restaurando a configuração anterior de rede.'
        if (( HAD_NETWORK_FILE == 1 )); then
            cp -p "$BACKUP_DIR/90-aja-infra.yaml" "$NETWORK_FILE"
        else
            rm -f -- "$NETWORK_FILE"
        fi
        if netplan generate && netplan apply; then
            log INFO 'Configuração anterior reaplicada; confira IP e DNS antes de reiniciar.'
        else
            log ERRO "Revise a rede pelo console do VirtualBox. Backup: $BACKUP_DIR"
        fi
    fi
    if (( result != 0 )); then
        log ERRO "Provisionamento interrompido (código $result). Consulte $LOG_FILE."
    fi
    exit "$result"
}
trap finish EXIT
trap 'exit 130' INT
trap 'exit 143' TERM
trap 'exit 129' HUP

log INICIO 'Provisionamento da infraestrutura AJA.'
for command in netplan ip hostnamectl useradd getent python3 flock; do
    command -v "$command" >/dev/null || {
        log ERRO "Comando obrigatório ausente: $command"
        exit 1
    }
done
exec 9>/run/lock/aja-infra.lock
flock -n 9 || { log ERRO 'Já existe um provisionamento em execução.'; exit 1; }

[[ -t 0 ]] || { log ERRO 'Execute em um terminal interativo para confirmar a rede.'; exit 1; }
[[ -d "/sys/class/net/$INTERFACE" ]] || { log ERRO "Interface ausente: $INTERFACE"; exit 1; }

# Valida os valores antes de inseri-los no YAML ou alterar a máquina.
python3 - "$INTERFACE" "$ADDRESS" "$GATEWAY" "$DNS" "$SERVER_NAME" <<'PY'
import ipaddress
import re
import sys
interface, address, gateway, dns, hostname = sys.argv[1:]
assert re.fullmatch(r'[a-zA-Z0-9_-]{1,15}', interface), 'Interface inválida'
network = ipaddress.IPv4Interface(address)
assert ipaddress.IPv4Address(gateway) in network.network, 'Gateway fora da sub-rede'
ipaddress.IPv4Address(dns)
assert re.fullmatch(r'[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?', hostname), 'Hostname inválido'
PY

BACKUP_DIR=$(mktemp -d /var/backups/aja-infra-XXXXXXXX)
chmod 0700 "$BACKUP_DIR"
cp -a /etc/netplan "$BACKUP_DIR/netplan"
cp -p /etc/hosts "$BACKUP_DIR/hosts"
hostnamectl --static > "$BACKUP_DIR/hostname"
if [[ -e "$NETWORK_FILE" ]]; then
    cp -p "$NETWORK_FILE" "$BACKUP_DIR/90-aja-infra.yaml"
    HAD_NETWORK_FILE=1
fi
log INFO "Backup criado em $BACKUP_DIR."

ROLLBACK_NEEDED=1
install -o root -g root -m 0600 /dev/null "$NETWORK_FILE"
cat > "$NETWORK_FILE" <<YAML
# Gerado por 01-provisionar-infra.sh. Rede NAT do VirtualBox.
network:
  version: 2
  renderer: networkd
  ethernets:
    $INTERFACE:
      dhcp4: false
      dhcp6: true
      addresses: [$ADDRESS]
      routes:
        - to: default
          via: $GATEWAY
          metric: 100
      nameservers:
        addresses: [$DNS]
YAML

netplan generate
log INFO "Testando IP $ADDRESS, gateway $GATEWAY e DNS $DNS."
log AVISO 'Quando o Netplan perguntar, pressione ENTER para manter a rede se o terminal continuar respondendo.'
log AVISO 'Se perder acesso, aguarde a reversão e use o console do VirtualBox para conferir a rede.'
if ! netplan try --timeout 60; then
    log ERRO 'Rede não confirmada. A configuração anterior será restaurada.'
    exit 1
fi

# Confirma endereço e resolução antes de encerrar a possibilidade de reversão.
ip -4 -o address show dev "$INTERFACE" | awk '{print $4}' | grep -Fxq "$ADDRESS"
timeout 15 getent ahostsv4 ubuntu.com >/dev/null
ROLLBACK_NEEDED=0
log OK 'IP fixo e resolução de nomes verificados.'

hostnamectl set-hostname "$SERVER_NAME"
python3 - "$SERVER_NAME" <<'PY'
from pathlib import Path
import sys
path = Path('/etc/hosts')
lines = [line for line in path.read_text().splitlines()
         if not line.split('#', 1)[0].split()[:1] == ['127.0.1.1']]
lines.append(f'127.0.1.1\t{sys.argv[1]}')
path.write_text('\n'.join(lines) + '\n')
PY
log OK "Hostname configurado: $SERVER_NAME."

if ! id aja >/dev/null 2>&1; then
    useradd --system --user-group --home-dir /var/lib/aja --no-create-home --shell /usr/sbin/nologin aja
    log OK 'Usuário de serviço aja criado, sem login interativo.'
else
    [[ "$(id -u aja)" != 0 && "$(getent passwd aja | cut -d: -f7)" == /usr/sbin/nologin ]] || {
        log ERRO 'Já existe um usuário aja incompatível com o usuário de serviço esperado.'
        exit 1
    }
fi
install -d -o root -g aja -m 0750 /opt/aja
install -d -o root -g root -m 0750 /etc/aja
install -d -o aja -g aja -m 0750 /var/lib/aja
install -d -o aja -g aja -m 0700 /var/lib/aja/keys
log OK 'Diretórios e permissões preparados para a aplicação e suas chaves.'

log INFO 'Resumo da infraestrutura:'
hostnamectl --static
ip -br address show dev "$INTERFACE"
ip -4 route
resolvectl dns "$INTERFACE"
log FIM "Infraestrutura provisionada. Log: $LOG_FILE"
