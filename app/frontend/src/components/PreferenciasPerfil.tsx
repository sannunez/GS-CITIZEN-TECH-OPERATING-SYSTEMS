import { StyleSheet, Switch, View } from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { Button, Text } from '@/components/ui';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useEstilos } from '@/hooks/useEstilos';
import type { ThemeColors } from '@/constants/theme';

const opcoes = [
  {
    chave: 'modoEscuro',
    titulo: 'Modo escuro',
    descricao: 'Fundos escuros para uma leitura mais confortável.',
    icone: 'moon-outline',
  },
  {
    chave: 'altoContraste',
    titulo: 'Alto contraste',
    descricao: 'Reforça a diferença entre textos, botões e fundos. Funciona nos dois temas.',
    icone: 'contrast-outline',
  },
  {
    chave: 'textoAmpliado',
    titulo: 'Texto ampliado',
    descricao: 'Aumenta os textos em 25%, além do tamanho definido no seu aparelho.',
    icone: 'text-outline',
  },
  {
    chave: 'textoNegrito',
    titulo: 'Texto em negrito',
    descricao: 'Dá mais destaque às letras para facilitar a leitura.',
    icone: 'eye-outline',
  },
] as const;

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    painel: {
      marginTop: 28,
      gap: 8,
    },

    titulo: {
      fontSize: 19,
      fontWeight: '800',
    },

    detalhe: {
      color: colors.muted,
      fontSize: 12,
    },

    opcao: {
      flexDirection: 'row',
      alignItems: 'center',
      flexWrap: 'wrap',
      gap: 12,
      paddingVertical: 16,
      borderBottomWidth: 1,
      borderBottomColor: colors.line,
    },

    icone: {
      width: 40,
      height: 40,
      alignItems: 'center',
      justifyContent: 'center',
      borderRadius: 12,
      backgroundColor: colors.softBlue,
    },

    descricao: {
      flex: 1,
      minWidth: 120,
      gap: 4,
    },

    nome: {
      fontWeight: '700',
    },

    controle: {
      minWidth: 52,
      minHeight: 48,
    },

    erro: {
      gap: 12,
      marginTop: 12,
    },
  });

export default function PreferenciasPerfil() {
  const styles = useEstilos(criarEstilos);
  const { preferencias, colors, carregando, erro, atualizar, tentarSalvar } = usePreferencias();

  return (
    <View style={styles.painel}>
      <Text accessibilityRole="header" style={styles.titulo}>
        Acessibilidade e aparência
      </Text>

      <Text style={styles.detalhe}>
        Suas escolhas valem para todo o app e ficam salvas neste aparelho.
      </Text>

      {opcoes.map((opcao) => (
        <View key={opcao.chave} style={styles.opcao}>
          <View
            style={styles.icone}
            importantForAccessibility="no-hide-descendants"
            accessibilityElementsHidden
          >
            <Ionicons name={opcao.icone} color={colors.primary} size={22} />
          </View>

          <View style={styles.descricao}>
            <Text style={styles.nome}>{opcao.titulo}</Text>

            <Text style={styles.detalhe}>{opcao.descricao}</Text>
          </View>

          <Switch
            style={styles.controle}
            accessibilityLabel={opcao.titulo}
            accessibilityHint={opcao.descricao}
            value={preferencias[opcao.chave]}
            disabled={carregando}
            onValueChange={(valor) => atualizar(opcao.chave, valor)}
            trackColor={{ false: colors.line, true: colors.primary }}
            thumbColor={preferencias[opcao.chave] ? colors.onPrimary : colors.surface}
            ios_backgroundColor={colors.line}
          />
        </View>
      ))}

      <Text style={styles.detalhe}>
        Compatível com TalkBack e VoiceOver. Você também pode ampliar a fonte nas configurações do
        aparelho.
      </Text>

      {erro && (
        <View style={styles.erro}>
          <Text accessibilityRole="alert" accessibilityLiveRegion="polite">
            {erro}
          </Text>

          <Button title="Salvar preferências novamente" onPress={tentarSalvar} />
        </View>
      )}
    </View>
  );
}
