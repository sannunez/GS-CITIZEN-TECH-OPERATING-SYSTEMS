import { useCallback, useEffect, useRef, useState } from 'react';
import { buscarPostagens } from '@/services/postagens';
import type { Postagem } from '@/types/postagem';

export function useFeed() {
  const [postagens, setPostagens] = useState<Postagem[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [atualizando, setAtualizando] = useState(false);
  const [temMais, setTemMais] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  const cursor = useRef<number | null>(null);
  const ocupado = useRef(false);
  const terminou = useRef(false);
  const falhou = useRef(false);
  const requisicao = useRef<AbortController | null>(null);

  const executar = useCallback(async (reiniciar: boolean, controller: AbortController) => {
    try {
      const pagina = await buscarPostagens(cursor.current, controller.signal);

      if (controller.signal.aborted) {
        return;
      }

      setPostagens(atuais => {
        if (reiniciar) {
          return pagina.itens;
        }

        const ids = new Set(atuais.map(postagem => postagem.id));

        return [...atuais, ...pagina.itens.filter(postagem => !ids.has(postagem.id))];
      });
      cursor.current = pagina.proximoCursor;
      terminou.current = !pagina.temMais;
      setTemMais(pagina.temMais);
    } catch {
      if (!controller.signal.aborted) {
        falhou.current = true;
        setErro('Não foi possível carregar as postagens. Verifique sua conexão e tente novamente.');
      }
    } finally {
      if (requisicao.current === controller && !controller.signal.aborted) {
        ocupado.current = false;
        setCarregando(false);
        setAtualizando(false);
      }
    }
  }, []);

  const carregar = useCallback(
    async (reiniciar = false, tentarNovamente = false) => {
      if (!reiniciar && (ocupado.current || terminou.current || (falhou.current && !tentarNovamente))) {
        return;
      }

      if (reiniciar) {
        requisicao.current?.abort();
        cursor.current = null;
        terminou.current = false;
        setTemMais(true);
      }

      const controller = new AbortController();

      requisicao.current = controller;
      ocupado.current = true;
      falhou.current = false;
      setCarregando(true);
      setAtualizando(reiniciar);
      setErro(null);
      await executar(reiniciar, controller);
    },
    [executar],
  );

  const atualizar = useCallback(() => void carregar(true), [carregar]);

  const carregarMais = useCallback(() => void carregar(), [carregar]);

  useEffect(() => {
    const controller = new AbortController();

    requisicao.current = controller;
    ocupado.current = true;
    void executar(true, controller);

    return () => requisicao.current?.abort();
  }, [executar]);

  return {
    postagens,
    carregando,
    atualizando,
    temMais,
    erro,
    carregarMais,
    atualizar,
    tentarNovamente: () => void carregar(postagens.length === 0, true),
  };
}
