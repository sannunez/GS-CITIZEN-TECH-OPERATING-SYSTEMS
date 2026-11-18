import { Button, Text, TextInput } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { useEffect, useRef, useState } from 'react';
import { router, useLocalSearchParams } from 'expo-router';
import { ActivityIndicator, KeyboardAvoidingView, Platform, ScrollView, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useAuth } from '@/contexts/AuthContext';
import { obterDenuncia, salvarDenuncia } from '@/services/denuncias';
import { buscarEnderecoCep } from '@/services/viacep';
import { ApiError } from '@/services/http';
import type { EnderecoDenuncia } from '@/types/denuncia';
import { imagensDemonstracao, separarImagens, validarImagens } from '@/utils/denuncias';
import CarrosselDenuncia from '@/components/CarrosselDenuncia';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    conteudo: {
      padding: 22,
      gap: 14,
    },

    titulo: {
      fontSize: 26,
      fontWeight: '800',
      letterSpacing: -0.6,
      lineHeight: 34,
    },

    rotulo: {
      marginTop: 8,
      fontWeight: '700',
      color: colors.muted,
      fontSize: 12,
    },

    input: {
      borderWidth: 1,
      borderColor: colors.line,
      borderRadius: 14,
      padding: 12,
      color: colors.ink,
      backgroundColor: colors.surface,
    },

    texto: {
      minHeight: 90,
      textAlignVertical: 'top',
    },

    somenteLeitura: {
      backgroundColor: colors.softBlue,
    },

    endereco: {
      gap: 10,
    },

    erro: {
      color: colors.danger,
    },
  });

