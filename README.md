# AJA — Operating Systems

Infraestrutura Linux da CitizenTech AJA para a Global Solution FIAP 2026.
Aplicação Web Expo, API ASP.NET Core e MySQL hospedados em uma VM Ubuntu Server,
com provisionamento Bash, serviços systemd, monitoramento, alertas e auditoria.
A infraestrutura, as jornadas principais, os testes de recuperação e o retorno
dos serviços após reiniciar foram validados na VM durante a implementação.
Os logs e as evidências técnicas documentam o provisionamento, o funcionamento
da aplicação e a recuperação dos serviços após falhas.

## Integrantes

| Nome | RM |
|---|---|
| Guilherme Santos Nunes | 558989 |
| Kaique Rodrigues Zaffarani | 556677 |
| Pedro Josué Pereira Almeida | 554913 |

## Proposta e arquitetura

A AJA permite que moradores consultem pautas, compartilhem postagens e registrem
denúncias sobre problemas urbanos. O recorte acadêmico prioriza acessibilidade
no Parque Doroteia, em São Paulo. Os registros são de um protótipo e não são
encaminhados automaticamente a órgãos públicos.

```text
Navegador no Windows: http://127.0.0.1:8080
                       | redirecionamento NAT 8080 -> 80
Ubuntu Server          v
                  Nginx:80 ---- arquivos Expo Web
                       |
                       +---- /api e /health -> ASP.NET Core:5080 (loopback)
                                                   |
                                                   +---- MySQL local

systemd: API + monitor Bash, início no boot e recuperação após falha
monitor Bash: /proc + df + /health -> logs de recursos, alertas e journal
ViaCEP: consulta HTTPS feita pelo frontend no navegador
```

O código da aplicação e sua origem estão documentados em [app/ORIGEM.md](app/ORIGEM.md).
Os dados demonstrativos e contas do protótipo não representam cadastros reais.
O laboratório é local; a VM precisa estar ligada para o site abrir.

## Vídeo de demonstração

O vídeo apresenta o provisionamento automático, a aplicação hospedada, o
monitoramento, os logs e os testes de falha, com duração máxima de sete minutos.

