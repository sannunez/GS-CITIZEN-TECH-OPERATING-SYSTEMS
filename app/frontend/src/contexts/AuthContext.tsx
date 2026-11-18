import AsyncStorage from '@react-native-async-storage/async-storage';
import { createContext, useCallback, useContext, useEffect, useState, type PropsWithChildren } from 'react';
import { autenticar, validarSessao } from '@/services/autenticacao';
import { ApiError } from '@/services/http';
import type { Sessao } from '@/types/sessao';
import { atualizarBiografia } from '@/services/usuarios';

type DadosAcesso = {
  nome?: string;
  email: string;
  senha: string;
};

type Auth = {
  sessao: Sessao | null;
  carregando: boolean;
  entrando: boolean;
  concluirEntrada: () => void;
  erroRestauracao: boolean;
  tentarRestaurar: () => void;
  entrar: (modo: 'login' | 'cadastro', dados: DadosAcesso) => Promise<void>;
  sair: () => Promise<void>;
  salvarBiografia: (biografia: string) => Promise<void>;
};

const CHAVE = '@aja/sessao';

const Context = createContext<Auth | null>(null);

export function AuthProvider({ children }: PropsWithChildren) {
  const [sessao, setSessao] = useState<Sessao | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [erroRestauracao, setErroRestauracao] = useState(false);
  const [tentativa, setTentativa] = useState(0);
  const [entrando, setEntrando] = useState(false);
  const concluirEntrada = useCallback(() => setEntrando(false), []);

  useEffect(() => {
    let ativo = true;

    async function restaurar() {
      try {
        const raw = await AsyncStorage.getItem(CHAVE);

        if (!raw) {
          return;
        }

        let salva: Sessao;

        try {
          salva = JSON.parse(raw);
        } catch {
          await AsyncStorage.removeItem(CHAVE);

          return;
        }

        if (
          !salva?.token ||
          !salva.usuario?.id ||
          !Number.isFinite(salva.expiraEm) ||
          salva.expiraEm <= Date.now()
        ) {
          await AsyncStorage.removeItem(CHAVE);

          return;
        }

        try {
          const usuario = await validarSessao(salva.token);

          if (ativo) {
            setSessao({ ...salva, usuario });
          }
        } catch (error) {
          if (error instanceof ApiError && error.status === 401) {
            await AsyncStorage.removeItem(CHAVE);
          } else {
            throw error;
          }
        }
      } catch {
        if (ativo) {
          setErroRestauracao(true);
        }
      } finally {
        if (ativo) {
          setCarregando(false);
        }
      }
    }

    void restaurar();

    return () => {
      ativo = false;
    };
  }, [tentativa]);

  async function entrar(modo: 'login' | 'cadastro', dados: DadosAcesso) {
    const nova = await autenticar(modo, dados);

    await AsyncStorage.setItem(CHAVE, JSON.stringify(nova));

    setEntrando(true);
    setSessao(nova);
    setErroRestauracao(false);
  }

  async function sair() {
    await AsyncStorage.removeItem(CHAVE);

    setSessao(null);
    setEntrando(false);
    setErroRestauracao(false);
  }

  async function salvarBiografia(biografia: string) {
    if (!sessao) throw new Error('Entre novamente para atualizar sua biografia.');

    const resultado = await atualizarBiografia(sessao.usuario.id, biografia, sessao.token);

    setSessao(atual => atual?.token === sessao.token ? {
      ...atual,
      usuario: { ...atual.usuario, biografia: resultado.biografia },
    } : atual);
  }

  function tentarRestaurar() {
    setCarregando(true);
    setErroRestauracao(false);
    setTentativa(valor => valor + 1);
  }

  return (
    <Context.Provider
      value={{
        sessao,
        carregando,
        entrando,
        concluirEntrada,
        erroRestauracao,
        entrar,
        sair,
        salvarBiografia,
        tentarRestaurar,
      }}
    >
      {children}
    </Context.Provider>
  );
}

export function useAuth() {
  const auth = useContext(Context);

  if (!auth) {
    throw new Error('AuthProvider não encontrado.');
  }

  return auth;
}
