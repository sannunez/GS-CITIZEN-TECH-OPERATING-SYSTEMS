import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useState } from 'react';
import { Pressable, StyleSheet, View } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import type { Postagem } from '@/types/postagem';
import AutorPerfil from '@/components/AutorPerfil';
import CarrosselImagens from '@/components/CarrosselImagens';

type Props = { postagem: Postagem; altura?: number };

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    card: {
      backgroundColor: colors.surface,
      borderWidth: 1,
      borderColor: colors.line,
      borderRadius: 20,
      marginBottom: 18,
      overflow: 'hidden',
    },

    header: {
      padding: 16,
    },

    legenda: {
      padding: 16,
      gap: 12,
    },

    selo: {
      flexDirection: 'row',
      alignItems: 'center',
      gap: 6,
    },

    comunidade: {
      fontSize: 11,
      color: colors.primary,
      fontWeight: '700',
    },

    texto: {
      fontSize: 14,
      lineHeight: 22,
    },

    lerMais: {
      color: colors.primary,
      fontWeight: '600',
      paddingVertical: 8,
    },
  });

export default function PostagemCard({ postagem }: Props) {
  const styles = useEstilos(criarEstilos);
  const { colors } = usePreferencias();

  const [expandido, setExpandido] = useState(false);
  const texto = postagem.texto ?? '';

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <AutorPerfil
          autor={postagem.autor}
          detalhe={new Date(postagem.criadaEm).toLocaleString('pt-BR')}
        />
      </View>

      <CarrosselImagens imagensUrls={postagem.imagemUrl} tipo="postagem" />

      <View style={styles.legenda}>
        <View style={styles.selo}>
          <Ionicons name="location-outline" size={14} color={colors.primary} />

          <Text style={styles.comunidade}>VOZES DA COMUNIDADE</Text>
        </View>

        {!!texto && (
          <Text
            style={styles.texto}
            numberOfLines={texto.length > 160 && !expandido ? 4 : undefined}
            accessibilityLabel={texto}
          >
            {texto}
          </Text>
        )}

        {texto.length > 160 && (
          <Pressable
            accessibilityRole="button"
            accessibilityState={{ expanded: expandido }}
            onPress={() => setExpandido((valor) => !valor)}
          >
            <Text style={styles.lerMais}>{expandido ? 'Mostrar menos' : 'Continuar lendo'}</Text>
          </Pressable>
        )}

      </View>
    </View>
  );
}
