import { Image, StyleSheet, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    safe: {
      backgroundColor: colors.surface,
    },

    header: {
      flexDirection: 'row',
      alignItems: 'center',
      justifyContent: 'space-between',
      paddingHorizontal: 22,
      paddingVertical: 14,
      backgroundColor: colors.surface,
      borderBottomWidth: 1,
      borderBottomColor: colors.line,
    },

    logo: {
      width: 85,
      height: 38,
      borderRadius: 9,
    },

    meta: {
      alignItems: 'flex-end',
      flexShrink: 1,
      maxWidth: '68%',
    },

    local: {
      fontSize: 12,
      fontWeight: '700',
    },

    caption: {
      color: colors.muted,
      fontSize: 11,
    },
  });

export default function MarcaAja() {
  const styles = useEstilos(criarEstilos);

  return (
    <SafeAreaView edges={['top']} style={styles.safe}>
      <View style={styles.header}>
        <Image
          source={require('../../assets/aja-logo.jpeg')}
          style={styles.logo}
          accessibilityLabel="AJA"
        />

        <View style={styles.meta}>
          <Text style={styles.local}>Parque Doroteia</Text>

          <Text style={styles.caption}>Sua comunidade, mais perto</Text>
        </View>
      </View>
    </SafeAreaView>
  );
}
