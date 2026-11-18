import { solicitar } from '@/services/http';
import type { Topico } from '@/types/topico';

type Interesse = {
  id: number;
  interesses: number;
};

const curtidas = new Map<string, Interesse>();

const requisicoes = new Map<string, Promise<Interesse>>();

function chaveInteresse(usuarioId: number, topicoId: number) {
  return `${usuarioId}:${topicoId}`;
}

export function jaDemonstrouInteresse(usuarioId: number, topicoId: number) {
  return curtidas.has(chaveInteresse(usuarioId, topicoId));
}

export function demonstrarInteresse(topicoId: number, usuarioId: number, token: string) {
  const chave = chaveInteresse(usuarioId, topicoId);
  const curtida = curtidas.get(chave);

  if (curtida) {
    return Promise.resolve(curtida);
  }

  const pendente = requisicoes.get(chave);

  if (pendente) {
    return pendente;
  }

  const requisicao = solicitar<Interesse>(`/api/topicos/${topicoId}/interesse`, { method: 'POST' }, token)
    .then(resultado => {
      curtidas.set(chave, resultado);

      return resultado;
    })
    .finally(() => {
      requisicoes.delete(chave);
    });

  requisicoes.set(chave, requisicao);

  return requisicao;
}

export function buscarTopicos(signal?: AbortSignal) {
  return solicitar<Topico[]>('/api/topicos', { signal });
}
