import { Button, Text } from '@/components/ui';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useRef, useState, type ComponentRef } from 'react';
import { ActivityIndicator, Image, ScrollView, StyleSheet, View } from 'react-native';
import { imagensDemonstracao, separarImagens } from '@/utils/imagens';

type Props = {
  imagensUrls: string | null;
  tipo?: 'denuncia' | 'postagem';
};

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    container: {
      marginVertical: 12,
      gap: 8,
    },

    feed: {
      marginVertical: 0,
      paddingBottom: 12,
    },

    controlesFeed: {
      marginHorizontal: 16,
    },

    foto: {
      height: 210,
      alignItems: 'center',
      justifyContent: 'center',
      backgroundColor: colors.softBlue,
      borderRadius: 16,
      overflow: 'hidden',
    },

    imagem: {
      width: '100%',
      height: '100%',
    },

    contador: {
      textAlign: 'center',
      fontSize: 12,
      color: colors.muted,
    },

    controles: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      gap: 12,
    },
  });

function Foto({
  url,
  indice,
  total,
  largura,
  tipo,
}: {
  url: string;
  indice: number;
  total: number;
  largura: number;
  tipo: NonNullable<Props['tipo']>;
}) {
  const styles = useEstilos(criarEstilos);

  const [falhou, setFalhou] = useState(false);
  const [carregando, setCarregando] = useState(true);

  const dimensoesFoto = {
    width: largura,
    height: tipo === 'postagem' ? largura / 1.15 : 210,
  };
  const descricao = tipo === 'postagem' ? 'da postagem' : 'da denúncia';

  return (
    <View style={[styles.foto, dimensoesFoto]}>
      {falhou ? (
        <Text accessibilityRole="alert">Imagem {indice + 1} indisponível.</Text>
      ) : (
        <Image
          source={{ uri: url }}
          style={styles.imagem}
          resizeMode="contain"
          accessibilityLabel={`Foto ${indice + 1} de ${total} ${descricao}. O relato descreve a situação.`}
          onLoad={() => setCarregando(false)}
          onError={() => {
            setFalhou(true);
            setCarregando(false);
          }}
        />
      )}

      {carregando && (
        <ActivityIndicator
          style={StyleSheet.absoluteFill}
          accessibilityLabel={`Carregando foto ${indice + 1}`}
        />
      )}
    </View>
  );
}

export default function CarrosselImagens({ imagensUrls, tipo = 'denuncia' }: Props) {
  return <Galeria key={`${tipo}:${imagensUrls}`} imagensUrls={imagensUrls} tipo={tipo} />;
}

function Galeria({ imagensUrls, tipo = 'denuncia' }: Props) {
  const styles = useEstilos(criarEstilos);
  const { reduzirMovimento } = usePreferencias();

  const urls = separarImagens(imagensUrls);
  const descricao = tipo === 'postagem' ? 'da postagem' : 'da denúncia';

  const [largura, setLargura] = useState(0);
  const [indice, setIndice] = useState(0);

  const scroll = useRef<ComponentRef<typeof ScrollView>>(null);

  if (!urls.length) return null;

  function ir(novo: number) {
    setIndice(novo);
    scroll.current?.scrollTo({ x: novo * largura, animated: !reduzirMovimento });
  }

  return (
    <View
      onLayout={(e) => setLargura(e.nativeEvent.layout.width)}
      style={[styles.container, tipo === 'postagem' && styles.feed]}
    >
      {largura > 0 && (
        <ScrollView
          ref={scroll}
          horizontal
          pagingEnabled
          showsHorizontalScrollIndicator
          accessibilityLabel={`Fotos ${descricao}. Deslize horizontalmente ou use os botões.`}
          onMomentumScrollEnd={(e) =>
            setIndice(
              Math.min(
                urls.length - 1,
                Math.max(0, Math.round(e.nativeEvent.contentOffset.x / largura)),
              ),
            )
          }
        >
          {urls.map((url, i) => (
            <Foto key={`${i}:${url}`} url={url} indice={i} total={urls.length} largura={largura} tipo={tipo} />
          ))}
        </ScrollView>
      )}

      <Text accessibilityLiveRegion="polite" style={styles.contador}>
        Foto {indice + 1} de {urls.length}
      </Text>

      {urls.length > 1 && (
        <View style={[styles.controles, tipo === 'postagem' && styles.controlesFeed]}>
          <Button
            title="Anterior"
            accessibilityLabel="Foto anterior"
            disabled={indice === 0}
            onPress={() => ir(indice - 1)}
          />

          <Button
            title="Próxima"
            accessibilityLabel="Próxima foto"
            disabled={indice >= urls.length - 1}
            onPress={() => ir(indice + 1)}
          />
        </View>
      )}

      {urls.some((url) => imagensDemonstracao.includes(url)) && (
        <Text style={tipo === 'postagem' ? styles.controlesFeed : undefined}>Fotos de demonstração: não representam uma ocorrência real.</Text>
      )}
    </View>
  );
}
