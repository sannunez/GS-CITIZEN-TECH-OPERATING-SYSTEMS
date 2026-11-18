import { Button, Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useCallback, useEffect, useRef, useState } from 'react';
import { router, useFocusEffect } from 'expo-router';
import { ActivityIndicator, FlatList, Modal, Pressable, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import PostagemCard from '@/components/PostagemCard';
import { useFeed } from '@/hooks/useFeed';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    lista: {
      paddingHorizontal: 16,
      paddingBottom: 80,
    },

    estado: {
      paddingVertical: 24,
    },

    rodape: {
      paddingVertical: 12,
    },

    acaoRodape: {
      marginTop: 16,
    },

    botaoPublicar: {
      position: 'absolute',
      right: 16,
      bottom: 16,
      width: 56,
      height: 56,
      borderRadius: 28,
      backgroundColor: colors.primary,
      borderWidth: 0,
      alignItems: 'center',
      justifyContent: 'center',
      elevation: 5,
      boxShadow: '0 6px 20px #3F7CEE40',
    },

    iconePublicar: {
      fontSize: 32,
      color: colors.onPrimary,
    },

    fundoGaveta: {
      flex: 1,
      backgroundColor: 'rgba(0,0,0,0.45)',
      justifyContent: 'flex-end',
    },

    foraGaveta: {
      flex: 1,
    },

    gaveta: {
      backgroundColor: colors.surface,
      padding: 24,
      gap: 20,
      borderTopLeftRadius: 28,
      borderTopRightRadius: 28,
    },

    tituloGaveta: {
      fontSize: 22,
      fontWeight: '600',
    },

    acessoDenuncias: {
      paddingHorizontal: 16,
      paddingVertical: 20,
    },

    cabecalhoFeed: {
      flexDirection: 'row',
      flexWrap: 'wrap',
      alignItems: 'center',
      justifyContent: 'space-between',
      gap: 12,
    },

    eyebrow: {
      fontSize: 8,
      fontWeight: '700',
      letterSpacing: 1,
      color: colors.muted,
    },

    tituloFeed: {
      fontSize: 25,
      lineHeight: 32,
      fontWeight: '800',
      letterSpacing: -0.7,
    },

    chipDenuncias: {
      paddingHorizontal: 12,
      paddingVertical: 10,
      borderRadius: 20,
      backgroundColor: colors.softBlue,
    },

    textoChip: {
      color: colors.primary,
      fontSize: 12,
      fontWeight: '700',
    },
  });

