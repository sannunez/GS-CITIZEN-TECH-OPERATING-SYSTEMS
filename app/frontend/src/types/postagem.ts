export type Autor = {
  id: number;
  nome: string;
  biografia: string;
};

export type Postagem = {
  id: number;
  texto: string | null;
  imagemUrl: string | null;
  autor: Autor;
  criadaEm: string;
};

export type PaginaPostagens = {
  itens: Postagem[];
  temMais: boolean;
  proximoCursor: number | null;
};
