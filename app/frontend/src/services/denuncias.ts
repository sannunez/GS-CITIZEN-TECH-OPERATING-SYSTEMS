import { solicitar } from '@/services/http';
import type { DadosDenuncia, Denuncia } from '@/types/denuncia';

export function buscarDenuncias(token: string, minhas = false, signal?: AbortSignal) {
  return solicitar<Denuncia[]>(`/api/denuncias${minhas ? '/minhas' : ''}`, { signal }, token);
}

export function obterDenuncia(id: number, token: string, signal?: AbortSignal) {
  return solicitar<Denuncia>(`/api/denuncias/${id}`, { signal }, token);
}

export function salvarDenuncia(dados: DadosDenuncia, token: string, id?: number) {
  return solicitar<Denuncia>(
    id ? `/api/denuncias/${id}` : '/api/denuncias',
    {
      method: id ? 'PUT' : 'POST',
      body: JSON.stringify(dados),
    },
    token,
  );
}

export function alterarSituacaoDenuncia(id: number, solucionado: boolean, token: string) {
  return solicitar<Denuncia>(
    `/api/denuncias/${id}/situacao`,
    {
      method: 'PATCH',
      body: JSON.stringify({ solucionado }),
    },
    token,
  );
}

export function excluirDenuncia(id: number, token: string) {
  return solicitar<void>(`/api/denuncias/${id}`, { method: 'DELETE' }, token);
}
