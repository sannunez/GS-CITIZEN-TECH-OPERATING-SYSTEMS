import { usePreferencias } from '@/contexts/PreferenciasContext';
import { useState } from 'react';
import { Pressable, StyleSheet, Text as NativeText, TextInput as NativeInput } from 'react-native';
import type { ButtonProps, TextProps, TextInputProps, TextStyle } from 'react-native';
import type { ThemeColors } from '@/constants/theme';
import { useEstilos } from '@/hooks/useEstilos';

const criarEstilos = (colors: ThemeColors) =>
  StyleSheet.create({
    text: {
      color: colors.ink,
      fontSize: 14,
      lineHeight: 21,
    },

    button: {
      backgroundColor: colors.primary,
      minHeight: 48,
      borderRadius: 14,
      paddingHorizontal: 20,
      paddingVertical: 12,
      justifyContent: 'center',
      alignItems: 'center',
    },

    buttonLabel: {
      color: colors.onPrimary,
      fontSize: 14,
      fontWeight: '700',
      textAlign: 'center',
    },

    disabled: {
      opacity: 0.45,
    },

    pressed: {
      opacity: 0.75,
    },

    input: {
      backgroundColor: colors.surface,
      borderWidth: 1,
      borderColor: colors.line,
      borderRadius: 14,
      paddingHorizontal: 16,
      paddingVertical: 14,
      minHeight: 50,
      color: colors.ink,
      fontSize: 15,
    },

    focused: {
      borderColor: colors.primary,
      backgroundColor: colors.surface,
    },
  });

export function Text({ style, ...props }: TextProps) {
  const styles = useEstilos(criarEstilos);
  const { preferencias } = usePreferencias();
  const base = StyleSheet.flatten([styles.text, style]) ?? {};
  const leitura = estiloLeitura(base, preferencias.textoAmpliado, preferencias.textoNegrito);

  return <NativeText {...props} style={[styles.text, style, leitura]} />;
}

export function Button({ title, disabled, onPress, color, ...props }: ButtonProps) {
  const styles = useEstilos(criarEstilos);

  const corBotao = color ? { backgroundColor: color } : undefined;

  return (
    <Pressable
      {...props}
      accessibilityRole="button"
      accessibilityState={{ ...props.accessibilityState, disabled }}
      disabled={disabled}
      onPress={onPress}
      style={({ pressed }) => [
        styles.button,
        corBotao,
        disabled && styles.disabled,
        pressed && styles.pressed,
      ]}
    >
      <Text style={styles.buttonLabel}>{title}</Text>
    </Pressable>
  );
}

export function TextInput({ style, onFocus, onBlur, ...props }: TextInputProps) {
  const styles = useEstilos(criarEstilos);
  const { colors, preferencias } = usePreferencias();

  const [focused, setFocused] = useState(false);
  const base = StyleSheet.flatten([styles.input, style]) ?? {};
  const leitura = estiloLeitura(base, preferencias.textoAmpliado, preferencias.textoNegrito);

  return (
    <NativeInput
      {...props}
      placeholderTextColor={colors.muted}
      selectionColor={colors.primary}
      onFocus={(event) => {
        setFocused(true);
        onFocus?.(event);
      }}
      onBlur={(event) => {
        setFocused(false);
        onBlur?.(event);
      }}
      style={[styles.input, style, focused && styles.focused, leitura]}
    />
  );
}

function estiloLeitura(base: TextStyle, ampliar: boolean, negrito: boolean): TextStyle {
  const escala = ampliar ? 1.25 : 1;
  const tamanho = Math.max(base.fontSize ?? 14, 12);
  const alturaLinha = Math.max(base.lineHeight ?? tamanho * 1.5, tamanho * 1.4);
  const peso = base.fontWeight === 'bold' ? 700 : Number(base.fontWeight) || 400;

  return {
    fontSize: tamanho * escala,
    lineHeight: alturaLinha * escala,
    ...(negrito && peso < 600 ? { fontWeight: '600' } : {}),
  };
}
