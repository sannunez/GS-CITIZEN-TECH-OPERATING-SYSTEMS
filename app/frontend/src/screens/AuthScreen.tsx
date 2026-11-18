import { Button, Text, TextInput } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { useState } from 'react';
import { Link } from 'expo-router';
import { ActivityIndicator, Image, KeyboardAvoidingView, Platform, ScrollView, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useAuth } from '@/contexts/AuthContext';

type Props = {
  modo: 'login' | 'cadastro';
};

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.background,
    },

    conteudo: {
      flexGrow: 1,
      justifyContent: 'flex-start',
      padding: 24,
      paddingBottom: 40,
      maxWidth: 500,
      width: '100%',
      alignSelf: 'center',
    },

    campos: {
      gap: 16,
      marginTop: 20,
    },

    input: {
      borderWidth: 1,
      padding: 12,
      borderColor: colors.line,
      borderRadius: 14,
      backgroundColor: colors.surface,
    },

    mensagem: {
      marginTop: 14,
      color: colors.danger,
    },

    acoes: {
      marginTop: 20,
    },

    link: {
      paddingVertical: 20,
      color: colors.primary,
      fontWeight: '600',
      textAlign: 'center',
    },

    hero: {
      backgroundColor: colors.brandBackground,
      borderRadius: 26,
      padding: 26,
      marginBottom: 32,
      gap: 18,
    },

    logo: {
      width: 95,
      height: 42,
    },

    heroTitulo: {
      fontSize: 29,
      lineHeight: 35,
      fontWeight: '800',
      color: colors.onBrand,
      letterSpacing: -1,
    },

    heroLegenda: {
      color: colors.onBrand,
      fontSize: 14,
      lineHeight: 21,
    },

    eyebrow: {
      color: colors.primary,
      fontSize: 10,
      fontWeight: '800',
      letterSpacing: 1.8,
      marginBottom: 10,
    },

    titulo: {
      fontSize: 25,
      fontWeight: '800',
      lineHeight: 32,
      letterSpacing: -0.7,
    },

    subtitulo: {
      color: colors.muted,
      marginTop: 8,
    },
  });

export default function AuthScreen({ modo }: Props) {
  const styles = useEstilos(criarEstilos);

  const { entrar } = useAuth();

  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState<string | null>(null);
  const [enviando, setEnviando] = useState(false);

  const cadastrando = modo === 'cadastro';

  async function enviar() {
    if (enviando) {
      return;
    }

    if (!email.trim() || !senha || (cadastrando && !nome.trim())) {
      setErro('Preencha os campos obrigatórios.');

      return;
    }

    const emailInformado = email.trim();

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(emailInformado)) {
      setErro('Informe um e-mail válido. Exemplo: nome@exemplo.com.');

      return;
    }

    if (cadastrando && senha.length < 8) {
      setErro('A senha deve ter pelo menos 8 caracteres.');

      return;
    }

    setEnviando(true);
    setErro(null);

    try {
      await entrar(modo, {
        nome: nome.trim(),
        email: emailInformado,
        senha,
      });
    } catch (error) {
      const mensagem = error instanceof Error ? error.message : 'Não foi possível entrar.';

      setErro(mensagem);
    } finally {
      setEnviando(false);
      setSenha('');
    }
  }

  return (
    <SafeAreaView style={styles.tela}>
      <KeyboardAvoidingView
        style={styles.tela}
        behavior={Platform.OS === 'ios' ? 'padding' : undefined}
      >
        <ScrollView contentContainerStyle={styles.conteudo} keyboardShouldPersistTaps="handled">
          <View style={styles.hero}>
            <Image
              source={require('../../assets/aja-logo.jpeg')}
              style={styles.logo}
              accessibilityLabel="AJA"
            />

            <Text style={styles.heroTitulo}>O seu bairro começa com você.</Text>

            <Text style={styles.heroLegenda}>
              Conecte ideias. Compartilhe histórias. Faça a diferença.
            </Text>
          </View>

          <Text style={styles.eyebrow}>BEM-VINDO À COMUNIDADE</Text>

          <Text accessibilityRole="header" style={styles.titulo}>
            {cadastrando ? 'Vamos fazer acontecer?' : 'Bom ter você de volta.'}
          </Text>

          <Text style={styles.subtitulo}>
            {cadastrando
              ? 'Crie sua conta e participe das mudanças.'
              : 'Entre para acompanhar o que move o seu bairro.'}
          </Text>

          <View style={styles.campos}>
            {cadastrando && (
              <TextInput
                accessibilityLabel="Nome"
                accessibilityHint="Informe o nome que será exibido no seu perfil."
                accessibilityState={{ disabled: enviando }}
                placeholder="Nome"
                value={nome}
                onChangeText={(valor) => {
                  setNome(valor);
                  setErro(null);
                }}
                style={styles.input}
                editable={!enviando}
                maxLength={100}
              />
            )}

            <TextInput
              accessibilityLabel="E-mail"
              accessibilityHint="Informe o e-mail usado para entrar na sua conta."
              accessibilityState={{ disabled: enviando }}
              placeholder="E-mail"
              value={email}
              onChangeText={(valor) => {
                setEmail(valor);
                setErro(null);
              }}
              keyboardType="email-address"
              autoCapitalize="none"
              autoCorrect={false}
              style={styles.input}
              editable={!enviando}
              maxLength={254}
            />

            <TextInput
              accessibilityLabel="Senha"
              accessibilityHint={
                cadastrando
                  ? 'Crie uma senha com pelo menos oito caracteres.'
                  : 'Informe a senha da sua conta.'
              }
              accessibilityState={{ disabled: enviando }}
              placeholder="Senha"
              value={senha}
              onChangeText={(valor) => {
                setSenha(valor);
                setErro(null);
              }}
              secureTextEntry
              autoCapitalize="none"
              autoCorrect={false}
              style={styles.input}
              editable={!enviando}
              maxLength={128}
            />
          </View>

          {erro && (
            <Text
              accessibilityRole="alert"
              accessibilityLiveRegion="polite"
              style={styles.mensagem}
            >
              {erro}
            </Text>
          )}

          {enviando && (
            <ActivityIndicator
              accessible
              accessibilityLabel={cadastrando ? 'Criando sua conta' : 'Validando seu acesso'}
              accessibilityRole="progressbar"
              style={styles.mensagem}
            />
          )}

          <View style={styles.acoes}>
            <Button
              title={cadastrando ? 'Cadastrar' : 'Entrar'}
              accessibilityLabel={cadastrando ? 'Cadastrar nova conta' : 'Entrar na conta'}
              disabled={enviando}
              onPress={() => void enviar()}
            />
          </View>

          <Link
            href={cadastrando ? '/login' : '/cadastro'}
            accessibilityRole="link"
            accessibilityLabel={
              cadastrando
                ? 'Já tenho conta. Ir para login'
                : 'Ainda não tenho conta. Ir para cadastro'
            }
            style={styles.link}
          >
            {cadastrando ? 'Já tenho conta' : 'Ainda não tenho conta'}
          </Link>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}
