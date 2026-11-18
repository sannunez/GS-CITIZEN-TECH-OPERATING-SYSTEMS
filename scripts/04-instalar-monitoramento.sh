#!/usr/bin/env bash
set -Eeuo pipefail
umask 027
(( EUID == 0 )) || { echo 'Execute com sudo.' >&2; exit 1; }
ROOT=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)
install -d -o root -g adm -m 0750 /var/log/aja
LOG=/var/log/aja/instalacao-monitoramento.log
touch "$LOG"
chown root:adm "$LOG"
chmod 0640 "$LOG"
exec > >(tee -a "$LOG") 2>&1
log() { printf '%s [%s] %s\n' "$(date --iso-8601=seconds)" "$1" "$2"; }
trap 'log ERRO "Instalação interrompida na linha $LINENO (código $?)."' ERR
log INICIO 'Instalação do monitoramento AJA.'
exec 9>/run/lock/aja-infra.lock
flock -n 9 || { log ERRO 'Outro provisionamento AJA está em execução.'; exit 1; }
for file in scripts/monitoramento.sh config/aja-monitor.conf config/aja-monitor.service config/logrotate-aja; do
    [[ -f "$ROOT/$file" ]] || { log ERRO "Arquivo ausente: $file"; exit 1; }
done
for command in curl logrotate systemd-analyze runuser; do
    command -v "$command" >/dev/null || { log ERRO "Dependência ausente: $command"; exit 1; }
done
bash -n "$ROOT/scripts/monitoramento.sh"
if ! id aja-monitor >/dev/null 2>&1; then
    useradd --system --user-group --no-create-home --home-dir /nonexistent --shell /usr/sbin/nologin aja-monitor
else
    [[ "$(id -u aja-monitor)" != 0 && "$(getent passwd aja-monitor | cut -d: -f7)" == /usr/sbin/nologin ]] || {
        log ERRO 'Usuário aja-monitor existente é incompatível.'; exit 1;
    }
fi
install -d -o root -g root -m 0755 /usr/local/lib/aja
install -o root -g root -m 0755 "$ROOT/scripts/monitoramento.sh" /usr/local/lib/aja/monitoramento.sh
install -d -o aja-monitor -g adm -m 0750 /var/log/aja-monitor
for name in recursos alertas; do
    touch "/var/log/aja-monitor/$name.log"
    chown aja-monitor:adm "/var/log/aja-monitor/$name.log"
    chmod 0640 "/var/log/aja-monitor/$name.log"
done
if [[ ! -f /etc/aja-monitor.conf ]]; then
    install -o root -g aja-monitor -m 0640 "$ROOT/config/aja-monitor.conf" /etc/aja-monitor.conf
else
    log INFO 'Configuração existente de limiares preservada em /etc/aja-monitor.conf.'
fi
install -o root -g root -m 0644 "$ROOT/config/aja-monitor.service" /etc/systemd/system/aja-monitor.service
install -o root -g root -m 0644 "$ROOT/config/logrotate-aja" /etc/logrotate.d/aja
systemd-analyze verify /etc/systemd/system/aja-monitor.service
logrotate --debug /etc/logrotate.d/aja

# Coleta real com o mesmo usuário, antes de colocar o processo em segundo plano.
log INFO 'Testando uma coleta real de recursos com o usuário de monitoramento.'
runuser -u aja-monitor -- bash -c '
    set -a
    source /etc/aja-monitor.conf
    set +a
    exec /usr/local/lib/aja/monitoramento.sh --once
'
systemctl daemon-reload
systemctl enable aja-monitor
systemctl reset-failed aja-monitor
systemctl restart aja-monitor
sleep 4
systemctl is-active --quiet aja-monitor
systemctl is-enabled aja-monitor
systemctl show aja-monitor --property=MainPID --property=NRestarts
log FIM 'Monitoramento instalado e ativo. Confira: sudo tail -n 10 /var/log/aja-monitor/recursos.log'