[Assistir ao vídeo de demonstração da AJA](https://youtu.be/EfVVVXgOFds)

## Reproduzir a partir do GitHub

Crie a VM conforme a configuração abaixo, instale OpenSSH durante a instalação
do Ubuntu e configure a regra NAT SSH. A partir do Windows, obtenha o repositório:

```powershell
git clone https://github.com/sannunez/GS-CITIZEN-TECH-OPERATING-SYSTEMS.git
cd GS-CITIZEN-TECH-OPERATING-SYSTEMS
powershell -NoProfile -ExecutionPolicy Bypass -File .\scripts\preparar-pacote.ps1
scp -P 2222 .\output\aja-publicacao.tar.gz guilherme@127.0.0.1:~/
ssh -p 2222 guilherme@127.0.0.1
```

Substitua `guilherme` pela conta criada na instalação da nova VM. O SSH acima é
executado no Windows; dentro da VM, `127.0.0.1:2222` não corresponde ao host Windows.
No Ubuntu, execute:

```bash
mkdir -p ~/aja-os
tar -xzf ~/aja-publicacao.tar.gz -C ~/aja-os
cd ~/aja-os
sudo bash scripts/01-provisionar-infra.sh
sudo bash scripts/02-instalar-dependencias.sh
sudo bash scripts/03-publicar-aplicacao.sh
sudo bash scripts/04-instalar-monitoramento.sh
```

Confirme a rede quando o script 01 solicitar. Depois adicione a regra NAT HTTP
descrita na etapa 3 e abra o site. Os detalhes de cada etapa estão abaixo.

## Ambiente inicial

- Windows com VirtualBox e Ubuntu Server 26.04.1 LTS na VM `AJA-Server`.
- 4 GB de RAM, disco de 40 GB e 1 CPU virtual.
- Adaptador 1 em NAT, interface Linux `enp0s3`.
- SSH instalado e ativo. Redirecionamento TCP: `127.0.0.1:2222` no Windows
  para porta `22` no convidado (IP do convidado vazio).
- Acesso: `ssh -p 2222 guilherme@127.0.0.1`. Substitua o usuário em outra VM.

## 1. Provisionar infraestrutura

O script `scripts/01-provisionar-infra.sh` configura:

- Hostname `aja-server` e sua entrada local em `/etc/hosts`.
- IPv4 fixo `10.0.2.15/24`, gateway `10.0.2.2` e DNS NAT `10.0.2.3`.
- Usuário de serviço `aja`, sem login interativo.
- Diretórios `/opt/aja`, `/etc/aja` e `/var/lib/aja`, com permissões específicas.
- Diretório persistente de chaves em `/var/lib/aja/keys`.
- Log em `/var/log/aja/provisionamento-infra.log`, acessível a root e grupo adm.

Os endereços são próprios do NAT padrão do VirtualBox; não são uma configuração
genérica para redes físicas ou máquinas na nuvem. O IPv6 permanece via DHCP/RA.
O script preserva o YAML do instalador e adiciona `90-aja-infra.yaml`, que
sobrescreve as opções IPv4 da mesma interface na configuração combinada.

### Executar no Ubuntu

Após transferir e extrair o pacote conforme as instruções de reprodução:

```bash
sudo bash ~/aja-os/scripts/01-provisionar-infra.sh
```

Durante o teste do Netplan, pressione **Enter** quando for solicitado para
confirmar a rede, se o terminal continuar respondendo. O prazo é de 60 segundos.
O script verifica também o IP aplicado e a resolução IPv4 de `ubuntu.com`.
Sem confirmação ou se essa verificação falhar, restaura seu arquivo anterior e
reaplica a rede. Outros erros interrompem a execução e ficam registrados no log.

O Netplan possui ressalvas de reversão: mantenha o console do VirtualBox disponível.
Se perder acesso, aguarde a reversão, tente reconectar e confira a rede pelo console.
Não reinicie sem conferir os arquivos se a reversão tiver falhado.
Backups ficam em `/var/backups/aja-infra-*`, com acesso exclusivo de root.

Para outra interface ou rede, é possível informar `AJA_INTERFACE`, `AJA_ADDRESS`,
`AJA_GATEWAY`, `AJA_DNS` e `AJA_HOSTNAME` como variáveis ao executar com sudo.
Os valores precisam corresponder à rede real da VM.

### Conferir após executar

```bash
hostname
ip -br address
ip route
getent ahostsv4 ubuntu.com
sudo netplan get
sudo tail -n 30 /var/log/aja/provisionamento-infra.log
```

Abra outra conexão SSH para confirmar que o acesso continua funcionando. Depois,
reinicie a VM e repita a conexão e as verificações de IP/DNS para validar persistência.
O script pode ser executado novamente: reaproveita o usuário e os diretórios e
cria um novo backup antes de testar a configuração de rede.

## 2. Instalar dependências

O script `scripts/02-instalar-dependencias.sh` instala pelo APT do Ubuntu 26.04:

- SDK .NET 10 e runtime ASP.NET Core 10, para compilar e executar a API C#.
- MySQL Server, para persistência dos dados.
- Nginx, para servir o frontend Web e encaminhar as chamadas à API.
- Curl, certificados e unzip, para verificações e transferência da aplicação.

Ele habilita MySQL e Nginx no boot e verifica serviços, versões do .NET, consulta
local ao MySQL e uma resposta HTTP do Nginx. A versão real do MySQL fica registrada
no log e será usada na configuração do provedor da API na etapa seguinte.
Não cria o banco da aplicação, não publica o AJA e não solicita senhas de banco.
A instalação pode demorar vários minutos, principalmente com uma CPU.

No terminal conectado ao Ubuntu:

```bash
sudo bash ~/aja-os/scripts/02-instalar-dependencias.sh
```

O resultado é registrado em `/var/log/aja/instalacao-dependencias.log`.
O APT aguarda até 300 segundos por uma instalação concorrente, como as atualizações
automáticas iniciais. Erros interrompem o script e ficam no log. É possível
executar novamente após corrigir o erro; pacotes já instalados são reaproveitados.
Se outro serviço já ocupar a porta 80, o Nginx poderá falhar e o conflito precisará
ser resolvido antes de prosseguir.

## 3. Publicar a aplicação AJA

O código reaproveitado está em `app/`; sua origem e as adaptações estão em
[`app/ORIGEM.md`](app/ORIGEM.md). O projeto Mobile original foi preservado.
O script `03-publicar-aplicacao.sh` instala Node.js e npm dos repositórios Ubuntu,
compila Expo Web e a API, cria o banco e um usuário exclusivo e configura Nginx e
o serviço `aja-api`. Instalação deve partir de uma conta comum com sudo; os builds
são executados como essa conta, sem privilégios administrativos.

### Executar no Ubuntu

Com o pacote já extraído em `~/aja-os`:

```bash
sudo bash ~/aja-os/scripts/03-publicar-aplicacao.sh
```

Execute os scripts 01 e 02 antes do 03. A primeira compilação na VM pode demorar,
principalmente com uma CPU. Não feche a conexão enquanto o script estiver ativo.
O build Web limita o Metro a um worker. A variável pública `EXPO_PUBLIC_API_URL=/`
é incorporada ao build e faz as requisições usarem `/api/...` na origem do site.
Essa variável contém apenas o endereço da API, nunca uma senha.

### Banco e configurações

- Banco: `aja_helperapp`; usuário: `aja_app`, restrito à conexão local e a esse banco.
- Senha aleatória criada em `/etc/aja/database-password`, apenas root pode ler.
- Ambiente da API: `/etc/aja/api.env`, também apenas root pode ler. O systemd
  fornece o ambiente à API, incluindo conexão e versão real do MySQL instalado.
- Migrações do Entity Framework criam as tabelas e dados demonstrativos no boot.
- As chaves de autenticação persistem em `/var/lib/aja/keys` entre publicações.
- API escuta apenas em `127.0.0.1:5080` dentro da VM.
- Nginx escuta na porta 80, serve os arquivos Web e encaminha `/api/` e `/health`.
- Serviço `aja-api`: início automático, execução como `aja`, reinício após falha,
  acesso de escrita limitado ao diretório de dados e logs no journal.
- Log da publicação: `/var/log/aja/publicacao-aplicacao.log`.

O script é voltado para a VM acadêmica descrita neste projeto. As migrações
alteram o banco e não têm reversão automática na publicação. Novas
execuções preservam senha, banco e chaves; não apagam os dados existentes.
Binários e arquivos Web ficam em diretórios por publicação, acessados por links
`/opt/aja/api` e `/var/www/aja`. Os diretórios anteriores não são apagados.
Configurações anteriores e destinos dos links ficam em `/var/backups/aja-publicacao-*`.
Se a publicação falhar, consulte o log, corrija a causa e execute novamente.

### Abrir no Windows

Adicione outra regra NAT no adaptador 1 do VirtualBox, preservando a regra SSH:

| Nome | Protocolo | IP do hospedeiro | Porta do hospedeiro | IP do convidado | Porta do convidado |
|---|---|---|---|---|---|
| HTTP | TCP | 127.0.0.1 | 8080 | vazio | 80 |

Acesse `http://127.0.0.1:8080`. O uso de HTTP fica restrito ao laboratório local.
Pode criar uma conta de teste ou usar os dados demonstrativos do protótipo:
e-mail `ana@aja.example` e senha `AjaDemo@2026`.

### Conferir o funcionamento

```bash
systemctl is-active aja-api mysql nginx
systemctl is-enabled aja-api
curl -f http://127.0.0.1/health
sudo journalctl -u aja-api -n 60 --no-pager
```

Além do teste de saúde, valide no navegador: login, feed, consulta ao ViaCEP,
cadastro de uma denúncia e persistência após atualizar a página. Confirme também
que os dados e a sessão continuam disponíveis após reiniciar a VM.

## 4. Instalar o monitoramento

O script `scripts/04-instalar-monitoramento.sh` instala um processo Bash contínuo,
executado como o usuário sem login `aja-monitor`, com serviço systemd e rotação
de logs. Não é necessário republicar a aplicação para instalar o monitoramento.

Com o pacote já extraído, execute no Ubuntu:

```bash
sudo bash ~/aja-os/scripts/04-instalar-monitoramento.sh
```

### Medições e alertas

| Recurso | Como é medido | Limiar inicial |
|---|---|---|
| CPU | Diferença dos contadores de `/proc/stat` durante 2 segundos; idle e iowait não são uso ativo; guest não é somado duas vezes | 80% |
| Memória | `(MemTotal - MemAvailable) / MemTotal`, de `/proc/meminfo` | 80% |
| Disco | Percentual ocupado do sistema de arquivos de `/`, usando `df -P` | 85% |
| Aplicação | HTTP de `/health`, passando por Nginx e verificando MySQL pela API | Alerta se indisponível |

O alerta dispara ao atingir ou ultrapassar o limiar. Cada ciclo mede CPU por
2 segundos e consulta os demais recursos; depois aguarda 15 segundos. A consulta
HTTP tem prazo máximo de 5 segundos. Os percentuais registrados são reais.

O arquivo `/etc/aja-monitor.conf` contém os limites e intervalos. Para alterar,
edite com `sudo nano /etc/aja-monitor.conf` e execute
`sudo systemctl restart aja-monitor`. Novas instalações preservam a configuração
existente. Limiares aceitos: inteiros de 0 a 100; amostra: 1 a 10 segundos;
intervalo entre ciclos: 1 a 3600 segundos.

### Serviço e logs

```bash
systemctl is-active aja-monitor
systemctl is-enabled aja-monitor
sudo tail -n 10 /var/log/aja-monitor/recursos.log
sudo tail -n 10 /var/log/aja-monitor/alertas.log
sudo journalctl -u aja-monitor -n 20 --no-pager
```

- `recursos.log`: início, medições, alertas, recuperações e encerramento.
- `alertas.log`: alertas e retorno dos recursos ao normal.
- Journal: saída do monitor e decisões do systemd sobre falha/reinício.
- `/var/log/aja/instalacao-monitoramento.log`: auditoria da instalação.
- `/etc/logrotate.d/aja`: rotação diária ou por tamanho acima de 5 MB, quando
  o logrotate executar; até cinco arquivos antigos, com compressão adiada.

O monitor abre o arquivo novamente em cada registro, permitindo rotação sem
reiniciar o processo. O log inclui horário, nível, modo e PID. Acompanhamento em
tempo real: `sudo tail -f /var/log/aja-monitor/recursos.log`; Ctrl+C encerra apenas
a visualização. O serviço continua funcionando.

O systemd inicia `aja-monitor` no boot e reinicia o processo após uma falha com
espera de três segundos. Há limite de seis tentativas por minuto para evitar
reinício infinito em configuração inválida. Após corrigir uma falha repetida,
use `sudo systemctl reset-failed aja-monitor` e `sudo systemctl restart aja-monitor`.
Uma parada administrativa com `systemctl stop` é intencional e não dispara reinício.
O monitor registra indisponibilidade da API; ele não altera os dados nem tenta
reiniciar MySQL/Nginx por conta própria.

## 5. Testar alertas e recuperação de falhas

Depois de instalar e verificar o monitor, execute no Ubuntu:

```bash
sudo bash ~/aja-os/scripts/05-testar-falhas.sh
```

O teste faz uma coleta real separada, com limiares temporários iguais a zero e
marcação `modo=teste-limiares`, e exige um alerta de CPU, memória e disco.
Isso demonstra o mecanismo de alertas; não representa uma sobrecarga real.
O processo normal continua com seus limites originais, sem alterar o arquivo
de configuração. Não são criados arquivos gigantes nem consumida RAM artificialmente.

Depois o teste envia SIGKILL apenas ao processo principal de `aja-monitor` e
aguarda sua recuperação automática pelo systemd. Confere PID diferente,
incremento de `NRestarts` e uma nova medição feita pelo processo recuperado.
O reinício durante esse teste é feito automaticamente pelo systemd.

Para incluir também uma falha real e breve na API:

```bash
sudo bash ~/aja-os/scripts/05-testar-falhas.sh --com-api
```

Faça isso sem uma operação de cadastro em andamento. O teste encerra o processo
principal da API, aguarda novo PID e incremento de `NRestarts` e confirma que
`/health` voltou a responder. Nenhum banco é apagado. Após o teste, atualize o
site e confira os dados e a sessão no navegador. A falha breve da API pode não
coincidir com a coleta do monitor; o journal e o log de testes documentam o evento.

Resultado e PIDs ficam em `/var/log/aja/testes-falha.log`.
Não altere os limites normais para zero; o script faz isso só no processo de teste.
Após reiniciar a VM, confira também `systemctl is-active aja-monitor aja-api mysql nginx`,
nova medição em `recursos.log` e a resposta de `/health`.

## 6. Exportar as evidências finais

Com o sistema funcionando, execute no Ubuntu:

```bash
sudo bash ~/aja-os/scripts/06-coletar-evidencias.sh
```

O script reúne versões, rede, serviços, saúde, medições, alertas e logs selecionados
em `~/evidencias-aja.tar.gz`. Os arquivos internos têm extensão `.txt` ou `.json`,
para poderem ser anexados como evidências. O pacote não coleta senhas, chaves,
arquivos de ambiente nem histórico do terminal. Revise os textos antes de publicar.

No PowerShell do Windows, dentro do repositório:

```powershell
scp -P 2222 guilherme@127.0.0.1:~/evidencias-aja.tar.gz .\output\
tar -xzf .\output\evidencias-aja.tar.gz -C .\docs\evidencias
```

## Evidências técnicas e resultados

As capturas e os logs estão organizados em
[docs/evidencias](docs/evidencias/README.md). Eles documentam a execução dos scripts,
as medições reais e os testes de recuperação na VM.

- [Execução do provisionamento](docs/evidencias/01-provisionamento-infra.png)
- [Rede e SSH após reiniciar](docs/evidencias/02-rede-apos-reiniciar.png)
- [Dependências instaladas e verificadas](docs/evidencias/03-dependencias-instaladas.png)
- [Publicação no Linux e saúde do banco](docs/evidencias/04-aplicacao-publicada.png)
- [Monitor ativo com medições reais e API disponível](docs/evidencias/05-monitoramento-ativo.png)
- [Alertas e recuperação automática do monitor](docs/evidencias/06-alertas-recuperacao-monitor.png)

| Verificação | Resultado |
|---|---|
| Rede após reiniciar a VM | IP fixo e resolução DNS mantidos |
| Aplicação Web | Login, feed, postagens e denúncias funcionando |
| Integração externa | Endereço consultado pelo ViaCEP |
| Persistência | Denúncia mantida após atualizar a página e reiniciar a VM |
| Monitoramento | Medições contínuas de CPU, memória, disco e saúde da API |
| Alertas | CPU, memória e disco comprovados com limiares temporários identificados como teste |
| Recuperação do monitor | Novo PID, incremento de NRestarts e novas medições após SIGKILL |
| Recuperação da API | Novo PID, incremento de NRestarts e retorno de /health após SIGKILL |
| Inicialização automática | Monitor, API, MySQL e Nginx ativos e habilitados após reboot |

O [índice de evidências](docs/evidencias/README.md) relaciona cada resultado aos
respectivos registros de execução, serviços e testes de falha.
