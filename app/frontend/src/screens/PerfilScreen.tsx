import { Button, Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import PreferenciasPerfil from '@/components/PreferenciasPerfil';
import EditorBiografia from '@/components/EditorBiografia';
import { useCallback, useRef, useState } from 'react';
import { router, useFocusEffect } from 'expo-router';
import { ActivityIndicator, FlatList, Modal, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import PostagemCard from '@/components/PostagemCard';
import { useAuth } from '@/contexts/AuthContext';
import { buscarMinhasPostagens, excluirPostagem } from '@/services/postagens';
import type { Postagem } from '@/types/postagem';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    nomePerfil: {
      fontSize: 23,
      fontWeight: '800',
      lineHeight: 30,
      textAlign: 'center',
    },

    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    conteudo: {
      padding: 16,
    },

    cabecalho: {
      marginBottom: 16,
      backgroundColor: colors.surface,
      borderRadius: 22,
      padding: 20,
      borderWidth: 1,
      borderColor: colors.line,
    },

    dados: {
      gap: 8,
      alignItems: 'center',
    },

    acoes: {
      marginTop: 24,
      gap: 14,
    },

    titulo: {
      marginTop: 24,
      marginBottom: 16,
      fontSize: 18,
      fontWeight: '800',
    },

    estado: {
      paddingVertical: 16,
    },

    postagem: {
      marginBottom: 24,
    },

    acaoPostagem: {
      marginTop: 12,
    },

    fundoModal: {
      flex: 1,
      justifyContent: 'center',
      padding: 24,
      backgroundColor: 'rgba(0, 0, 0, 0.5)',
    },

    modal: {
      backgroundColor: colors.surface,
      padding: 24,
      gap: 20,
      width: '100%',
      maxWidth: 440,
      alignSelf: 'center',
      borderRadius: 24,
    },

    botoesModal: {
      gap: 16,
      marginTop: 4,
    },

    avatar: {
      width: 76,
      height: 76,
      borderRadius: 38,
      backgroundColor: colors.softBlue,
      alignItems: 'center',
      justifyContent: 'center',
      marginBottom: 12,
    },

    inicial: {
      color: colors.primary,
      fontSize: 30,
      fontWeight: '800',
    },
  });

