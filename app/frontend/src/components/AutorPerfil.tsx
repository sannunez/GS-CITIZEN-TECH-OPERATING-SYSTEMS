import { Pressable, StyleSheet, View } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { router, usePathname } from 'expo-router';
import { Text } from '@/components/ui';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useEstilos } from '@/hooks/useEstilos';
import type { ThemeColors } from '@/constants/theme';
import type { Autor } from '@/types/postagem';

type Props = {
  autor: Autor;
  detalhe?: string;
};

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    linha: {
      flexDirection: 'row',
      alignItems: 'center',
      gap: 11,
      minHeight: 48,
    },

    avatar: {
      width: 42,
      height: 42,
      borderRadius: 21,
      backgroundColor: colors.softBlue,
      borderWidth: 1,
      borderColor: colors.line,
      alignItems: 'center',
      justifyContent: 'center',
    },

    dados: {
      flex: 1,
      gap: 2,
    },

    nome: {
      fontWeight: '700',
      flexShrink: 1,
    },

    detalhe: {
      color: colors.muted,
      fontSize: 11,
    },
  });

export default function AutorPerfil({ autor, detalhe }: Props) {
  const styles = useEstilos(criarEstilos);
  const { colors } = usePreferencias();
  const caminho = usePathname();
  const mesmoPerfil = caminho === `/usuario/${autor.id}`;
  const conteudo = (
    <>
      <View
        style={styles.avatar}
        accessibilityElementsHidden
        importantForAccessibility="no-hide-descendants"
      >
        <Ionicons name="person" size={23} color={colors.primary} />
      </View>

      <View style={styles.dados}>
        <Text style={styles.nome}>{autor.nome}</Text>

        {!!detalhe && <Text style={styles.detalhe}>{detalhe}</Text>}
      </View>
    </>
  );

  if (mesmoPerfil) return <View style={styles.linha}>{conteudo}</View>;

  return (
    <Pressable
      style={styles.linha}
      accessibilityRole="button"
      accessibilityLabel={`Ver perfil de ${autor.nome}${detalhe ? `. ${detalhe}` : ''}`}
      accessibilityHint="Abre a biografia e as publicações desta pessoa."
      onPress={() => router.push({ pathname: '/usuario/[id]', params: { id: String(autor.id) } })}
    >
      {conteudo}
    </Pressable>
  );
}
