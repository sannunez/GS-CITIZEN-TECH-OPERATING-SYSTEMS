import { PreferenciasProvider, usePreferencias } from '@/contexts/PreferenciasContext';
import { Button, Text } from '@/components/ui';
import { useState } from 'react';
import { Stack } from 'expo-router';
import { StyleSheet, View } from 'react-native';
import { AuthProvider, useAuth } from '@/contexts/AuthContext';
import TransicaoAja from '@/components/TransicaoAja';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { StatusBar } from 'expo-status-bar';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    cena: {
      backgroundColor: colors.background,
    },

    tituloHeader: {
      fontSize: 17,
      fontWeight: '700',
    },

    carregando: {
      flex: 1,
      justifyContent: 'center',
    },

    recuperacao: {
      flex: 1,
      justifyContent: 'center',
      padding: 16,
      gap: 16,
    },

    acoes: {
      gap: 16,
      marginTop: 8,
    },
  });

export default function RootLayout() {
  return (
    <PreferenciasProvider>
      <AuthProvider>
        <Navegacao />
      </AuthProvider>
    </PreferenciasProvider>
  );
}

function Navegacao() {
  const styles = useEstilos(criarEstilos);
  const {
    colors,
    preferencias,
    reduzirMovimento,
    carregando: carregandoPreferencias,
  } = usePreferencias();

  const auth = useAuth();

  const [erroSaida, setErroSaida] = useState<string | null>(null);

  async function trocarConta() {
    setErroSaida(null);

    try {
      await auth.sair();
    } catch {
      setErroSaida('Não foi possível remover a sessão salva. Tente novamente.');
    }
  }

  if (auth.carregando || carregandoPreferencias) return <TransicaoAja />;

  if (auth.entrando) return <TransicaoAja onComplete={auth.concluirEntrada} />;

  if (auth.erroRestauracao) {
    return (
      <View style={styles.recuperacao}>
        <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
          Não foi possível validar sua sessão. Confira a conexão com a API.
        </Text>

        {erroSaida && (
          <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
            {erroSaida}
          </Text>
        )}

        <View style={styles.acoes}>
          <Button title="Tentar novamente" onPress={auth.tentarRestaurar} />

          <Button title="Usar outra conta" onPress={() => void trocarConta()} />
        </View>
      </View>
    );
  }

  return (
    <>
      <StatusBar style={preferencias.modoEscuro ? 'light' : 'dark'} />

      <Stack
        screenOptions={{
          headerShown: false,
          contentStyle: styles.cena,
          headerTintColor: colors.ink,
          headerStyle: { backgroundColor: colors.surface },
          headerShadowVisible: false,
          headerTitleStyle: styles.tituloHeader,
          animation: reduzirMovimento ? 'none' : 'default',
        }}
      >
        <Stack.Protected guard={!auth.sessao}>
          <Stack.Screen name="cadastro" />

          <Stack.Screen name="login" />
        </Stack.Protected>

        <Stack.Protected guard={!!auth.sessao}>
          <Stack.Screen name="(tabs)" />

          <Stack.Screen
            name="usuario/[id]"
            options={{ headerShown: true, title: 'Perfil da comunidade' }}
          />

          <Stack.Screen name="denuncias" options={{ headerShown: true, title: 'Denúncias' }} />

          <Stack.Screen name="denuncia-form" options={{ headerShown: true, title: 'Denúncia' }} />

          <Stack.Screen
            name="nova-postagem"
            options={{
              headerShown: true,
              title: 'Nova postagem',
            }}
          />
        </Stack.Protected>
      </Stack>
    </>
  );
}
