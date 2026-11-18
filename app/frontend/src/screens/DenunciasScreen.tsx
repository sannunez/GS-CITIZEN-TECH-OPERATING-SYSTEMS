import { Button, Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useCallback, useRef, useState } from 'react';
import { router, useFocusEffect, useLocalSearchParams } from 'expo-router';
import { ActivityIndicator, FlatList, Modal, Pressable, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useAuth } from '@/contexts/AuthContext';
import { alterarSituacaoDenuncia, buscarDenuncias, excluirDenuncia } from '@/services/denuncias';
import type { Denuncia } from '@/types/denuncia';
import CarrosselDenuncia from '@/components/CarrosselDenuncia';

type Acao = { denuncia: Denuncia; tipo: 'excluir' | 'situacao' };

const situacoes = [
  { valor: 'todas', titulo: 'Todas as situações' },
  { valor: 'solucionadas', titulo: 'Solucionadas' },
  { valor: 'pendentes', titulo: 'Não solucionadas' },
] as const;

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    lista: {
      padding: 16,
    },

    cabecalho: {
      gap: 14,
      marginBottom: 20,
    },

    titulo: {
      fontSize: 26,
      fontWeight: '800',
      lineHeight: 34,
      letterSpacing: -0.6,
    },

    botoes: {
      flexDirection: 'row',
      flexWrap: 'wrap',
      gap: 16,
    },

    card: {
      backgroundColor: colors.surface,
      borderColor: colors.line,
      borderWidth: 1,
      borderRadius: 20,
      padding: 20,
      marginBottom: 20,
      gap: 8,
    },

    filtro: {
      minHeight: 48,
      maxWidth: '100%',
      paddingVertical: 12,
      paddingHorizontal: 16,
      borderRadius: 14,
      borderWidth: 1,
      borderColor: colors.line,
      backgroundColor: colors.surface,
      justifyContent: 'center',
    },

    filtroAtivo: {
      borderColor: colors.primary,
      backgroundColor: colors.softBlue,
    },

    textoFiltro: {
      color: colors.ink,
      fontWeight: '600',
    },

    status: {
      fontWeight: '700',
      color: colors.green,
      backgroundColor: colors.softGreen,
      padding: 10,
      borderRadius: 10,
      fontSize: 12,
    },

    relato: {
      lineHeight: 23,
    },

    acoes: {
      gap: 12,
      marginTop: 12,
    },

    fundo: {
      flex: 1,
      backgroundColor: 'rgba(0,0,0,0.5)',
      justifyContent: 'center',
      padding: 24,
    },

    modal: {
      backgroundColor: colors.surface,
      padding: 24,
      gap: 20,
      borderRadius: 24,
      maxWidth: 500,
      width: '100%',
      alignSelf: 'center',
    },
  });

