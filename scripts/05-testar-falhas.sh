#!/usr/bin/env bash
# Teste padrão: alertas e falha do monitor. --com-api também interrompe a API.
set -Eeuo pipefail
umask 027
(( EUID == 0 )) || { echo 'Execute com sudo.' >&2; exit 1; }
WITH_API=0
if [[ "${1:-}" == --com-api && $# == 1 ]]; then
    WITH_API=1
elif (( $# != 0 )); then
    echo 'Uso: sudo bash scripts/05-testar-falhas.sh [--com-api]' >&2
    exit 2
fi
install -d -o root -g adm -m 0750 /var/log/aja
LOG=/var/log/aja/testes-falha.log
touch "$LOG"
chown root:adm "$LOG"
chmod 0640 "$LOG"
exec > >(tee -a "$LOG") 2>&1
log() { printf '%s [%s] %s\n' "$(date --iso-8601=seconds)" "$1" "$2"; }
TEMP_OUTPUT=$(mktemp)
trap 'rm -f -- "$TEMP_OUTPUT"' EXIT
trap 'log ERRO "Teste interrompido na linha $LINENO (código $?)."' ERR
exec 9>/run/lock/aja-infra.lock
flock -n 9 || { log ERRO 'Outro provisionamento ou teste AJA está em execução.'; exit 1; }

testar_reinicio() {
    local unit="$1" pid_before pid_after restarts_before restarts_after recovered=0
    systemctl is-active --quiet "$unit"
    pid_before=$(systemctl show "$unit" --property=MainPID --value)
    restarts_before=$(systemctl show "$unit" --property=NRestarts --value)
    [[ "$pid_before" =~ ^[0-9]+$ && "$pid_before" -gt 1 ]]
    log TESTE "Falha real em $unit: encerrando processo principal PID=$pid_before com SIGKILL."
    # O systemd seleciona o processo do serviço; não usamos pkill nem padrões globais.
    systemctl kill --kill-whom=main --signal=SIGKILL "$unit"
    for (( attempt=1; attempt<=45; attempt++ )); do
        sleep 1
        pid_after=$(systemctl show "$unit" --property=MainPID --value)
        restarts_after=$(systemctl show "$unit" --property=NRestarts --value)
        if systemctl is-active --quiet "$unit" && [[ "$pid_after" -gt 1 && "$pid_after" != "$pid_before" && "$restarts_after" -gt "$restarts_before" ]]; then
            recovered=1
            break
        fi
    done
    if (( recovered != 1 )); then
        log ERRO "$unit não se recuperou no prazo. Confira sudo journalctl -u $unit -n 40 --no-pager."
        return 1
    fi
    log OK "$unit recuperado automaticamente: PID $pid_before -> $pid_after; NRestarts $restarts_before -> $restarts_after."
    if [[ "$unit" == aja-monitor ]]; then
        local measured=0
        for (( attempt=1; attempt<=15; attempt++ )); do
            if grep -F "[MEDICAO] modo=normal pid=$pid_after " /var/log/aja-monitor/recursos.log >/dev/null; then
                measured=1
                break
            fi
            sleep 1
        done
        (( measured == 1 )) || { log ERRO 'Monitor reiniciou, mas não registrou nova medição.'; return 1; }
        log OK 'O novo processo do monitor voltou a registrar medições.'
    fi
}

log INICIO 'Teste de alertas com medições reais e limiares temporários iguais a zero.'
log INFO 'Não haverá consumo artificial de RAM/disco. A configuração normal do serviço não será alterada.'
systemctl is-active --quiet aja-monitor
runuser -u aja-monitor -- env CPU_LIMIT=0 MEMORY_LIMIT=0 DISK_LIMIT=0 RUN_LABEL=teste-limiares \
    /usr/local/lib/aja/monitoramento.sh --once | tee "$TEMP_OUTPUT"
for resource in CPU MEMORIA DISCO; do
    grep -F '[ALERTA] modo=teste-limiares ' "$TEMP_OUTPUT" | grep -F "recurso=$resource " >/dev/null
    log OK "Alerta de $resource registrado em coleta real com limiar de demonstração."
done
testar_reinicio aja-monitor

if (( WITH_API == 1 )); then
    log AVISO 'A API ficará indisponível brevemente durante o teste solicitado.'
    curl --fail --silent --max-time 5 --output /dev/null http://127.0.0.1/health
    testar_reinicio aja-api
    API_READY=0
    for (( attempt=1; attempt<=30; attempt++ )); do
        if curl --fail --silent --max-time 3 --output /dev/null http://127.0.0.1/health; then
            API_READY=1
            break
        fi
        sleep 1
    done
    (( API_READY == 1 )) || { log ERRO 'A API reiniciou, mas /health não confirmou recuperação.'; exit 1; }
    log OK 'API recuperada; /health voltou a confirmar conexão com MySQL.'
fi
log FIM "Testes concluídos. Evidência registrada em $LOG."
