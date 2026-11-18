import type { EnderecoDenuncia } from '@/types/denuncia';

export async function buscarEnderecoCep(valor: string, signal?: AbortSignal): Promise<EnderecoDenuncia> {
  const cep = valor.replace(/\D/g, '');

  if (!/^\d{8}$/.test(cep)) throw new Error('Informe um CEP com oito dígitos.');

  const controller = new AbortController();
  const cancelar = () => controller.abort();

  if (signal?.aborted) controller.abort();

  signal?.addEventListener('abort', cancelar);

  const timeout = setTimeout(cancelar, 10000);

  try {
    const resposta = await fetch(`https://viacep.com.br/ws/${cep}/json/`, { signal: controller.signal });

    if (!resposta.ok) throw new Error('O ViaCEP está indisponível. Tente buscar novamente.');

    const dados = await resposta.json();

    if (dados.erro) throw new Error('CEP não encontrado. Confira os números e tente novamente.');

    if (
      typeof dados.localidade !== 'string' ||
      !dados.localidade.trim() ||
      typeof dados.uf !== 'string' ||
      !/^[A-Z]{2}$/.test(dados.uf)
    ) {
      throw new Error('O ViaCEP retornou um endereço incompleto. Tente novamente.');
    }

    return {
      cep,
      cidade: dados.localidade,
      uf: dados.uf,
      logradouro: typeof dados.logradouro === 'string' ? dados.logradouro : '',
      complemento: typeof dados.complemento === 'string' ? dados.complemento : '',
      bairro: typeof dados.bairro === 'string' ? dados.bairro : '',
    };
  } catch (error) {
    if (controller.signal.aborted && !signal?.aborted) {
      throw new Error('A consulta ao ViaCEP demorou demais. Tente novamente.');
    }

    if (error instanceof TypeError)
      throw new Error('Não foi possível consultar o ViaCEP. Confira sua conexão e tente novamente.');

    throw error;
  } finally {
    clearTimeout(timeout);
    signal?.removeEventListener('abort', cancelar);
  }
}
