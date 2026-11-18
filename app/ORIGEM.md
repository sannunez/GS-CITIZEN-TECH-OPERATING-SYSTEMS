# Código da aplicação AJA

Esta pasta contém uma cópia do código do repositório Mobile AJA, para tornar a
entrega de Operating Systems reproduzível sem depender de outra pasta do Windows.

- Repositório local de origem: `GS-CITIZEN-TECH-MOBILE-DEVELOPMENT`.
- Commit de origem: `20d813f060fab0b4d3ae21a6b44b1584aa996c67`.
- A árvore de origem estava sem modificações ao realizar a cópia.
- Foram excluídos dependências instaladas, binários, caches, arquivos `.env` e Git.
- O código original do projeto Mobile foi preservado.

Adaptações nesta cópia para a entrega de OS:

1. Conexão de desenvolvimento removida do `appsettings.json`. O script de publicação
   configura credenciais locais por variável de ambiente, fora do repositório.
2. Versão padrão do MySQL atualizada para 8.4.13. A publicação consulta a versão
   real do servidor e a informa por variável de ambiente.
3. Origens CORS de desenvolvimento removidas. Nginx serve frontend e API na mesma
   origem. O frontend é compilado com `EXPO_PUBLIC_API_URL=/` para chamar `/api/...`.
4. MySqlConnector fixado em 2.6.1 para resolver o alerta NU1902 observado na análise
   do projeto original. Referência: [aviso GHSA-473q-m89c-ghf8](https://github.com/advisories/GHSA-473q-m89c-ghf8).
5. Consulta paginada do feed passou a usar o mesmo cliente HTTP das demais telas.
   Antes ela construía `new URL('/api/postagens/feed')` quando a base Web era
   relativa, causando erro antes da requisição. A correção mantém paginação,
   cancelamento e suporte às URLs absolutas usadas no aplicativo Android.

As contas e os dados demonstrativos continuam sendo os do protótipo acadêmico.
Este ambiente é local, acessado pelo Windows via redirecionamento NAT.
