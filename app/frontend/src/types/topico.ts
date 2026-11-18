import type { Autor } from '@/types/postagem';

export type Topico = {
  id: number;
  titulo: string;
  texto: string;
  assuntos: string[];
  postagemId: number | null;
  autor: Autor;
  interesses: number;
};
