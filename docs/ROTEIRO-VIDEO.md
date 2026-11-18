# Roteiro de demonstração — Operating Systems

Meta: aproximadamente 6min50s; limite do enunciado: 7 minutos.
Grave terminal e navegador com texto legível e sem janelas pessoais sobrepostas.
Feche notificações. Autentique o sudo antes de limpar o terminal. Senhas devem
ser digitadas apenas no prompt de senha, que não mostra caracteres.

O ambiente é uma VM Ubuntu Server local: não é necessário publicar o site na
internet. A aplicação chama-se AJA, parte da CitizenTech desenvolvida pela equipe.

## Preparação

- Deixe a VM ligada e abra o SSH em uma aba do PowerShell do Windows.
- Abra `http://127.0.0.1:8080` e deixe uma conta de demonstração pronta.
- Abra o repositório local/GitHub, sem arquivos de credenciais.
- Teste o áudio e o tamanho das letras antes de gravar.
- Entre em `cd ~/aja-os` no terminal do Ubuntu.
- Faça `sudo -v` e depois `clear`.
- Deixe outra aba para consultar logs durante as demonstrações.
- Edite ou acelere somente esperas de downloads/builds, deixando claro o corte.
  A gravação precisa mostrar scripts sendo executados e seus resultados reais.

## Sequência sugerida

| Tempo | Mostrar | Explicação sugerida |
|---|---|---|
| 0:00–0:25 | Nome do projeto e integrantes | “A AJA permite participação da comunidade e registro de problemas urbanos. Nesta entrega automatizamos sua infraestrutura Linux.” |
| 0:25–0:55 | VM, sistema e rede | “Ubuntu Server no VirtualBox, com IP fixo, DNS e acesso SSH. O site e a API executam na VM.” |
| 0:55–1:15 | Transferência SCP no PowerShell do Windows e extração no Ubuntu | “Transferimos os arquivos ao servidor; a instalação é executada dentro do Linux.” |
| 1:15–1:50 | Execução do script 01 e confirmação do Netplan | “Este Bash configura hostname, IP, DNS, usuário de serviço, diretórios e permissões. Mantém backup e testa a rede antes de confirmar.” |
| 1:50–3:00 | Scripts 02 e 03 sendo executados, com cortes nas esperas | “Instalamos dependências, compilamos a aplicação, criamos o banco e configuramos Nginx e systemd. Credenciais são geradas na VM.” |
| 3:00–3:45 | Abrir http://127.0.0.1:8080 no navegador do Windows e usar o AJA | “O frontend Expo Web acessa a API ASP.NET Core e o MySQL. Demonstramos feed, denúncia, ViaCEP e persistência após F5.” |
| 3:45–4:25 | Script 04, serviço e medições | “O Bash lê consumo real de CPU, memória e disco. Registra alertas, verifica a saúde da API e inicia automaticamente no boot.” |
| 4:25–5:30 | Script 05 com `--com-api` | “Limiares temporários de zero demonstram os alertas com medições reais. Depois encerramos os processos e o systemd os recupera; PID e contador de reinícios comprovam isso.” |
| 5:30–6:15 | Reinicialização, serviços, saúde e dados | “Após reiniciar a VM, os serviços iniciam sozinhos, o monitor volta a registrar e os dados permanecem.” |
| 6:15–6:50 | Logs, README, evidências e repositório | “O repositório reúne as fontes, scripts, configurações e instruções de reprodução. Os logs registram execução e resultados dos testes.” |

## Comandos de apoio

O [roteiro passo a passo](ROTEIRO-GRAVACAO-PASSO-A-PASSO.md) contém os comandos
completos da transferência SCP no Windows, da extração no Ubuntu e da abertura
do site no navegador. Mantenha uma aba no PowerShell e outra conectada por SSH.

Identificação do ambiente:

```bash
hostnamectl
ip -br address
resolvectl dns enp0s3
```

Provisionamento (na ordem, dentro de `~/aja-os`):

```bash
sudo bash scripts/01-provisionar-infra.sh
sudo bash scripts/02-instalar-dependencias.sh
sudo bash scripts/03-publicar-aplicacao.sh
sudo bash scripts/04-instalar-monitoramento.sh
```

Confira a confirmação da rede no script 01. Ao repetir o 03, o banco, sua senha
e as chaves são preservados; a aplicação é recompilada e publicada novamente.
Os códigos de saída e logs ajudam a demonstrar o resultado.

Monitor e teste de falha:

```bash
systemctl is-active aja-monitor
sudo tail -n 5 /var/log/aja-monitor/recursos.log
sudo bash scripts/05-testar-falhas.sh --com-api
sudo tail -n 15 /var/log/aja/testes-falha.log
```

Não faça cadastro ou edição enquanto a API é interrompida no teste. Não diga que
o teste de limite zero é uma sobrecarga de 80%; explique a demonstração dos alertas.

Após `sudo reboot`, reconecte pelo Windows:

```powershell
ssh -p 2222 guilherme@127.0.0.1
```

Já no Ubuntu:

```bash
systemctl is-active aja-monitor aja-api mysql nginx
curl -f http://127.0.0.1/health
sudo tail -n 5 /var/log/aja-monitor/recursos.log
```

No navegador, atualize o feed e a denúncia de teste. Mostre que a informação
continua salva e que os serviços iniciaram sem comandos manuais de start.

## Encerramento da entrega

Publique o vídeo em um destino acessível ao avaliador, adicione o link ao README
e confira se abre em uma janela sem sua sessão. O enunciado de OS exige vídeo
de até 7 minutos; não determina no PDF uma plataforma específica para esse vídeo.
Um integrante envia o link do repositório na tarefa de Operating Systems no Teams,
até 19/11/2026 às 23h55, conforme as páginas 35–37 do enunciado.
