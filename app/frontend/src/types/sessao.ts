export type UsuarioSessao = {
  id: number;
  nome: string;
  email: string;
  biografia: string;
};

export type Sessao = {
  token: string;
  expiraEm: number;
  usuario: UsuarioSessao;
};
