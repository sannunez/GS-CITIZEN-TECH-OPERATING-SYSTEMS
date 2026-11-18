#!/usr/bin/env bash
# Exporta apenas diagnósticos e logs selecionados; não exporta credenciais.
set -Eeuo pipefail
umask 027
(( EUID == 0 )) || { echo 'Execute com sudo.' >&2; exit 1; }
OWNER="${SUDO_USER:-}"
[[ -n "$OWNER" && "$OWNER" != root ]] || { echo 'Execute com sudo a partir do seu usuário comum.' >&2; exit 1; }
OWNER_HOME=$(getent passwd "$OWNER" | cut -d: -f6)
OWNER_GROUP=$(id -gn "$OWNER")
[[ -d "$OWNER_HOME" ]] || exit 1
EVIDENCE_ROOT="$OWNER_HOME/evidencias-aja"
RUN_ID=$(date -u +%Y%m%dT%H%M%SZ)-$$
DEST="$EVIDENCE_ROOT/$RUN_ID"
install -d -o "$OWNER" -g "$OWNER_GROUP" -m 0750 "$EVIDENCE_ROOT" "$DEST"

{
    date --iso-8601=seconds
    hostnamectl
    uptime
    printf '\nREDE\n'
    ip -br address
    ip -4 route
    printf '\nDNS\n'
    resolvectl dns enp0s3
    printf '\nDISCO\n'
    df -h /
    printf '\nVERSOES\n'
    dotnet --list-sdks
    mysql --version
    nginx -v 2>&1
    node --version
} > "$DEST/ambiente.txt"

{
    date --iso-8601=seconds
    for unit in aja-monitor aja-api mysql nginx; do
        printf '\nSERVICO: %s\n' "$unit"
        systemctl is-active "$unit" || true
        systemctl is-enabled "$unit" || true
        systemctl show "$unit" --property=MainPID --property=NRestarts --property=ActiveEnterTimestamp
    done
} > "$DEST/servicos.txt"

curl --fail --silent --show-error --max-time 10 http://127.0.0.1/health > "$DEST/saude.json"
printf '\n' >> "$DEST/saude.json"

# Somente logs definidos pelo projeto. Nunca copiar /etc/aja/api.env,
# database-password, histórico do shell ou chaves de autenticação.
for name in provisionamento-infra instalacao-dependencias publicacao-aplicacao instalacao-monitoramento testes-falha; do
    path="/var/log/aja/$name.log"
    if [[ -f "$path" ]]; then
        tail -n 120 "$path" > "$DEST/$name.txt"
    fi
done
tail -n 30 /var/log/aja-monitor/recursos.log > "$DEST/recursos.txt"
tail -n 30 /var/log/aja-monitor/alertas.log > "$DEST/alertas.txt"
journalctl -b -u aja-monitor -n 40 --no-pager > "$DEST/journal-monitor-boot.txt"
journalctl -b -u aja-api -n 40 --no-pager > "$DEST/journal-api-boot.txt"

chown -R "$OWNER:$OWNER_GROUP" "$DEST"
ARCHIVE="$OWNER_HOME/evidencias-aja.tar.gz"
tar -czf "$ARCHIVE" -C "$EVIDENCE_ROOT" "$RUN_ID"
chown "$OWNER:$OWNER_GROUP" "$ARCHIVE"
chmod 0640 "$ARCHIVE"
printf 'Evidências coletadas: %s\nPacote para baixar ao Windows: %s\n' "$DEST" "$ARCHIVE"
