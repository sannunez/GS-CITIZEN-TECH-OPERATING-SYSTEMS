import { API_URL } from '@/config/api';

export class ApiError extends Error {
  constructor(
    message: string,
    public status: number,
  ) {
    super(message);
  }
}

export async function solicitar<T>(path: string, init: RequestInit = {}, token?: string): Promise<T> {
  const headers = new Headers(init.headers);

  if (init.body) {
    headers.set('Content-Type', 'application/json');
  }

  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  let response: Response;

  try {
    response = await fetch(`${API_URL}${path}`, { ...init, headers });
  } catch (error) {
    if (init.signal?.aborted) {
      throw error;
    }

    throw new Error(
      `Não foi possível conectar à API em ${API_URL}. ` +
        'Verifique se o backend está em execução e se o endereço está correto para este dispositivo.',
    );
  }

  if (!response.ok) {
    const data = await response.json().catch(() => null);
    const detalhes = data?.errors ? Object.values(data.errors).flat().join(' ') : null;

    throw new ApiError(
      data?.mensagem || detalhes || data?.detail || 'Não foi possível concluir a solicitação.',
      response.status,
    );
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
}
