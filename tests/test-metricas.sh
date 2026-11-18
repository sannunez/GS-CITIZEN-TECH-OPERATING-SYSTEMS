#!/usr/bin/env bash
# Testes de cálculo com snapshots controlados; não simulam validação da VM.
set -Eeuo pipefail
ROOT=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)
source "$ROOT/scripts/monitoramento.sh"
export LC_ALL=C
TMP=$(mktemp -d)
trap 'rm -f -- "$TMP/stat" "$TMP/meminfo"; rmdir -- "$TMP"' EXIT
igual() { [[ "$1" == "$2" ]] || { printf 'Esperado %s, recebido %s\n' "$2" "$1" >&2; exit 1; }; }
igual "$(calcular_cpu 1000 600 1200 650)" 75.0
igual "$(calcular_cpu 1000 600 1200 800)" 0.0
igual "$(calcular_cpu 1000 600 1200 600)" 100.0
if calcular_cpu 1200 800 1000 600 >/dev/null; then echo 'Contador regressivo foi aceito' >&2; exit 1; fi
if calcular_cpu 1000 600 1000 600 >/dev/null; then echo 'Amostra sem intervalo foi aceita' >&2; exit 1; fi
printf 'cpu 100 20 30 400 50 10 20 5 80 10\n' > "$TMP/stat"
igual "$(ler_cpu "$TMP/stat")" '635 450'
printf 'MemTotal: 1000 kB\nMemFree: 10 kB\nCached: 240 kB\nMemAvailable: 250 kB\n' > "$TMP/meminfo"
igual "$(ler_memoria "$TMP/meminfo")" 75.0
printf 'MemTotal: 1000 kB\nMemFree: 10 kB\n' > "$TMP/meminfo"
if ler_memoria "$TMP/meminfo" >/dev/null; then echo 'MemAvailable ausente foi aceito' >&2; exit 1; fi
limite_atingido 80.0 80
limite_atingido 80.1 80
if limite_atingido 79.9 80; then echo 'Alerta abaixo do limite' >&2; exit 1; fi
limite_atingido 0.0 0
printf 'PASS: CPU, idle/iowait, exclusão de guest, memória disponível, contadores inválidos e limites.\n'
