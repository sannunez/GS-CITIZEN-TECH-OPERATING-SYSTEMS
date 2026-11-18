# Gravação passo a passo — AJA Operating Systems

Meta de duração final: 6min50s, no máximo 7 minutos. Pode gravar em partes e
cortar as esperas. Mostre o comando antes do corte e seu resultado depois.
Ao repetir os scripts, estamos reaplicando o provisionamento em uma VM já
preparada. Não apresente essa gravação como uma instalação do zero.

## Antes de gravar

Ligue a VM no VirtualBox e aguarde o Ubuntu iniciar. No navegador do Windows,
digite `http://127.0.0.1:8080` na barra de endereços e pressione Enter. Aguarde
a página carregar e faça login. Os serviços iniciam automaticamente; não é
necessário executar Expo ou a API manualmente. Deixe também a pasta do projeto e o GitHub
abertos; mostre o GitHub apenas depois de publicar os arquivos.

Configure o gravador para capturar a tela e o microfone. Feche notificações e
janelas pessoais. Faça um teste curto de áudio e legibilidade antes da gravação.
Pode ler as falas abaixo; não é necessário decorar nem explicar cada linha.

Mantenha duas abas de terminal: uma no PowerShell do Windows (`PS C:\...>`) para
transferir arquivos e outra conectada ao Ubuntu para executar os scripts. Abra
a segunda aba do PowerShell e conecte nela:

```powershell
ssh -p 2222 guilherme@127.0.0.1
```

Digite a senha de login. Quando aparecer `guilherme@aja-server`, estará no Ubuntu.
Nessa mesma aba conectada, execute antes de gravar:

```bash
cd ~/aja-os
sudo -v
clear
```

O sudo pode pedir sua senha. Digite somente no prompt invisível. Se pedir durante
a gravação, corte esse trecho; nunca digite a senha na linha normal de comandos.
Depois de cada comando, aguarde o retorno da linha `guilherme@aja-server` antes
de executar o próximo. Todos os comandos de Bash a seguir são nessa aba Ubuntu.

## 1 — Apresentar (0:00–0:25)

Tela: navegador com o AJA aberto ou README com nome e integrantes.

Fala: “Este é o projeto AJA, uma CitizenTech para participação da comunidade e
registro de problemas urbanos. Nesta entrega de Operating Systems, montamos uma
infraestrutura Linux automatizada para hospedar e monitorar a aplicação.”

## 2 — Mostrar Linux e rede (0:25–0:55)

Tela: terminal conectado ao Ubuntu. Execute um por vez:

```bash
hostnamectl
ip -br address
resolvectl dns enp0s3
```

Mostre Ubuntu, hostname `aja-server`, IP `10.0.2.15` e DNS `10.0.2.3`.

Fala: “O servidor é uma máquina virtual Ubuntu Server no VirtualBox. Ela possui
IP fixo, DNS configurado e acesso remoto por SSH. O Windows é usado para acessar
o terminal e abrir o site; a aplicação executa dentro do Linux.”

## 3 — Transferir pelo PowerShell e extrair no Ubuntu (0:55–1:15)

Tela: aba do PowerShell do Windows, cuja linha começa com `PS C:\...>`.
Execute:

```powershell
scp -P 2222 "C:\Users\gsant\OneDrive\Desktop\FIAP\GS-CITIZEN-TECH-OPERATING-SYSTEMS\output\aja-publicacao.tar.gz" guilherme@127.0.0.1:~/
```

Digite a senha apenas se for solicitada e espere a transferência terminar.
Corte a espera/autenticação da gravação. Mostre a transferência concluída.

Fala: “Transferimos do Windows para o servidor Linux um pacote com os códigos,
scripts e configurações usando SCP. A instalação será executada dentro do Ubuntu.”

Mude para a aba conectada ao Ubuntu (`guilherme@aja-server`) e execute:

```bash
mkdir -p ~/aja-os
tar -xzf ~/aja-publicacao.tar.gz -C ~/aja-os
cd ~/aja-os
```

Fala: “No Linux, extraímos o pacote e entramos na pasta do projeto para executar
o provisionamento.” A extração pode terminar sem mensagem, o que é normal.

## 4 — Provisionar infraestrutura (1:15–1:50)

Tela: terminal Ubuntu, dentro de `~/aja-os`.

```bash
sudo bash scripts/01-provisionar-infra.sh
```

Quando aparecer “Do you want to keep these settings?”, pressione Enter se a
conexão continuar respondendo. Mostre os resultados OK e a conclusão do script.

Fala: “Este script Bash configura hostname, IP, gateway, DNS, usuário de serviço,
diretórios e permissões. Ele registra a execução em log e testa a alteração de
rede antes de mantê-la. Estamos reaplicando essa configuração no ambiente já instalado.”

## 5 — Instalar e publicar (1:50–3:00)

Tela: terminal Ubuntu. Execute primeiro o 02, aguarde terminar, depois o 03:

```bash
sudo bash scripts/02-instalar-dependencias.sh
```

Fala: “O segundo script instala .NET 10, MySQL, Nginx e ferramentas auxiliares.
Os pacotes existentes são reaproveitados, e os serviços são habilitados no boot.”

```bash
sudo bash scripts/03-publicar-aplicacao.sh
```

