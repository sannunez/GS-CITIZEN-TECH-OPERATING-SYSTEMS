import { usePreferencias } from '@/contexts/PreferenciasContext';
import { Tabs } from 'expo-router';
import { Ionicons } from '@expo/vector-icons';
import { StyleSheet } from 'react-native';
import MarcaAja from '@/components/MarcaAja';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';

const icons = {
  index: 'trending-up-outline',
  feed: 'grid-outline',
  perfil: 'person-outline',
} as const;
const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    bar: {
      backgroundColor: colors.surface,
      borderTopColor: colors.line,
      elevation: 0,
    },

    label: {
      fontSize: 11,
      fontWeight: '600',
    },

    scene: {
      backgroundColor: colors.background,
    },
  });

export default function TabLayout() {
  const styles = useEstilos(criarEstilos);
  const { colors, preferencias, reduzirMovimento } = usePreferencias();

  return (
    <Tabs
      screenOptions={({ route }) => ({
        header: () => <MarcaAja />,
        tabBarActiveTintColor: colors.primary,
        tabBarInactiveTintColor: colors.muted,
        tabBarStyle: styles.bar,
        tabBarLabelStyle: [styles.label, { fontSize: preferencias.textoAmpliado ? 15 : 12 }],
        animation: reduzirMovimento ? 'none' : 'fade',
        tabBarIcon: ({ color, size }) => (
          <Ionicons
            name={icons[route.name as keyof typeof icons]}
            color={typeof color === 'string' ? color : colors.muted}
            size={size}
          />
        ),
        sceneStyle: styles.scene,
      })}
    >
      <Tabs.Screen
        name="index"
        options={{ title: 'Tópicos', tabBarAccessibilityLabel: 'Tópicos do bairro' }}
      />

      <Tabs.Screen
        name="feed"
        options={{ title: 'Feed', tabBarAccessibilityLabel: 'Feed de postagens' }}
      />

      <Tabs.Screen
        name="perfil"
        options={{ title: 'Perfil', tabBarAccessibilityLabel: 'Meu perfil e minhas postagens' }}
      />
    </Tabs>
  );
}
