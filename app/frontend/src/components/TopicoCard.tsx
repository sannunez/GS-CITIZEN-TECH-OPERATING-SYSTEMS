import { Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { useState } from 'react';
import { Pressable, StyleSheet, View } from 'react-native';
import type { Topico } from '@/types/topico';
import { separarItensTopico } from '@/utils/topicos';
import AutorPerfil from '@/components/AutorPerfil';

type Props = {
  topico: Topico;
  curtido?: boolean;
  enviando?: boolean;
  erro?: string;
  onInteressar?: () => void;
  posicao?: number;
};

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    card: {
      borderWidth: 1,
      padding: 20,
      marginBottom: 12,
      borderColor: colors.line,
      backgroundColor: colors.surface,
      borderRadius: 18,
    },

    titulo: {
      marginTop: 10,
      fontWeight: '800',
      fontSize: 19,
      lineHeight: 26,
      letterSpacing: -0.4,
    },

    texto: {
      marginTop: 10,
      lineHeight: 21,
      color: colors.muted,
      fontSize: 13,
    },

    lista: {
      marginTop: 12,
      gap: 8,
    },

    item: {
      lineHeight: 22,
      fontSize: 13,
      color: colors.muted,
    },

    acao: {
      marginTop: 0,
      minHeight: 44,
      justifyContent: 'center',
      backgroundColor: colors.softBlue,
      borderRadius: 12,
      paddingHorizontal: 14,
      alignItems: 'center',
    },

    contador: {
      marginTop: 0,
      color: colors.green,
      fontWeight: '700',
      backgroundColor: colors.softGreen,
      padding: 10,
      borderRadius: 10,
      fontSize: 12,
    },

    engajamento: {
      flexDirection: 'row',
      flexWrap: 'wrap',
      alignItems: 'center',
      justifyContent: 'space-between',
      gap: 10,
      marginTop: 16,
    },

    leitura: {
      paddingVertical: 10,
      marginTop: 4,
    },

    acaoTexto: {
      color: colors.primary,
      fontSize: 12,
      fontWeight: '600',
    },

    erro: {
      marginTop: 12,
    },

    tendencia: {
      fontSize: 10,
      color: colors.muted,
      letterSpacing: 1,
      fontWeight: '600',
      marginBottom: 12,
    },
  });

export default function TopicoCard({
  topico,
  curtido,
  enviando,
  erro,
  onInteressar,
  posicao = 1,
}: Props) {
  const styles = useEstilos(criarEstilos);

  const [expandido, setExpandido] = useState(false);

  const itens = separarItensTopico(topico.assuntos);
  const textoLongo = topico.texto.length > 140;
  const listaLonga = itens.length > 3;
  const podeExpandir = textoLongo || listaLonga || itens.length > 0;
  const itensVisiveis = expandido ? itens : [];
  const titulo = topico.titulo || 'Tópico da comunidade';
  let textoInteresse = 'Tenho interesse';

  if (curtido) {
    textoInteresse = 'Interesse registrado';
  }

  if (enviando) {
    textoInteresse = 'Registrando interesse...';
  }

  return (
    <View style={styles.card}>
      <Text style={styles.tendencia}>
        {String(posicao).padStart(2, '0')} · EM PAUTA NA COMUNIDADE
      </Text>

      <AutorPerfil autor={topico.autor} />

      <Text accessibilityRole="header" style={styles.titulo}>
        {titulo}
      </Text>

      {topico.texto.length > 0 && (
        <Text style={styles.texto}>
          {textoLongo && !expandido
            ? `${topico.texto.slice(0, 140).trimEnd()}\u2026`
            : topico.texto}
        </Text>
      )}

      {itensVisiveis.length > 0 && (
        <View style={styles.lista}>
          {itensVisiveis.map((item, index) => (
            <Text
              key={`${index}-${item}`}
              accessibilityLabel={`Item ${index + 1} de ${itens.length}: ${item}`}
              style={styles.item}
            >
              {`\u2022 ${item}`}
            </Text>
          ))}
        </View>
      )}

      {podeExpandir && (
        <Pressable
          accessibilityRole="button"
          accessibilityLabel={`${expandido ? 'Ler menos' : 'Ler mais'}: ${titulo}`}
          accessibilityHint="Expande ou recolhe o texto e os itens deste tópico."
          accessibilityState={{ expanded: expandido }}
          onPress={() => setExpandido((atual) => !atual)}
          style={styles.leitura}
        >
          <Text style={styles.acaoTexto}>{expandido ? 'Ler menos' : 'Ler mais'}</Text>
        </Pressable>
      )}

      <View style={styles.engajamento}>
        <Text
          accessibilityLiveRegion="polite"
          accessibilityLabel={`${topico.interesses} interesses neste tópico`}
          style={styles.contador}
        >
          {topico.interesses} interesses
        </Text>

        {onInteressar && (
          <Pressable
            accessibilityRole="button"
            accessibilityLabel={`${curtido ? 'Interesse já registrado' : 'Demonstrar interesse'}: ${titulo}`}
            accessibilityHint="Adiciona um interesse ao contador deste tópico."
            accessibilityState={{
              disabled: curtido || enviando,
              selected: curtido,
              busy: enviando,
            }}
            disabled={curtido || enviando}
            onPress={onInteressar}
            style={styles.acao}
          >
            <Text style={styles.acaoTexto}>{textoInteresse}</Text>
          </Pressable>
        )}
      </View>

      {erro && (
        <Text accessibilityRole="alert" accessibilityLiveRegion="polite" style={styles.erro}>
          {erro}
        </Text>
      )}
    </View>
  );
}
