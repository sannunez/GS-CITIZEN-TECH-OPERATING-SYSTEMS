import { ApiError, solicitar } from '@/services/http';
import type { Sessao, UsuarioSessao } from '@/types/sessao';

type DadosAcesso = {
  nome?: string;
  email: string;
  senha: string;
};

type RespostaAcesso = {
  accessToken: string;
  expiresIn: number;
};

export function validarSessao(token: string) {
  return solicitar<UsuarioSessao>('/api/auth/me', {}, token);
}

export async function autenticar(modo: 'login' | 'cadastro', dados: DadosAcesso): Promise<Sessao> {
  const resposta = await solicitar<RespostaAcesso>(`/api/auth/${modo}`, {
    method: 'POST',
    body: JSON.stringify(dados),
  }).catch(error => {
    if (modo === 'login' && error instanceof ApiError && error.status === 401) {
      throw new ApiError('E-mail ou senha incorretos. Verifique os dados e tente novamente.', 401);
    }

    throw error;
  });

  const usuario = await validarSessao(resposta.accessToken);

  return {
    token: resposta.accessToken,
    expiraEm: Date.now() + resposta.expiresIn * 1000,
    usuario,
  };
}