export default function DenunciaFormScreen() {
  const styles = useEstilos(criarEstilos);

  const { id: parametroId } = useLocalSearchParams<{ id?: string }>();
  const id = parametroId ? Number(parametroId) : undefined;
  const idInvalido = id !== undefined && (!Number.isSafeInteger(id) || id <= 0);
  const { sessao, sair } = useAuth();
  const token = sessao?.token;

  const [cep, setCep] = useState('');
  const [endereco, setEndereco] = useState<EnderecoDenuncia | null>(null);
  const [numero, setNumero] = useState('');
  const [referencia, setReferencia] = useState('');
  const [texto, setTexto] = useState('');
  const [imagens, setImagens] = useState('');
  const [erro, setErro] = useState<string | null>(idInvalido ? 'Denúncia inválida.' : null);
  const [buscando, setBuscando] = useState(false);
  const [carregando, setCarregando] = useState(!!parametroId && !idInvalido);
  const [erroCarga, setErroCarga] = useState(idInvalido);
  const [tentativa, setTentativa] = useState(0);
  const [enviando, setEnviando] = useState(false);

  const consulta = useRef<AbortController | null>(null);
  const ocupado = useRef(false);

  const desabilitado = enviando || carregando || erroCarga;

  useEffect(() => {
    const controller = new AbortController();

    if (idInvalido || !id || !token) return;

    void obterDenuncia(id, token, controller.signal)
      .then((d) => {
        if (controller.signal.aborted) return;

        if (d.usuarioId !== sessao?.usuario.id)
          throw new Error('Somente o autor pode editar esta denúncia.');

        setCep(d.cep);
        setEndereco(d);
        setNumero(d.numero);
        setReferencia(d.pontoReferencia ?? '');
        setTexto(d.texto);
        setImagens(d.imagensUrls ?? '');
      })
      .catch((error) => {
        if (!controller.signal.aborted) {
          setErro(error.message);
          setErroCarga(true);
        }
      })
      .finally(() => {
        if (!controller.signal.aborted) setCarregando(false);
      });

    return () => controller.abort();
  }, [id, idInvalido, token, sessao?.usuario.id, tentativa]);

  useEffect(() => () => consulta.current?.abort(), []);

  function alterarCep(valor: string) {
    consulta.current?.abort();
    consulta.current = null;
    setBuscando(false);
    setEndereco(null);
    setErro(null);
    setCep(valor.replace(/\D/g, '').slice(0, 8));
  }

  async function buscar() {
    consulta.current?.abort();

    const controller = new AbortController();

    consulta.current = controller;
    setBuscando(true);
    setEndereco(null);
    setErro(null);

    try {
      const resultado = await buscarEnderecoCep(cep, controller.signal);

      if (!controller.signal.aborted) setEndereco(resultado);
    } catch (error) {
      if (!controller.signal.aborted)
        setErro(error instanceof Error ? error.message : 'Não foi possível buscar o CEP.');
    } finally {
      if (consulta.current === controller) setBuscando(false);
    }
  }

  async function salvar() {
    if (!sessao || ocupado.current) return;

    if (!texto.trim()) {
      setErro('Descreva a denúncia em até 500 caracteres.');

      return;
    }

    if (texto.length > 500) {
      setErro('A denúncia deve ter até 500 caracteres.');

      return;
    }

    if (!endereco || endereco.cep !== cep) {
      setErro('Busque e confirme o CEP antes de salvar.');

      return;
    }

    if (!numero.trim()) {
      setErro('Informe o número do local ou S/N.');

      return;
    }

    const erroImagens = validarImagens(imagens);

    if (erroImagens) {
      setErro(erroImagens);

      return;
    }

    ocupado.current = true;
    setEnviando(true);
    setErro(null);

    try {
      await salvarDenuncia(
        {
          cep: endereco.cep,
          logradouro: endereco.logradouro,
          complemento: endereco.complemento,
          bairro: endereco.bairro,
          cidade: endereco.cidade,
          uf: endereco.uf,
          numero: numero.trim(),
          pontoReferencia: referencia.trim() || null,
          texto: texto.trim(),
          imagensUrls: separarImagens(imagens).join(';') || null,
        },
        sessao.token,
        id,
      );
      router.back();
    } catch (error) {
      if (error instanceof ApiError && error.status === 401) {
        await sair();

        return;
      }

      setErro(error instanceof Error ? error.message : 'Não foi possível salvar a denúncia.');
    } finally {
      ocupado.current = false;
      setEnviando(false);
    }
  }

  return (
    <SafeAreaView edges={['left', 'right', 'bottom']} style={styles.tela}>
      <KeyboardAvoidingView
        style={styles.tela}
        behavior={Platform.OS === 'ios' ? 'padding' : undefined}
      >
        <ScrollView keyboardShouldPersistTaps="handled" contentContainerStyle={styles.conteudo}>
          <Text accessibilityRole="header" style={styles.titulo}>
            {id ? 'Editar denúncia' : 'Registrar denúncia'}
          </Text>

          <Text>
            Informe o endereço do problema. O relato será visível às pessoas conectadas ao AJA.
          </Text>

          {carregando && <ActivityIndicator accessibilityLabel="Carregando denúncia" />}

          <Text style={styles.rotulo}>CEP do local</Text>

          <TextInput
            accessibilityLabel="CEP do local"
            placeholder="Oito dígitos"
            keyboardType="number-pad"
            maxLength={9}
            value={cep}
            onChangeText={alterarCep}
            editable={!desabilitado}
            style={styles.input}
          />

          <Button
            title={buscando ? 'Buscando...' : 'Buscar CEP'}
            disabled={desabilitado || buscando}
            onPress={() => void buscar()}
          />

          {endereco && (
            <View style={styles.endereco}>
              <Text accessibilityLiveRegion="polite">
                Endereço consultado. Confira se corresponde ao local.
              </Text>

              {(
                [
                  ['Rua', endereco.logradouro],
                  ['Bairro', endereco.bairro],
                  ['Cidade', endereco.cidade],
                  ['UF', endereco.uf],
                  ['Complemento do CEP', endereco.complemento],
                ] as const
              ).map(([rotulo, valor]) => (
                <View key={rotulo}>
                  <Text>{rotulo}</Text>

                  <TextInput
                    accessibilityLabel={`${rotulo}, preenchido pelo ViaCEP, somente leitura`}
                    value={valor || 'Não informado pelo ViaCEP'}
                    editable={false}
                    style={[styles.input, styles.somenteLeitura]}
                  />
                </View>
              ))}

              {!endereco.logradouro && (
                <Text>
                  Este CEP não identifica uma rua específica. Detalhe o local no ponto de
                  referência.
                </Text>
              )}
            </View>
          )}

          <Text style={styles.rotulo}>Número (ou S/N)</Text>

          <TextInput
            accessibilityLabel="Número do local ou S/N"
            value={numero}
            onChangeText={setNumero}
            maxLength={20}
            editable={!desabilitado}
            style={styles.input}
          />

          <Text style={styles.rotulo}>Ponto de referência (opcional)</Text>

          <TextInput
            accessibilityLabel="Ponto de referência, opcional"
            value={referencia}
            onChangeText={setReferencia}
            maxLength={200}
            editable={!desabilitado}
            style={styles.input}
          />

          <Text style={styles.rotulo}>Descrição da denúncia</Text>

          <TextInput
            accessibilityLabel="Descrição da denúncia, até 500 caracteres"
            value={texto}
            onChangeText={setTexto}
            maxLength={500}
            multiline
            editable={!desabilitado}
            style={[styles.input, styles.texto]}
          />

          <Text>{texto.length}/500 caracteres</Text>

          <Text style={styles.rotulo}>URLs das fotos (opcional)</Text>

          <Text>Separe até dez URLs por ponto e vírgula (;). Use links diretos para imagens.</Text>

          <TextInput
            accessibilityLabel="URLs das fotos separadas por ponto e vírgula"
            value={imagens}
            onChangeText={setImagens}
            multiline
            autoCapitalize="none"
            autoCorrect={false}
            maxLength={10000}
            editable={!desabilitado}
            style={[styles.input, styles.texto]}
          />

          <Button
            title="Inserir 3 fotos de demonstração"
            disabled={desabilitado}
            onPress={() => setImagens(imagensDemonstracao.join(';'))}
          />

          <Text>As fotos de demonstração são ilustrativas e não comprovam a denúncia.</Text>

          {!validarImagens(imagens) && <CarrosselDenuncia imagensUrls={imagens} />}

          {erro && (
            <Text accessibilityRole="alert" accessibilityLiveRegion="polite" style={styles.erro}>
              {erro}
            </Text>
          )}

          {erroCarga && !idInvalido && (
            <Button
              title="Tentar carregar novamente"
              onPress={() => {
                setCarregando(true);
                setErroCarga(false);
                setErro(null);
                setTentativa((t) => t + 1);
              }}
            />
          )}

          <Button
            title={enviando ? 'Salvando...' : 'Salvar denúncia'}
            disabled={desabilitado || buscando}
            onPress={() => void salvar()}
          />
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}
