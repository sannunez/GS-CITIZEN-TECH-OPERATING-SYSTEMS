import { solicitar } from '@/services/http';
import type { Autor, PaginaPostagens } from '@/types/postagem';
import type { Topico } from '@/types/topico';

export function buscarUsuario(id: number, signal?: AbortSignal) {
  return solicitar<Autor>(`/api/usuarios/${id}`, { signal });
}

export function atualizarBiografia(id: number, biografia: string, token: string) {
  return solicitar<Autor>(`/api/usuarios/${id}/biografia`, {
    method: 'PATCH',
    body: JSON.stringify({ biografia }),
  }, token);
}

export function buscarPostagensUsuario(id: number, cursor: number | null, signal?: AbortSignal) {
  const consulta = cursor === null ? '' : `?antesDeId=${cursor}`;

  return solicitar<PaginaPostagens>(`/api/usuarios/${id}/postagens${consulta}`, { signal });
}

export function buscarTopicosUsuario(id: number, signal?: AbortSignal) {
  return solicitar<Topico[]>(`/api/usuarios/${id}/topicos`, { signal });
}
