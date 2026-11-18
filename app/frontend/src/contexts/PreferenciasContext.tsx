import AsyncStorage from '@react-native-async-storage/async-storage';
import { createContext, useCallback, useContext, useEffect, useMemo, useRef, useState, type PropsWithChildren } from 'react';
import { AccessibilityInfo } from 'react-native';
import { obterCores, type ThemeColors } from '@/constants/theme';

type Preferencias = {
  modoEscuro: boolean;
  altoContraste: boolean;
  textoAmpliado: boolean;
  textoNegrito: boolean;
};

type PreferenciasContexto = {
  preferencias: Preferencias;
  colors: ThemeColors;
  carregando: boolean;
  erro: string | null;
  reduzirMovimento: boolean;
  atualizar: (chave: keyof Preferencias, valor: boolean) => void;
  tentarSalvar: () => void;
};

const CHAVE = '@aja/preferencias';
const padrao: Preferencias = {
  modoEscuro: false,
  altoContraste: false,
  textoAmpliado: false,
  textoNegrito: false,
};
const Context = createContext<PreferenciasContexto | null>(null);

export function PreferenciasProvider({ children }: PropsWithChildren) {
  const [preferencias, setPreferencias] = useState(padrao);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const [reduzirMovimento, setMovimentoSistema] = useState(false);
  const [tentativa, setTentativa] = useState(0);
  const [alteradas, setAlteradas] = useState(false);
  const fila = useRef<Promise<void>>(Promise.resolve());
  const versao = useRef(0);
  const colors = obterCores(preferencias.modoEscuro, preferencias.altoContraste);

  useEffect(() => {
    let ativo = true;

    async function restaurar() {
      try {
        const raw = await AsyncStorage.getItem(CHAVE);
        const salva = raw ? JSON.parse(raw) : null;
        const validada = { ...padrao };

        for (const chave of Object.keys(padrao) as (keyof Preferencias)[]) {
          if (typeof salva?.[chave] === 'boolean') validada[chave] = salva[chave];
        }

        if (ativo) setPreferencias(validada);
      } catch {
        if (ativo)
          setErro('Não foi possível recuperar suas preferências. Você pode ajustá-las novamente.');
      } finally {
        if (ativo) setCarregando(false);
      }
    }

    void restaurar();

    return () => {
      ativo = false;
    };
  }, []);

  useEffect(() => {
    let ativo = true;
    let recebeuEvento = false;
    const inscricao = AccessibilityInfo.addEventListener('reduceMotionChanged', (valor) => {
      recebeuEvento = true;
      setMovimentoSistema(valor);
    });

    void AccessibilityInfo.isReduceMotionEnabled()
      .then((valor) => {
        if (ativo && !recebeuEvento) setMovimentoSistema(valor);
      })
      .catch(() => {});

    return () => {
      ativo = false;
      inscricao.remove();
    };
  }, []);

  useEffect(() => {
    if (carregando || !alteradas) return;

    const atual = ++versao.current;
    const dados = JSON.stringify(preferencias);

    fila.current = fila.current.then(async () => {
      try {
        await AsyncStorage.setItem(CHAVE, dados);
        if (versao.current === atual) setErro(null);
      } catch {
        if (versao.current === atual) {
          setErro('As preferências foram aplicadas, mas não puderam ser salvas neste aparelho.');
        }
      }
    });
  }, [preferencias, carregando, tentativa, alteradas]);

  const atualizar = useCallback((chave: keyof Preferencias, valor: boolean) => {
    setAlteradas(true);
    setPreferencias((atuais) => ({ ...atuais, [chave]: valor }));
  }, []);
  const tentarSalvar = useCallback(() => {
    setAlteradas(true);
    setTentativa((valor) => valor + 1);
  }, []);
  const value = useMemo(
    () => ({ preferencias, colors, carregando, erro, reduzirMovimento, atualizar, tentarSalvar }),
    [preferencias, colors, carregando, erro, reduzirMovimento, atualizar, tentarSalvar],
  );

  return <Context.Provider value={value}>{children}</Context.Provider>;
}

export function usePreferencias() {
  const contexto = useContext(Context);

  if (!contexto) throw new Error('PreferenciasProvider não encontrado.');

  return contexto;
}
