import { Button, Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { useCallback, useRef, useState } from 'react';
import { useFocusEffect } from 'expo-router';
import { ActivityIndicator, FlatList, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import TopicoCard from '@/components/TopicoCard';
import { buscarTopicos, demonstrarInteresse, jaDemonstrouInteresse } from '@/services/topicos';
import { useAuth } from '@/contexts/AuthContext';
import type { Topico } from '@/types/topico';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    conteudo: {
      padding: 18,
      paddingBottom: 28,
    },

    cabecalho: {
      marginBottom: 22,
    },

    aviso: {
      marginTop: 14,
      lineHeight: 17,
      fontSize: 11,
      color: colors.muted,
    },

    erro: {
      marginTop: 16,
      gap: 16,
    },

    vazio: {
      paddingVertical: 16,
    },

    eyebrow: {
      fontSize: 10,
      fontWeight: '800',
      letterSpacing: 1.6,
      color: colors.primary,
      marginTop: 12,
    },

    titulo: {
      fontSize: 28,
      lineHeight: 35,
      fontWeight: '800',
      letterSpacing: -0.8,
      marginTop: 8,
    },

    subtitulo: {
      color: colors.muted,
      marginTop: 8,
      fontSize: 13,
    },
  });

export default function TopicosScreen() {
  const styles = useEstilos(criarEstilos);

  const { sessao } = useAuth();

  const [topicos, setTopicos] = useState<Topico[]>([]);
  const [curtindo, setCurtindo] = useState<number[]>([]);
  const [errosInteresse, setErrosInteresse] = useState<Record<number, string>>({});
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  const requisicao = useRef<AbortController | null>(null);

  const carregar = useCallback(async (controller: AbortController) => {
    setCarregando(true);
    setErro(null);

    try {
      const resultado = await buscarTopicos(controller.signal);

      if (!controller.signal.aborted) {
        setTopicos((atuais) =>
          resultado.map((topico) => {
            const anterior = atuais.find((item) => item.id === topico.id);

            return {
              ...topico,
              interesses: Math.max(topico.interesses, anterior?.interesses ?? 0),
            };
          }),
        );
      }
    } catch (error) {
      if (!controller.signal.aborted) {
        const mensagem =
          error instanceof Error ? error.message : 'Não foi possível carregar os tópicos.';

        setErro(mensagem);
      }
    } finally {
      if (!controller.signal.aborted) {
        setCarregando(false);
      }
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      const controller = new AbortController();

      requisicao.current = controller;
      void carregar(controller);

      return () => requisicao.current?.abort();
    }, [carregar]),
  );

  function atualizar() {
    requisicao.current?.abort();

    const controller = new AbortController();

    requisicao.current = controller;
    void carregar(controller);
  }

  async function curtir(topicoId: number) {
    if (!sessao || jaDemonstrouInteresse(sessao.usuario.id, topicoId)) {
      return;
    }

    setCurtindo((atuais) => [...new Set([...atuais, topicoId])]);
    setErrosInteresse((atuais) => ({ ...atuais, [topicoId]: '' }));

    try {
      const resultado = await demonstrarInteresse(topicoId, sessao.usuario.id, sessao.token);

      setTopicos((atuais) =>
        atuais.map((topico) => {
          if (topico.id !== topicoId) {
            return topico;
          }

          return {
            ...topico,
            interesses: Math.max(topico.interesses, resultado.interesses),
          };
        }),
      );
    } catch (error) {
      const mensagem =
        error instanceof Error ? error.message : 'Não foi possível registrar seu interesse.';

      setErrosInteresse((atuais) => ({ ...atuais, [topicoId]: mensagem }));
    } finally {
      setCurtindo((atuais) => atuais.filter((id) => id !== topicoId));
    }
  }

  return (
    <SafeAreaView edges={['left', 'right']} style={styles.tela}>
      <FlatList
        data={topicos}
        keyExtractor={(topico) => String(topico.id)}
        renderItem={({ item, index }) => (
          <TopicoCard
            topico={item}
            posicao={index + 1}
            curtido={!!sessao && jaDemonstrouInteresse(sessao.usuario.id, item.id)}
            enviando={curtindo.includes(item.id)}
            erro={errosInteresse[item.id]}
            onInteressar={() => void curtir(item.id)}
          />
        )}
        contentContainerStyle={styles.conteudo}
        refreshing={carregando}
        onRefresh={atualizar}
        ListHeaderComponent={
          <View style={styles.cabecalho}>
            <Text style={styles.eyebrow}>SEU BAIRRO. SUAS IDEIAS.</Text>

            <Text accessibilityRole="header" style={styles.titulo}>
              O que está em pauta
            </Text>

            <Text style={styles.subtitulo}>Conversas que podem transformar o Parque Doroteia.</Text>

            <Text style={styles.aviso}>
              Os perfis institucionais, convites e propostas iniciais são fictícios para demonstrar
              o AJA.
            </Text>

            {erro && (
              <View style={styles.erro}>
                <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
                  {erro}
                </Text>

                <Button title="Tentar novamente" onPress={atualizar} />
              </View>
            )}
          </View>
        }
        ListEmptyComponent={
          <View style={styles.vazio}>
            {carregando && <ActivityIndicator accessibilityLabel="Carregando tópicos" />}

            {!carregando && !erro && <Text>Nenhum tópico publicado por enquanto.</Text>}
          </View>
        }
      />
    </SafeAreaView>
  );
}
