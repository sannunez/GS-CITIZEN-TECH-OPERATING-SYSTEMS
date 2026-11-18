import { useEffect, useState } from 'react';
import { Animated, Image, StyleSheet, View } from 'react-native';
import { Text } from '@/components/ui';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { StatusBar } from 'expo-status-bar';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    tela: {
      flex: 1,
      backgroundColor: colors.brandBackground,
      alignItems: 'center',
      justifyContent: 'center',
      padding: 32,
    },

    circulo: {
      position: 'absolute',
      width: 360,
      height: 360,
      borderRadius: 180,
      borderWidth: 1,
      borderColor: '#FFFFFF22',
    },

    marca: {
      alignItems: 'center',
      gap: 24,
    },

    logo: {
      width: 220,
      height: 97,
    },

    titulo: {
      color: colors.onBrand,
      fontSize: 28,
      fontWeight: '700',
      textAlign: 'center',
      letterSpacing: -0.6,
    },

    subtitulo: {
      color: colors.onBrand,
      fontSize: 15,
      textAlign: 'center',
      lineHeight: 24,
    },

    assinatura: {
      position: 'absolute',
      bottom: 64,
      color: colors.onBrand,
      fontSize: 11,
      letterSpacing: 3,
      fontWeight: '600',
    },
  });

export default function TransicaoAja({ onComplete }: { onComplete?: () => void }) {
  const styles = useEstilos(criarEstilos);
  const { reduzirMovimento } = usePreferencias();

  const [entrada] = useState(() => new Animated.Value(0));
  const animacao = {
    opacity: entrada,
    transform: [{ translateY: entrada.interpolate({ inputRange: [0, 1], outputRange: [14, 0] }) }],
  };

  useEffect(() => {
    const timer = onComplete ? setTimeout(onComplete, reduzirMovimento ? 0 : 1200) : undefined;

    Animated.timing(entrada, {
      toValue: 1,
      duration: reduzirMovimento ? 0 : 650,
      useNativeDriver: true,
    }).start();

    return () => {
      clearTimeout(timer);
      entrada.stopAnimation();
    };
  }, [entrada, onComplete, reduzirMovimento]);

  return (
    <View style={styles.tela} accessibilityLiveRegion="polite">
      <StatusBar style="light" />

      <View style={styles.circulo} accessible={false} />

      <Animated.View style={[styles.marca, animacao]}>
        <Image
          source={require('../../assets/aja-logo.jpeg')}
          style={styles.logo}
          accessibilityLabel="AJA"
        />

        <Text style={styles.titulo}>Juntos, a gente transforma.</Text>

        <Text style={styles.subtitulo}>
          {onComplete ? 'Bem-vindo à sua comunidade.' : 'Preparando seu espaço na comunidade.'}
        </Text>
      </Animated.View>

      <Text style={styles.assinatura}>CONECTAR. PARTICIPAR. AGIR.</Text>
    </View>
  );
}
