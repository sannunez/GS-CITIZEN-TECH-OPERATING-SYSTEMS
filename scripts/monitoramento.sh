#!/usr/bin/env bash
# Coleta dados reais do Linux. --once faz uma coleta; sem argumento, repete.
set -Eeuo pipefail

ler_cpu() {
    # Os campos guest/guest_nice já estão em user/nice: não somá-los novamente.
    awk '$1 == "cpu" { print $2+$3+$4+$5+$6+$7+$8+$9, $5+$6; found=1; exit }
         END { if (!found) exit 1 }' "${1:-/proc/stat}"
}

calcular_cpu() {
    awk -v t1="$1" -v i1="$2" -v t2="$3" -v i2="$4" 'BEGIN {
        total=t2-t1; idle=i2-i1;
        if (total<=0 || idle<0 || idle>total) exit 1;
        printf "%.1f\n", 100*(total-idle)/total;
    }'
}

ler_memoria() {
    # MemAvailable considera a memória recuperável; cache não é tratado como cheio.
    awk '/^MemTotal:/ { total=$2 } /^MemAvailable:/ { available=$2; found=1 }
         END {
             if (!found || total<=0 || available<0 || available>total) exit 1;
             printf "%.1f\n", 100*(total-available)/total;
         }' "${1:-/proc/meminfo}"
}

limite_atingido() {
    awk -v value="$1" -v limit="$2" 'BEGIN { exit !(value>=limit) }'
}

registrar() {
    local level="$1" message="$2" line
    line="$(date --iso-8601=seconds) [$level] modo=$RUN_LABEL pid=$$ $message"
    printf '%s\n' "$line" >> "$LOG_DIR/recursos.log"
    if [[ "$level" == ALERTA || "$level" == RECUPERACAO ]]; then
        printf '%s\n' "$line" >> "$LOG_DIR/alertas.log"
    fi
    printf '%s\n' "$line"
}

verificar_limite() {
    local resource="$1" value="$2" limit="$3"
    if limite_atingido "$value" "$limit"; then
        registrar ALERTA "recurso=$resource consumo=${value}% limite=${limit}%"
        EM_ALERTA["$resource"]=1
    elif [[ "${EM_ALERTA[$resource]:-0}" == 1 ]]; then
        registrar RECUPERACAO "recurso=$resource consumo=${value}% limite=${limit}%"
        EM_ALERTA["$resource"]=0
    fi
}

coletar() {
    local before after cpu memory disk api_status
    before=$(ler_cpu)
    sleep "$SAMPLE_SECONDS"
    after=$(ler_cpu)
    # Cada snapshot contém dois inteiros: total e idle.
    read -r TOTAL_ANTES IDLE_ANTES <<< "$before"
    read -r TOTAL_DEPOIS IDLE_DEPOIS <<< "$after"
    cpu=$(calcular_cpu "$TOTAL_ANTES" "$IDLE_ANTES" "$TOTAL_DEPOIS" "$IDLE_DEPOIS")
    memory=$(ler_memoria)
    disk=$(df -P -- "$DISK_PATH" | awk 'NR==2 { gsub(/%/, "", $5); print $5 }')
    [[ "$disk" =~ ^[0-9]+$ && "$disk" -le 100 ]] || return 1
    api_status=ok
    if ! curl --fail --silent --max-time 5 --output /dev/null http://127.0.0.1/health; then
        api_status=indisponivel
    fi
    registrar MEDICAO "cpu=${cpu}% memoria=${memory}% disco=${disk}% caminho=$DISK_PATH api=$api_status"
    verificar_limite CPU "$cpu" "$CPU_LIMIT"
    verificar_limite MEMORIA "$memory" "$MEMORY_LIMIT"
    verificar_limite DISCO "$disk" "$DISK_LIMIT"
    if [[ "$api_status" != ok ]]; then
        registrar ALERTA 'recurso=API estado=indisponivel endpoint=/health'
        EM_ALERTA[API]=1
    elif [[ "${EM_ALERTA[API]:-0}" == 1 ]]; then
        registrar RECUPERACAO 'recurso=API estado=ok endpoint=/health'
        EM_ALERTA[API]=0
    fi
}

main() {
    local once=0 value
    if [[ "${1:-}" == --once && $# == 1 ]]; then
        once=1
    elif (( $# != 0 )); then
        echo 'Uso: monitoramento.sh [--once]' >&2
        return 2
    fi
    export LC_ALL=C
    CPU_LIMIT="${CPU_LIMIT:-80}"
    MEMORY_LIMIT="${MEMORY_LIMIT:-80}"
    DISK_LIMIT="${DISK_LIMIT:-85}"
    SAMPLE_SECONDS="${SAMPLE_SECONDS:-2}"
    INTERVAL_SECONDS="${INTERVAL_SECONDS:-15}"
    DISK_PATH="${DISK_PATH:-/}"
    LOG_DIR=/var/log/aja-monitor
    RUN_LABEL="${RUN_LABEL:-normal}"
    declare -gA EM_ALERTA=()

    for value in "$CPU_LIMIT" "$MEMORY_LIMIT" "$DISK_LIMIT"; do
        [[ "$value" =~ ^(0|[1-9][0-9]?|100)$ ]] || {
            echo 'Limiares devem ser inteiros entre 0 e 100.' >&2; return 2;
        }
    done
    [[ "$SAMPLE_SECONDS" =~ ^([1-9]|10)$ ]] || return 2
    [[ "$INTERVAL_SECONDS" =~ ^[1-9][0-9]{0,3}$ && "$INTERVAL_SECONDS" -le 3600 ]] || return 2
    [[ "$RUN_LABEL" == normal || "$RUN_LABEL" == teste-limiares ]] || return 2
    [[ -d "$DISK_PATH" && -w "$LOG_DIR" ]] || {
        echo 'Confira o caminho do disco e instale o monitoramento pelo script 04.' >&2; return 1;
    }

    umask 027
    registrar INICIO "cpu_limite=$CPU_LIMIT memoria_limite=$MEMORY_LIMIT disco_limite=$DISK_LIMIT amostra=${SAMPLE_SECONDS}s intervalo=${INTERVAL_SECONDS}s"
    trap 'registrar ERRO "Monitoramento interrompido por erro na linha $LINENO."' ERR
    trap 'registrar FIM "Monitoramento encerrado por sinal."; exit 0' TERM INT
    while true; do
        coletar
        if (( once == 1 )); then
            registrar FIM 'Coleta única concluída.'
            break
        fi
        sleep "$INTERVAL_SECONDS"
    done
}

if [[ "${BASH_SOURCE[0]}" == "$0" ]]; then
    main "$@"
fi
