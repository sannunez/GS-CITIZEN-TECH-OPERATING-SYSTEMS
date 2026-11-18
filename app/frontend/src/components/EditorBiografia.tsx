import { useRef, useState } from 'react';
import { StyleSheet, View } from 'react-native';
import { Button, Text, TextInput } from '@/components/ui';
import { useAuth } from '@/contexts/AuthContext';
import { useEstilos } from '@/hooks/useEstilos';
import type { ThemeColors } from '@/constants/theme';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    editor: {
      marginTop: 20,
      gap: 14,
    },

    campo: {
      minHeight: 130,
      textAlignVertical: 'top',
    },

    contador: {
      color: colors.muted,
      fontSize: 12,
    },

    acoes: {
      gap: 12,
    },
  });

export default function EditorBiografia() {
  const styles = useEstilos(criarEstilos);
  const { sessao, salvarBiografia } = useAuth();
  const [editando, setEditando] = useState(false);
  const [texto, setTexto] = useState('');
  const [salvando, setSalvando] = useState(false);
  const [erro, setErro] = useState<string | null>(null);
  const [mensagem, setMensagem] = useState<string | null>(null);
  const ocupado = useRef(false);

  function editar() {
    setTexto(sessao?.usuario.biografia ?? '');
    setErro(null);
    setMensagem(null);
    setEditando(true);
  }

  async function salvar() {
    if (ocupado.current) return;

    if (texto.length > 1000) {
      setErro('A biografia deve ter até 1000 caracteres.');
      return;
    }

    ocupado.current = true;
    setSalvando(true);
    setErro(null);

    try {
      await salvarBiografia(texto.trim());
      setEditando(false);
      setMensagem('Biografia atualizada.');
    } catch (error) {
      setErro(error instanceof Error ? error.message : 'Não foi possível salvar sua biografia.');
    } finally {
      ocupado.current = false;
      setSalvando(false);
    }
  }

  return (
    <View style={styles.editor}>
      {editando ? (
        <>
          <Text accessibilityRole="header">Sua biografia</Text>

          <TextInput
            accessibilityLabel="Editar sua biografia"
            accessibilityHint="Até mil caracteres. Deixe vazio para remover a biografia."
            placeholder="Conte um pouco sobre você e sua relação com o bairro."
            value={texto}
            onChangeText={setTexto}
            editable={!salvando}
            maxLength={1000}
            multiline
            style={styles.campo}
          />

          <Text style={styles.contador}>{texto.length}/1000 caracteres</Text>

          {erro && <Text accessibilityRole="alert">{erro}</Text>}

          <View style={styles.acoes}>
            <Button title={salvando ? 'Salvando...' : 'Salvar biografia'} disabled={salvando} onPress={() => void salvar()} />

            <Button title="Cancelar edição" disabled={salvando} onPress={() => setEditando(false)} />
          </View>
        </>
      ) : (
        <>
          <Button title="Editar biografia" onPress={editar} />

          {mensagem && <Text accessibilityLiveRegion="polite">{mensagem}</Text>}
        </>
      )}
    </View>
  );
}
