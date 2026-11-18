import type { PaginaPostagens, Postagem } from '@/types/postagem';
import { solicitar } from '@/services/http';

type NovaPostagem = {
  texto: string | null;
  imagemUrl: string | null;
  usuarioId: number;
};

export function buscarMinhasPostagens(token: string, signal?: AbortSignal) {
  return solicitar<Postagem[]>('/api/postagens/minhas', { signal }, token);
}

export function excluirPostagem(id: number, token: string) {
  return solicitar<void>(`/api/postagens/${id}`, { method: 'DELETE' }, token);
}

export function criarPostagem(dados: NovaPostagem, token: string) {
  return solicitar(
    '/api/postagens',
    {
      method: 'POST',
      body: JSON.stringify(dados),
    },
    token,
  );
}

export async function buscarPostagens(cursor: number | null, signal?: AbortSignal): Promise<PaginaPostagens> {
  const parametros = new URLSearchParams({ limite: '3' });

  if (cursor !== null) {
    parametros.set('antesDeId', String(cursor));
  }

  const pagina = await solicitar<PaginaPostagens>(`/api/postagens/feed?${parametros}`, { signal });

  if (
    !Array.isArray(pagina.itens) ||
    typeof pagina.temMais !== 'boolean' ||
    (pagina.temMais && (typeof pagina.proximoCursor !== 'number' || pagina.proximoCursor <= 0))
  ) {
    throw new Error('Não foi possível interpretar as postagens recebidas.');
  }

  return pagina;
}
