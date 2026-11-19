# Prompts de apoio ao aprendizado

Projeto: AJA, Global Solution CitizenTech 2026, Operating Systems.

Este documento reúne exemplos propostos de prompts para contextualização,
compreensão de conceitos e orientação de estudo. Não é uma transcrição nem um
registro completo das interações utilizadas no desenvolvimento do projeto.
As aplicações e validações abaixo são propostas de uso, não resultados atribuídos
a conversas que não ocorreram.

## 1. Compreensão do enunciado

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Entender os requisitos de Operating Systems e distinguir exigências
do enunciado de escolhas de implementação.

**Prompt:**

> Leia o documento da Global Solution e me explique a atividade de Operating
> Systems. Quais são os entregáveis, os critérios de avaliação e as demonstrações
> esperadas? Diferencie o que está explicitamente solicitado das alternativas
> técnicas que eu posso escolher. Quero compreender a proposta antes de implementar.

**Aplicação da resposta:** Orientar a leitura do enunciado e a identificação dos
requisitos que deverão ser demonstrados.

**Validação:** Comparar cada requisito mencionado com as páginas da atividade e
com as orientações complementares da tarefa. Esclarecer eventuais divergências
com o professor.

## 2. Reaproveitamento do projeto Mobile

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Compreender a viabilidade de utilizar a aplicação existente no
contexto de hospedagem Web em Linux.

**Prompt:**

> Tenho uma aplicação React Native com Expo, uma API ASP.NET Core, banco MySQL
> e consulta ao ViaCEP. Consigo reaproveitar esse projeto para demonstrar uma
> aplicação Web hospedada no Linux? Explique quais componentes podem ser
> reaproveitados, quais compatibilidades preciso verificar e quais diferenças
> existem entre executar no emulador Android e acessar pelo navegador.

**Aplicação da resposta:** Apoiar a análise da arquitetura e das verificações
necessárias antes da adaptação do ambiente de execução.

**Validação:** Conferir as versões e dependências do projeto, a compatibilidade
do backend com Linux e o funcionamento das jornadas no navegador. Não assumir
compatibilidade apenas com base na explicação da ferramenta.

## 3. Conceitos de Linux, Bash e permissões

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Entender as ferramentas usadas na administração e automação do
servidor.

**Prompt:**

> Tenho pouca experiência com Linux. Explique a diferença entre sistema
> operacional, terminal, shell, Bash, script e serviço. Explique também o papel
> do sudo, dos usuários de serviço e das permissões de arquivos. Relacione esses
> conceitos a um servidor que executa uma API, um banco e um site.

**Aplicação da resposta:** Apoiar o estudo dos conceitos necessários para ler os
scripts e compreender as responsabilidades de cada componente.

**Validação:** Identificar esses elementos no ambiente Linux e explicar, com
palavras próprias, por que processos e credenciais precisam de permissões
diferentes. Conferir o comportamento dos comandos antes de usar privilégios
administrativos.

## 4. Organização das pastas do projeto

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Entender como organizar os arquivos da atividade no repositório.

**Prompt:**

> Como devo organizar a estrutura do meu projeto de Operating Systems? Quais
> pastas devo criar e onde colocar os scripts Bash, as configurações, o código
> da aplicação e as evidências? Me explique uma organização simples para que
> eu consiga manter o projeto organizado e facilitar a avaliação do professor.

**Aplicação da resposta:** Orientar a escolha das pastas e a separação dos arquivos
de acordo com sua função no projeto.

**Validação:** Conferir se os arquivos estão nas pastas correspondentes, se os
caminhos usados nos comandos e no README estão corretos e se os entregáveis
solicitados podem ser encontrados facilmente.

## 5. Guia de comandos para consulta

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Ter um guia de consulta para entender os comandos mais usados
durante a atividade.

**Prompt:**

> Me gere uma lista com os comandos essenciais que mais vou usar nesta atividade.
> Coloque ao lado de cada comando uma explicação simples da sua utilidade e de
> quando devo usá-lo. Indique também se ele deve ser executado no Windows ou no
> Ubuntu e se precisa de sudo. Quero usar essa lista como um guia de consulta
> enquanto estudo e trabalho no projeto. Avise quais comandos podem alterar ou
> apagar arquivos e configurações.

**Aplicação da resposta:** Apoiar o estudo e a consulta dos comandos, ajudando a
entender sua função antes de executá-los no ambiente correspondente.

**Validação:** Conferir a ajuda dos comandos e sua relação com o ambiente do
projeto. Verificar caminhos e argumentos antes da execução e observar o
resultado. Comandos que alteram ou apagam dados devem ser avaliados antes de
qualquer teste.

## 6. Acessar a aplicação pelo Windows

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Entender como acessar no Windows uma aplicação que está na VM.

**Prompt:**

> Minha aplicação vai rodar no Ubuntu dentro do VirtualBox. Como consigo abrir
> ela no navegador do Windows? Qual endereço eu uso? Pode me explicar de um
> jeito simples como o Windows se conecta com a máquina virtual?

**Aplicação da resposta:** Orientar a compreensão do acesso à aplicação e das
configurações de rede necessárias no VirtualBox.

**Validação:** Conferir as configurações do laboratório e tentar abrir a aplicação
no navegador do Windows, com a VM ligada.

## 7. Entender como testar uma falha

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Entender o teste de falha solicitado na atividade.

**Prompt:**

> Como posso fazer testes em um script Bash? O professor pede um teste de falha,
> mas não entendi como provocar uma falha sem estragar a VM. Pode me explicar
> o que eu poderia testar e como saber se o serviço voltou a funcionar sozinho?

**Aplicação da resposta:** Apoiar o planejamento de um teste controlado e a
compreensão do resultado esperado.

**Validação:** Em um teste no laboratório, observar se o serviço volta a funcionar
sem iniciá-lo manualmente e conferir os registros do ocorrido.

## 8. Entender o monitoramento e os logs

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Compreender o que acompanhar no servidor e como conferir os registros.

**Prompt:**

> O professor pede monitoramento de CPU, memória e disco, com alertas e logs.
> O que eu preciso observar nesses recursos? O que são os logs e como posso
> olhar esses registros para saber se o monitoramento está funcionando?

**Aplicação da resposta:** Apoiar a leitura das medições e a compreensão das
mensagens registradas pelo monitoramento.

**Validação:** Consultar os registros na VM e conferir se aparecem novas medições
de CPU, memória e disco, além das mensagens dos testes de alerta.

## 9. Revisão dos entregáveis

**Ferramenta utilizada:** ChatGPT, sugerido para este exemplo.

**Objetivo:** Apoiar a conferência dos entregáveis antes do envio da atividade.

**Prompt:**

> Faça uma revisão do repositório da minha atividade e compare com o enunciado
> que forneci. Procure possíveis inconsistências, pontos a melhorar e itens que
> possam estar faltando. Estou atendendo a todas as demandas? Explique os
> apontamentos e onde devo conferir cada um, sem alterar os arquivos, para que
> eu possa avaliar o que precisa ser ajustado antes da entrega.

**Aplicação da resposta:** Orientar uma conferência final dos arquivos, das
instruções de execução, das evidências e do vídeo em relação ao enunciado.

**Validação:** Conferir os apontamentos diretamente no repositório e no enunciado,
testar os comandos e links relevantes e decidir quais ajustes são necessários.
Uma análise do código, por si só, não comprova que a aplicação funciona na VM
nem substitui os testes e as evidências de execução.
