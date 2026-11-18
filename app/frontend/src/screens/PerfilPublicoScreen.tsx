import { useCallback, useMemo, useRef, useState } from 'react';
import { ActivityIndicator, FlatList, StyleSheet, View } from 'react-native';
import { useFocusEffect, useLocalSearchParams } from 'expo-router';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Ionicons } from '@expo/vector-icons';
import { Button, Text } from '@/components/ui';
import PostagemCard from '@/components/PostagemCard';
import TopicoCard from '@/components/TopicoCard';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useEstilos } from '@/hooks/useEstilos';
import { buscarUsuario, buscarPostagensUsuario, buscarTopicosUsuario } from '@/services/usuarios';
import { ApiError } from '@/services/http';
import type { ThemeColors } from '@/constants/theme';
import type { Autor, PaginaPostagens, Postagem } from '@/types/postagem';
import type { Topico } from '@/types/topico';

type DadosPerfil = {
  usuario: Autor;
  pagina: PaginaPostagens;
  topicos: Topico[];
};

type Publicacao = { tipo: 'topico'; item: Topico } | { tipo: 'postagem'; item: Postagem };

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    lista: {
      padding: 18,
      paddingBottom: 36,
    },

    cabecalho: {
      backgroundColor: colors.surface,
      borderWidth: 1,
      borderColor: colors.line,
      borderRadius: 22,
      padding: 22,
      gap: 16,
      marginBottom: 24,
    },

    avatar: {
      width: 80,
      height: 80,
      borderRadius: 40,
      backgroundColor: colors.softBlue,
      alignItems: 'center',
      justifyContent: 'center',
    },

    nome: {
      fontSize: 25,
      lineHeight: 33,
      fontWeight: '800',
    },

    subtitulo: {
      fontSize: 12,
      color: colors.muted,
    },

    titulo: {
      fontSize: 19,
      fontWeight: '800',
      marginBottom: 16,
    },

    estado: {
      paddingVertical: 20,
      gap: 16,
    },
  });

export default function PerfilPublicoScreen() {
  const styles = useEstilos(criarEstilos);
  const { id: parametro } = useLocalSearchParams<{ id: string }>();
  const id = typeof parametro === 'string' ? Number(parametro) : NaN;

  if (!Number.isSafeInteger(id) || id <= 0) {
    return (
      <View style={[styles.tela, styles.lista]}>
        <Text accessibilityRole="alert">Perfil inválido.</Text>
      </View>
    );
  }

  return <PerfilUsuario key={id} id={id} />;
}

function PerfilUsuario({ id }: { id: number }) {
  const styles = useEstilos(criarEstilos);
  const { colors } = usePreferencias();
  const [dados, setDados] = useState<DadosPerfil | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [carregandoMais, setCarregandoMais] = useState(false);
  const [erro, setErro] = useState<string | null>(null);
  const requisicao = useRef<AbortController | null>(null);
  const ocupado = useRef(false);
  const publicacoes = useMemo<Publicacao[]>(
    () =>
      dados
        ? [
            ...dados.topicos.map((item) => ({ tipo: 'topico' as const, item })),
            ...dados.pagina.itens.map((item) => ({ tipo: 'postagem' as const, item })),
          ]
        : [],
    [dados],
  );

  const carregar = useCallback(
    async (controller: AbortController) => {
      setCarregando(true);
      setCarregandoMais(false);
      setErro(null);
      setDados(null);
      ocupado.current = true;

      try {
        const [usuario, pagina, topicos] = await Promise.all([
          buscarUsuario(id, controller.signal),
          buscarPostagensUsuario(id, null, controller.signal),
          buscarTopicosUsuario(id, controller.signal),
        ]);

        if (!controller.signal.aborted) setDados({ usuario, pagina, topicos });
      } catch (error) {
        if (!controller.signal.aborted) {
          setErro(
            error instanceof ApiError && error.status === 404
              ? 'Perfil não encontrado.'
              : error instanceof Error
                ? error.message
                : 'Não foi possível carregar este perfil.',
          );
        }
      } finally {
        if (!controller.signal.aborted) {
          ocupado.current = false;
          setCarregando(false);
        }
      }
    },
    [id],
  );

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

  async function carregarMais() {
    if (!dados?.pagina.temMais || dados.pagina.proximoCursor === null || ocupado.current) return;

    const controller = new AbortController();

    requisicao.current = controller;
    ocupado.current = true;
    setCarregandoMais(true);
    setErro(null);

    try {
      const pagina = await buscarPostagensUsuario(
        id,
        dados.pagina.proximoCursor,
        controller.signal,
      );

      if (!controller.signal.aborted) {
        setDados((atual) =>
          atual
            ? {
                ...atual,
                pagina: {
                  ...pagina,
                  itens: [
                    ...atual.pagina.itens,
                    ...pagina.itens.filter(
                      (item) => !atual.pagina.itens.some((existente) => existente.id === item.id),
                    ),
                  ],
                },
              }
            : atual,
        );
      }
    } catch (error) {
      if (!controller.signal.aborted)
        setErro(
          error instanceof Error ? error.message : 'Não foi possível carregar mais postagens.',
        );
    } finally {
      if (!controller.signal.aborted) {
        ocupado.current = false;
        setCarregandoMais(false);
      }
    }
  }

  return (
    <SafeAreaView edges={['left', 'right', 'bottom']} style={styles.tela}>
      <FlatList
        data={publicacoes}
        keyExtractor={(publicacao) => `${publicacao.tipo}-${publicacao.item.id}`}
        contentContainerStyle={styles.lista}
        refreshing={carregando}
        onRefresh={atualizar}
        ListHeaderComponent={
          dados ? (
            <>
              <View style={styles.cabecalho}>
                <View
                  style={styles.avatar}
                  accessibilityElementsHidden
                  importantForAccessibility="no-hide-descendants"
                >
                  <Ionicons name="person" size={42} color={colors.primary} />
                </View>

                <Text style={styles.nome} accessibilityRole="header">
                  {dados.usuario.nome}
                </Text>

                <Text style={styles.subtitulo}>PERFIL DA COMUNIDADE</Text>

                <Text>
                  {dados.usuario.biografia || 'Esta pessoa ainda não adicionou uma biografia.'}
                </Text>
              </View>

              <Text style={styles.titulo} accessibilityRole="header">
                Tópicos e postagens
              </Text>
            </>
          ) : undefined
        }
        renderItem={({ item, index }) =>
          item.tipo === 'topico' ? (
            <TopicoCard topico={item.item} posicao={index + 1} />
          ) : (
            <PostagemCard postagem={item.item} />
          )
        }
        ListEmptyComponent={
          <View style={styles.estado}>
            {carregando && (
              <ActivityIndicator color={colors.primary} accessibilityLabel="Carregando perfil" />
            )}

            {!carregando && !erro && dados && (
              <Text>Esta pessoa ainda não publicou tópicos ou postagens.</Text>
            )}
          </View>
        }
        ListFooterComponent={
          <View style={styles.estado}>
            {erro && (
              <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
                {erro}
              </Text>
            )}

            {erro && !dados && <Button title="Tentar novamente" onPress={atualizar} />}

            {carregandoMais && (
              <ActivityIndicator
                color={colors.primary}
                accessibilityLabel="Carregando mais postagens"
              />
            )}

            {dados?.pagina.temMais && !carregando && !carregandoMais && (
              <Button
                title={erro ? 'Tentar carregar novamente' : 'Carregar mais postagens'}
                onPress={() => void carregarMais()}
              />
            )}
          </View>
        }
      />
    </SafeAreaView>
  );
}