export default function PerfilScreen() {
  const styles = useEstilos(criarEstilos);
  const { reduzirMovimento } = usePreferencias();

  const { sessao, sair } = useAuth();
  const token = sessao?.token;

  const [postagens, setPostagens] = useState<Postagem[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const [selecionada, setSelecionada] = useState<Postagem | null>(null);
  const [excluindo, setExcluindo] = useState(false);
  const [erroExclusao, setErroExclusao] = useState<string | null>(null);

  const requisicao = useRef<AbortController | null>(null);
  const exclusaoEmAndamento = useRef(false);

  const carregarPostagens = useCallback(
    async (controller: AbortController) => {
      if (!token) {
        return;
      }

      setCarregando(true);
      setErro(null);

      try {
        const resultado = await buscarMinhasPostagens(token, controller.signal);

        if (!controller.signal.aborted) {
          setPostagens(resultado);
        }
      } catch (error) {
        if (!controller.signal.aborted) {
          const mensagem =
            error instanceof Error ? error.message : 'Não foi possível carregar suas postagens.';

          setErro(mensagem);
        }
      } finally {
        if (!controller.signal.aborted) {
          setCarregando(false);
        }
      }
    },
    [token],
  );

  useFocusEffect(
    useCallback(() => {
      const controller = new AbortController();

      requisicao.current = controller;
      void carregarPostagens(controller);

      return () => {
        controller.abort();
        requisicao.current?.abort();
      };
    }, [carregarPostagens]),
  );

  function atualizar() {
    if (exclusaoEmAndamento.current) {
      return;
    }

    requisicao.current?.abort();

    const controller = new AbortController();

    requisicao.current = controller;
    void carregarPostagens(controller);
  }

  function solicitarExclusao(postagem: Postagem) {
    setErroExclusao(null);
    setSelecionada(postagem);
  }

  function cancelarExclusao() {
    if (!exclusaoEmAndamento.current) {
      setSelecionada(null);
      setErroExclusao(null);
    }
  }

  async function confirmarExclusao() {
    if (!selecionada || !token || exclusaoEmAndamento.current) {
      return;
    }

    const id = selecionada.id;

    exclusaoEmAndamento.current = true;
    setExcluindo(true);
    setErroExclusao(null);

    try {
      await excluirPostagem(id, token);

      setPostagens((atuais) => atuais.filter((postagem) => postagem.id !== id));
      setSelecionada(null);
    } catch (error) {
      const mensagem =
        error instanceof Error ? error.message : 'Não foi possível excluir a postagem.';

      setErroExclusao(mensagem);
    } finally {
      exclusaoEmAndamento.current = false;
      setExcluindo(false);
    }
  }

  async function encerrarSessao() {
    setErro(null);

    try {
      await sair();
    } catch {
      setErro('Não foi possível encerrar a sessão.');
    }
  }

  return (
    <SafeAreaView edges={['left', 'right']} style={styles.tela}>
      <FlatList
        accessibilityElementsHidden={selecionada !== null}
        importantForAccessibility={selecionada ? 'no-hide-descendants' : 'auto'}
        data={postagens}
        keyExtractor={(postagem) => String(postagem.id)}
        contentContainerStyle={styles.conteudo}
        refreshing={carregando}
        onRefresh={atualizar}
        ListHeaderComponent={
          <View style={styles.cabecalho}>
            <View style={styles.dados}>
              <View style={styles.avatar}>
                <Text style={styles.inicial}>
                  {sessao?.usuario.nome.trim().charAt(0).toUpperCase()}
                </Text>
              </View>

              <Text
                style={styles.nomePerfil}
                accessibilityRole="header"
                accessibilityLabel={`Perfil de ${sessao?.usuario.nome}`}
              >
                {sessao?.usuario.nome}
              </Text>

              <Text accessibilityLabel={`E-mail: ${sessao?.usuario.email}`}>
                {sessao?.usuario.email}
              </Text>

              {!!sessao?.usuario.biografia && <Text>{sessao.usuario.biografia}</Text>}
            </View>

            <EditorBiografia />

            <View style={styles.acoes}>
              <Button
                title="Minhas denúncias"
                onPress={() => router.push({ pathname: '/denuncias', params: { minhas: 'true' } })}
              />

              <Button
                title="Sair da conta"
                disabled={excluindo}
                onPress={() => void encerrarSessao()}
              />
            </View>

            <PreferenciasPerfil />

            <Text accessibilityRole="header" style={styles.titulo}>
              Minhas postagens
            </Text>

            {erro && (
              <View style={styles.dados}>
                <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
                  {erro}
                </Text>

                <Button title="Tentar novamente" disabled={excluindo} onPress={atualizar} />
              </View>
            )}
          </View>
        }
        ListEmptyComponent={
          <View style={styles.estado}>
            {carregando && <ActivityIndicator accessibilityLabel="Carregando suas postagens" />}

            {!carregando && !erro && <Text>Você ainda não publicou nenhuma postagem.</Text>}
          </View>
        }
        renderItem={({ item }) => (
          <View style={styles.postagem}>
            <PostagemCard postagem={item} altura={320} />

            <View style={styles.acaoPostagem}>
              <Button
                title="Excluir postagem"
                accessibilityLabel={`Excluir postagem de ${new Date(item.criadaEm).toLocaleString('pt-BR')}`}
                disabled={carregando || excluindo}
                onPress={() => solicitarExclusao(item)}
              />
            </View>
          </View>
        )}
      />

      <Modal
        visible={selecionada !== null}
        transparent
        animationType={reduzirMovimento ? 'none' : 'fade'}
        onRequestClose={cancelarExclusao}
      >
        <View style={styles.fundoModal}>
          <View style={styles.modal} accessibilityViewIsModal importantForAccessibility="yes">
            <Text accessibilityRole="header">Excluir postagem?</Text>

            <Text>Esta ação não pode ser desfeita.</Text>

            {erroExclusao && (
              <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
                {erroExclusao}
              </Text>
            )}

            <View style={styles.botoesModal}>
              <Button
                title="Cancelar"
                accessibilityLabel="Cancelar exclusão e manter a postagem"
                disabled={excluindo}
                onPress={cancelarExclusao}
              />

              <Button
                title={excluindo ? 'Excluindo...' : 'Confirmar exclusão'}
                accessibilityLabel={
                  excluindo ? 'Excluindo postagem' : 'Confirmar exclusão definitiva da postagem'
                }
                disabled={excluindo}
                onPress={() => void confirmarExclusao()}
              />
            </View>
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}
