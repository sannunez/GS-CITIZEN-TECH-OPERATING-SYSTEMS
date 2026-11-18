import { Button, Text, TextInput } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { useRef, useState } from 'react';
import { router } from 'expo-router';
import { KeyboardAvoidingView, Platform, ScrollView, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useAuth } from '@/contexts/AuthContext';
import { criarPostagem } from '@/services/postagens';
import { ApiError } from '@/services/http';
import CarrosselImagens from '@/components/CarrosselImagens';
import { imagensDemonstracao, separarImagens, validarImagens } from '@/utils/imagens';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    conteudo: {
      padding: 22,
      gap: 8,
    },

    input: {
      borderWidth: 1,
      padding: 12,
      marginTop: 16,
      borderColor: colors.line,
      borderRadius: 14,
      backgroundColor: colors.surface,
    },

    texto: {
      minHeight: 120,
      textAlignVertical: 'top',
    },

    mensagem: {
      marginTop: 16,
    },

    acoes: {
      marginTop: 20,
    },
  });

export default function NovaPostagemScreen() {
  const styles = useEstilos(criarEstilos);

  const { sessao, sair } = useAuth();

  const [texto, setTexto] = useState('');
  const [imagemUrl, setImagemUrl] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [enviando, setEnviando] = useState(false);

  const ocupado = useRef(false);

  async function publicar() {
    if (!sessao || ocupado.current) {
      return;
    }

    if (!texto.trim() && separarImagens(imagemUrl).length === 0) {
      setErro('Informe um texto ou uma URL de imagem.');

      return;
    }

    const erroImagens = validarImagens(imagemUrl);

    if (erroImagens) {
      setErro(erroImagens);

      return;
    }

    ocupado.current = true;
    setEnviando(true);
    setErro(null);

    try {
      await criarPostagem(
        {
          texto: texto.trim() || null,
          imagemUrl: separarImagens(imagemUrl).join(';') || null,
          usuarioId: sessao.usuario.id,
        },
        sessao.token,
      );

      router.back();
    } catch (error) {
      if (error instanceof ApiError && error.status === 401) {
        await sair();

        return;
      }

      const mensagem = error instanceof Error ? error.message : 'Não foi possível publicar.';

      setErro(mensagem);
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
          <Text accessibilityRole="header">Publicar como {sessao?.usuario.nome}</Text>

          <TextInput
            accessibilityLabel="Texto da postagem"
            accessibilityHint="Descreva a situação de acessibilidade que deseja compartilhar."
            accessibilityState={{ disabled: enviando }}
            placeholder="Texto da postagem"
            multiline
            value={texto}
            onChangeText={setTexto}
            editable={!enviando}
            style={[styles.input, styles.texto]}
          />

          <TextInput
            accessibilityLabel="URLs das imagens da postagem"
            accessibilityHint="Campo opcional. Separe até dez URLs por ponto e vírgula."
            accessibilityState={{ disabled: enviando }}
            placeholder="URLs das imagens separadas por ; (opcional)"
            value={imagemUrl}
            onChangeText={setImagemUrl}
            editable={!enviando}
            autoCapitalize="none"
            autoCorrect={false}
            keyboardType="url"
            multiline
            maxLength={10000}
            style={styles.input}
          />

          <Text>Separe até dez URLs por ponto e vírgula (;). Use links diretos para imagens.</Text>

          <Button
            title="Usar imagens de demonstração"
            disabled={enviando}
            onPress={() => setImagemUrl(imagensDemonstracao.join(';'))}
          />

          {!!validarImagens(imagemUrl) && <Text accessibilityRole="alert">{validarImagens(imagemUrl)}</Text>}

          {!validarImagens(imagemUrl) && <CarrosselImagens imagensUrls={imagemUrl} tipo="postagem" />}

          {erro && (
            <Text
              accessibilityRole="alert"
              accessibilityLiveRegion="polite"
              style={styles.mensagem}
            >
              {erro}
            </Text>
          )}

          <View style={styles.acoes}>
            <Button
              title={enviando ? 'Publicando...' : 'Publicar'}
              accessibilityLabel={enviando ? 'Publicando postagem' : 'Publicar postagem'}
              disabled={enviando}
              onPress={() => void publicar()}
            />
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}
