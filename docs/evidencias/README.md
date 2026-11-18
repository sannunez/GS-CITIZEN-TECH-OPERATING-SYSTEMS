# Evidências da infraestrutura AJA

As capturas documentam as etapas de configuração e validação da infraestrutura. O diretório
`20261117T033957Z-2174` contém diagnósticos coletados diretamente da VM após os
testes e uma reinicialização. Os horários nos logs estão em UTC.

## Registros por requisito

| Requisito | Evidência |
|---|---|
| Configuração automática da infraestrutura | [Captura](01-provisionamento-infra.png), [log](20261117T033957Z-2174/provisionamento-infra.txt) |
| Rede persistente após reiniciar | [Captura](02-rede-apos-reiniciar.png), [ambiente](20261117T033957Z-2174/ambiente.txt) |
| Dependências | [Captura](03-dependencias-instaladas.png), [log](20261117T033957Z-2174/instalacao-dependencias.txt) |
| Aplicação publicada | [Captura](04-aplicacao-publicada.png), [log](20261117T033957Z-2174/publicacao-aplicacao.txt), [saúde](20261117T033957Z-2174/saude.json) |
| Monitoramento real | [Captura](05-monitoramento-ativo.png), [medições](20261117T033957Z-2174/recursos.txt) |
| Alertas e retorno ao normal | [Alertas](20261117T033957Z-2174/alertas.txt), [journal](20261117T033957Z-2174/journal-monitor-boot.txt) |
| Falha e recuperação automática | [Captura do monitor](06-alertas-recuperacao-monitor.png), [teste completo com API](20261117T033957Z-2174/testes-falha.txt) |
| Inicialização automática após reboot | [Serviços](20261117T033957Z-2174/servicos.txt), [journal da API](20261117T033957Z-2174/journal-api-boot.txt) |

## Resultados conferidos

- Monitor, API, MySQL e Nginx: `active` e `enabled` após reiniciar a VM.
- Saúde final: `status=ok` e `database=MySQL`.
- Alertas de CPU, memória e disco demonstrados em coleta real com limiares
  temporários iguais a zero, identificados como `modo=teste-limiares`.
- Monitor recuperado após SIGKILL: PID 1795 → 2218, com `NRestarts` 0 → 1.
- No teste completo seguinte: monitor 2218 → 2457, com `NRestarts` 1 → 2.
- API recuperada após SIGKILL: PID 1143 → 2537, com `NRestarts` 0 → 1, seguida
  de sucesso de `/health`.
- Após reboot: monitor com PID 829 e API com PID 1169. O contador `NRestarts=0`
  da nova sessão não contradiz as recuperações anteriores registradas nos testes.
- Durante o boot houve CPU real de 100% (limiar normal de 80%) e indisponibilidade
  transitória da API, ainda iniciando. Ambos geraram alertas normais e depois
  registros de recuperação. Não houve falha permanente.
- As medições seguintes mostram os recursos acompanhados continuamente, com
  a API disponível.

Login, feed, ViaCEP, criação de denúncia, alteração de situação e persistência
foram validados no navegador. A demonstração deve apresentar essas jornadas
e o provisionamento automático em execução.