export default function DenunciasScreen() {
  const styles = useEstilos(criarEstilos);
  const { reduzirMovimento } = usePreferencias();

  const { minhas: filtroInicial } = useLocalSearchParams<{ minhas?: string }>();
  const { sessao } = useAuth();
  const token = sessao?.token;

  const [minhas, setMinhas] = useState(filtroInicial === 'true');
  const [situacao, setSituacao] = useState<(typeof situacoes)[number]['valor']>('todas');
  const [itens, setItens] = useState<Denuncia[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);
  const [acao, setAcao] = useState<Acao | null>(null);
  const [erroAcao, setErroAcao] = useState<string | null>(null);
  const [salvando, setSalvando] = useState(false);

  const requisicao = useRef<AbortController | null>(null);
  const ocupado = useRef(false);
  const itensVisiveis = itens.filter(item => situacao === 'todas' || item.solucionado === (situacao === 'solucionadas'));

  const carregar = useCallback(
    async (controller: AbortController) => {
      if (!token) return;

      setCarregando(true);
      setErro(null);

      try {
        const resultado = await buscarDenuncias(token, minhas, controller.signal);

        if (!controller.signal.aborted) setItens(resultado);
      } catch (error) {
        if (!controller.signal.aborted)
          setErro(
            error instanceof Error ? error.message : 'Não foi possível carregar as denúncias.',
          );
      } finally {
        if (!controller.signal.aborted) setCarregando(false);
      }
    },
    [token, minhas],
  );

  useFocusEffect(
    useCallback(() => {
      const controller = new AbortController();

      requisicao.current = controller;
      setItens([]);
      void carregar(controller);

      return () => requisicao.current?.abort();
    }, [carregar]),
  );

  function atualizar() {
    if (ocupado.current) return;

    requisicao.current?.abort();

    const controller = new AbortController();

    requisicao.current = controller;
    void carregar(controller);
  }

  async function confirmar() {
    if (!acao || !token || ocupado.current) return;

    ocupado.current = true;
    setSalvando(true);
    setErroAcao(null);

    try {
      if (acao.tipo === 'excluir') {
        await excluirDenuncia(acao.denuncia.id, token);
        setItens((atuais) => atuais.filter((d) => d.id !== acao.denuncia.id));
      } else {
        const resultado = await alterarSituacaoDenuncia(
          acao.denuncia.id,
          !acao.denuncia.solucionado,
          token,
        );

        setItens((atuais) => atuais.map((d) => (d.id === resultado.id ? resultado : d)));
      }

      setAcao(null);
    } catch (error) {
      setErroAcao(error instanceof Error ? error.message : 'Não foi possível concluir a ação.');
    } finally {
      ocupado.current = false;
      setSalvando(false);
    }
  }

  function solicitar(denuncia: Denuncia, tipo: Acao['tipo']) {
    setErroAcao(null);
    setAcao({ denuncia, tipo });
  }

  return (
    <SafeAreaView edges={['left', 'right', 'bottom']} style={styles.tela}>
      <FlatList
        data={itensVisiveis}
        keyExtractor={(d) => String(d.id)}
        contentContainerStyle={styles.lista}
        accessibilityElementsHidden={!!acao}
        importantForAccessibility={acao ? 'no-hide-descendants' : 'auto'}
        refreshing={carregando}
        onRefresh={atualizar}
        ListHeaderComponent={
          <View style={styles.cabecalho}>
            <Text accessibilityRole="header" style={styles.titulo}>
              Denúncias da comunidade
            </Text>

            <Text>
              A situação é informada pelo autor e não representa confirmação oficial de atendimento.
            </Text>

            <View style={styles.botoes}>
              <Button
                title="Todas"
                accessibilityState={{ selected: !minhas }}
                disabled={salvando}
                onPress={() => setMinhas(false)}
              />

              <Button
                title="Minhas"
                accessibilityState={{ selected: minhas }}
                disabled={salvando}
                onPress={() => setMinhas(true)}
              />
            </View>

            <Text accessibilityRole="header">Filtrar por situação</Text>

            <View style={styles.botoes}>
              {situacoes.map(opcao => (
                <Pressable
                  key={opcao.valor}
                  accessibilityRole="button"
                  accessibilityState={{ selected: situacao === opcao.valor, disabled: salvando }}
                  disabled={salvando}
                  onPress={() => setSituacao(opcao.valor)}
                  style={[styles.filtro, situacao === opcao.valor && styles.filtroAtivo]}
                >
                  <Text style={styles.textoFiltro}>{situacao === opcao.valor ? `${opcao.titulo} · selecionado` : opcao.titulo}</Text>
                </Pressable>
              ))}
            </View>

            <Text accessibilityLiveRegion="polite">{itensVisiveis.length} denúncia(s) nesta seleção.</Text>

            <Button
              title="Nova denúncia"
              disabled={salvando}
              onPress={() => router.push('/denuncia-form')}
            />

            {erro && (
              <>
                <Text accessibilityRole="alert">{erro}</Text>

                <Button title="Tentar novamente" onPress={atualizar} />
              </>
            )}
          </View>
        }
        ListEmptyComponent={
          <View style={styles.cabecalho}>
            {carregando ? (
              <ActivityIndicator accessibilityLabel="Carregando denúncias" />
            ) : (
              !erro && <Text>Nenhuma denúncia encontrada para os filtros selecionados.</Text>
            )}
          </View>
        }
        renderItem={({ item }) => (
          <View style={styles.card}>
            <Text accessibilityRole="header">
              Denúncia #{item.id} · {item.autorNome}
            </Text>

            <Text>{new Date(item.criadaEm).toLocaleString('pt-BR')}</Text>

            <Text accessibilityLiveRegion="polite" style={styles.status}>
              {item.solucionado ? 'Solucionada pelo relato do autor' : 'Não solucionada'}
            </Text>

            <Text style={styles.relato}>{item.texto}</Text>

            <CarrosselDenuncia imagensUrls={item.imagensUrls} />

            <Text>
              {[item.logradouro, item.numero, item.bairro, `${item.cidade}/${item.uf}`]
                .filter(Boolean)
                .join(', ')}
            </Text>

            <Text>
              CEP: {item.cep.slice(0, 5)}-{item.cep.slice(5)}
            </Text>

            {!!item.complemento && <Text>Complemento: {item.complemento}</Text>}

            {!!item.pontoReferencia && <Text>Ponto de referência: {item.pontoReferencia}</Text>}

            {item.usuarioId === sessao?.usuario.id && (
              <View style={styles.acoes}>
                <Button
                  title="Editar denúncia"
                  disabled={salvando || carregando}
                  onPress={() =>
                    router.push({ pathname: '/denuncia-form', params: { id: String(item.id) } })
                  }
                />

                <Button
                  title={item.solucionado ? 'Reabrir denúncia' : 'Confirmar solução'}
                  disabled={salvando || carregando}
                  onPress={() => solicitar(item, 'situacao')}
                />

                <Button
                  title="Excluir denúncia"
                  disabled={salvando || carregando}
                  onPress={() => solicitar(item, 'excluir')}
                />
              </View>
            )}
          </View>
        )}
      />

      <Modal
        visible={!!acao}
        transparent
        animationType={reduzirMovimento ? 'none' : 'fade'}
        onRequestClose={() => {
          if (!ocupado.current) setAcao(null);
        }}
      >
        <View style={styles.fundo}>
          <View style={styles.modal} accessibilityViewIsModal>
            <Text accessibilityRole="header" style={styles.titulo}>
              {acao?.tipo === 'excluir'
                ? 'Excluir denúncia?'
                : acao?.denuncia.solucionado
                  ? 'Reabrir denúncia?'
                  : 'O problema foi solucionado?'}
            </Text>

            <Text>
              {acao?.tipo === 'excluir'
                ? 'A denúncia será excluída definitivamente.'
                : acao?.denuncia.solucionado
                  ? 'A denúncia voltará a aparecer como não solucionada.'
                  : 'Confirme somente se houve atendimento e uma medida resolveu o problema relatado.'}
            </Text>

            {erroAcao && <Text accessibilityRole="alert">{erroAcao}</Text>}

            <Button title="Cancelar" disabled={salvando} onPress={() => setAcao(null)} />

            <Button
              title={salvando ? 'Salvando...' : 'Confirmar'}
              disabled={salvando}
              onPress={() => void confirmar()}
            />
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}