export default function FeedScreen() {
  const styles = useEstilos(criarEstilos);
  const { reduzirMovimento } = usePreferencias();

  const [altura, setAltura] = useState(0);
  const [alturaConteudo, setAlturaConteudo] = useState(0);
  const [gavetaAberta, setGavetaAberta] = useState(false);

  const feed = useFeed();

  const primeiraVez = useRef(true);

  const { atualizar, carregarMais, carregando, erro, temMais } = feed;

  useEffect(() => {
    if (
      altura > 0 &&
      alturaConteudo > 0 &&
      alturaConteudo <= altura * 1.35 &&
      feed.postagens.length > 0 &&
      temMais &&
      !carregando &&
      !erro
    ) {
      carregarMais();
    }
  }, [altura, alturaConteudo, feed.postagens.length, temMais, carregando, erro, carregarMais]);

  useFocusEffect(
    useCallback(() => {
      if (primeiraVez.current) {
        primeiraVez.current = false;

        return;
      }

      atualizar();
    }, [atualizar]),
  );

  function renderizarEstadoVazio() {
    if (feed.carregando) {
      return (
        <View style={styles.estado}>
          <ActivityIndicator accessibilityLabel="Carregando postagens" />
        </View>
      );
    }

    if (feed.erro) {
      return null;
    }

    return (
      <View style={styles.estado}>
        <Text>Nenhuma postagem encontrada.</Text>
      </View>
    );
  }

  function renderizarRodape() {
    if (feed.erro) {
      return (
        <View style={styles.rodape}>
          <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
            {feed.erro}
          </Text>

          <View style={styles.acaoRodape}>
            <Button title="Tentar novamente" onPress={feed.tentarNovamente} />
          </View>
        </View>
      );
    }

    if (feed.carregando && !feed.atualizando && feed.postagens.length > 0) {
      return (
        <View style={styles.rodape}>
          <ActivityIndicator accessibilityLabel="Carregando mais postagens" />
        </View>
      );
    }

    if (!feed.temMais && feed.postagens.length > 0) {
      return (
        <View style={styles.rodape}>
          <Text>Todas as postagens foram carregadas.</Text>
        </View>
      );
    }

    if (feed.temMais && !feed.carregando && feed.postagens.length > 0) {
      return (
        <View style={styles.rodape}>
          <Button title="Carregar mais postagens" onPress={carregarMais} />
        </View>
      );
    }

    return null;
  }

  return (
    <SafeAreaView edges={['left', 'right']} style={styles.tela}>
      <View
        style={styles.acessoDenuncias}
        accessibilityElementsHidden={gavetaAberta}
        importantForAccessibility={gavetaAberta ? 'no-hide-descendants' : 'auto'}
      >
        <View style={styles.cabecalhoFeed}>
          <View>
            <Text style={styles.eyebrow}>HISTÓRIAS DO NOSSO BAIRRO</Text>

            <Text style={styles.tituloFeed}>Comunidade</Text>
          </View>

          <Pressable
            accessibilityRole="button"
            accessibilityLabel="Ver denúncias da comunidade"
            onPress={() => router.push('/denuncias')}
            style={styles.chipDenuncias}
          >
            <Text style={styles.textoChip}>Denúncias ↗</Text>
          </Pressable>
        </View>
      </View>

      <View
        style={styles.tela}
        accessibilityElementsHidden={gavetaAberta}
        importantForAccessibility={gavetaAberta ? 'no-hide-descendants' : 'auto'}
        onLayout={(event) => setAltura(event.nativeEvent.layout.height)}
      >
        <FlatList
          data={feed.postagens}
          keyExtractor={(postagem) => String(postagem.id)}
          renderItem={({ item }) => <PostagemCard postagem={item} />}
          contentContainerStyle={styles.lista}
          onContentSizeChange={(_, alturaLista) => setAlturaConteudo(alturaLista)}
          initialNumToRender={3}
          maxToRenderPerBatch={3}
          onEndReached={feed.carregarMais}
          onEndReachedThreshold={0.2}
          onScroll={({ nativeEvent }) => {
            const { contentOffset, contentSize, layoutMeasurement } = nativeEvent;
            const distanciaDoFim = contentSize.height - contentOffset.y - layoutMeasurement.height;

            if (distanciaDoFim <= layoutMeasurement.height * 0.35) {
              carregarMais();
            }
          }}
          scrollEventThrottle={100}
          refreshing={feed.atualizando}
          onRefresh={feed.atualizar}
          ListEmptyComponent={renderizarEstadoVazio() ?? undefined}
          ListFooterComponent={renderizarRodape() ?? undefined}
        />

        <Pressable
          accessibilityRole="button"
          accessibilityLabel="Criar postagem ou denúncia"
          accessibilityHint="Abre as opções de publicação."
          accessibilityState={{ expanded: gavetaAberta }}
          onPress={() => setGavetaAberta(true)}
          style={styles.botaoPublicar}
        >
          <Text style={styles.iconePublicar}>+</Text>
        </Pressable>
      </View>

      <Modal
        visible={gavetaAberta}
        transparent
        animationType={reduzirMovimento ? 'none' : 'slide'}
        onRequestClose={() => setGavetaAberta(false)}
      >
        <View style={styles.fundoGaveta}>
          <Pressable
            style={styles.foraGaveta}
            accessibilityRole="button"
            accessibilityLabel="Fechar opções de publicação"
            onPress={() => setGavetaAberta(false)}
          />

          <SafeAreaView edges={['bottom']} style={styles.gaveta} accessibilityViewIsModal>
            <Text accessibilityRole="header" style={styles.tituloGaveta}>
              O que deseja publicar?
            </Text>

            <Button
              title="Postagem"
              onPress={() => {
                setGavetaAberta(false);
                router.push('/nova-postagem');
              }}
            />

            <Button
              title="Denúncia"
              onPress={() => {
                setGavetaAberta(false);
                router.push('/denuncia-form');
              }}
            />

            <Button title="Cancelar" onPress={() => setGavetaAberta(false)} />
          </SafeAreaView>
        </View>
      </Modal>
    </SafeAreaView>
  );
}
