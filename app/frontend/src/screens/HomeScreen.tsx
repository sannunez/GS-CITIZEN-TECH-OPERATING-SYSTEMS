import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { StatusBar } from 'expo-status-bar';
import { StyleSheet, View } from 'react-native';
import { Text } from '@/components/ui';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: colors.surface,
      alignItems: 'center',
      justifyContent: 'center',
    },
  });

export default function HomeScreen() {
  const styles = useEstilos(criarEstilos);

  return (
    <View style={styles.container}>
      <Text>Open up src/screens/HomeScreen.tsx to start working on your app!</Text>

      <StatusBar style="auto" />
    </View>
  );
}