Fala: “O terceiro compila o frontend Expo para Web e a API C#, prepara o banco e
configura o Nginx e o serviço da API. A senha do banco é gerada na VM. As
publicações preservam os dados e as chaves da aplicação.”

Corte ou acelere as esperas de instalação/compilação, indicando “espera acelerada”.
Mostre o início de cada comando e a conclusão, incluindo `status=ok` e
“Publicação concluída”. Não precisa narrar toda a saída. Essa parte pode durar
vários minutos na gravação bruta; a faixa de tempo é para o vídeo editado.

## 6 — Abrir e demonstrar o AJA (3:00–3:45)

Tela: navegador do Windows (Chrome, Edge ou o navegador habitual). Clique na
barra de endereços, digite `http://127.0.0.1:8080` e pressione Enter. Atualize com
Ctrl+F5 após publicar. Não digite esse endereço como comando no Ubuntu.
Mostre o feed. Abra a criação de denúncia, consulte um CEP válido e mostre o
endereço preenchido. Use texto identificado como teste acadêmico, salve e pressione F5.
Mostre que a denúncia permanece. Pode deixar CEP e texto preparados antes de gravar.

Fala: “O Nginx disponibiliza o frontend e encaminha as chamadas para a API
ASP.NET Core. A API salva os dados no MySQL. Aqui temos o feed e uma denúncia com
endereço consultado no ViaCEP. Após atualizar a página, o registro permanece salvo.”

## 7 — Monitoramento e logs (3:45–4:25)

Tela: terminal Ubuntu.

```bash
sudo bash scripts/04-instalar-monitoramento.sh
```

Depois da conclusão:

```bash
systemctl is-active aja-monitor
sudo tail -n 5 /var/log/aja-monitor/recursos.log
```

Mostre `active`, CPU, memória, disco e `api=ok`.

Fala: “O monitor é outro script Bash, executado continuamente como serviço.
Ele mede CPU, memória e disco e verifica a saúde da API. Os limites normais são
80% para CPU e memória e 85% para disco. Medições, alertas e recuperações são
registrados em arquivos e no journal.”

## 8 — Falha e recuperação (4:25–5:30)

Tela: terminal Ubuntu. Não mantenha uma operação de cadastro/edição em andamento.

```bash
sudo bash scripts/05-testar-falhas.sh --com-api
```

Fala durante os alertas: “Primeiro fazemos uma coleta real separada com limites
temporários iguais a zero para demonstrar os três alertas. Isso não é uma
sobrecarga artificial e não muda os limites do monitor normal.”

Fala durante a recuperação: “Depois encerramos os processos do monitor e da
API para simular falhas reais. O systemd os reinicia automaticamente. O novo PID,
o aumento do contador de reinícios e a volta das medições e da saúde comprovam
a recuperação.”

Mostre as mensagens “recuperado automaticamente”, “API recuperada” e “Testes
concluídos”. Não execute `systemctl restart` no lugar do teste: é a recuperação
automática que estamos demonstrando.

## 9 — Reiniciar e conferir (5:30–6:15)

Tela: terminal Ubuntu.

```bash
sudo reboot
```

Fala: “Também verificamos que a infraestrutura volta a funcionar após reiniciar,
sem iniciar manualmente cada componente.”

A conexão SSH vai terminar. Corte a espera do boot. No PowerShell do Windows,
conecte novamente:

```powershell
ssh -p 2222 guilherme@127.0.0.1
```

Depois do login, já no Ubuntu:

```bash
systemctl is-active aja-monitor aja-api mysql nginx
curl -f http://127.0.0.1/health
sudo tail -n 3 /var/log/aja-monitor/recursos.log
```

Se pedir senha de sudo, autentique e corte essa espera. Aguarde uma nova medição
e o retorno de `/health`; o monitor pode registrar a API ainda iniciando logo no boot.
Mostre quatro linhas `active`, saúde `ok` e medições recentes. Volte ao navegador,
atualize a denúncia e mostre o registro preservado.

Fala: “Os quatro serviços estão ativos após o boot, a API acessa o MySQL e o
monitor voltou a registrar. A denúncia continua salva.”

## 10 — Encerrar com a entrega (6:15–6:50)

Tela: GitHub com os arquivos publicados. Mostre brevemente README, scripts,
configurações e `docs/evidencias`. Os links do README ajudam a localizar os logs.

Fala: “O repositório reúne o código, os scripts Bash, as configurações dos
serviços, as evidências técnicas e as instruções para reproduzir o ambiente.
Demonstramos provisionamento, aplicação hospedada, monitoramento, auditoria e
recuperação de falhas.”

Finalize antes de sete minutos. Depois publique o vídeo, acrescente seu link ao
README e confira se tanto o vídeo quanto o repositório abrem para o avaliador.

## Resumo de onde executar

| Local | Ação |
|---|---|
| VirtualBox no Windows | Ligar a VM; manter ligada durante a demonstração |
| PowerShell do Windows | Executar `ssh -p 2222 guilherme@127.0.0.1` |
| Aba conectada ao Ubuntu | `cd`, scripts `.sh`, consultas de serviços/logs e reboot |
| Navegador Windows | Usar o AJA e mostrar o GitHub |

Não precisa recriar a VM, formatar o banco nem executar o coletor 06 no vídeo.
As evidências já estão organizadas. O vídeo mostra a execução e os resultados
dos componentes que atendem aos requisitos.
