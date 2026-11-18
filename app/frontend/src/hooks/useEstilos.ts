import { useMemo } from 'react';
import { usePreferencias } from '@/contexts/PreferenciasContext';
import type { ThemeColors } from '@/constants/theme';

export function useEstilos<T>(criar: (colors: ThemeColors) => T) {
  const { colors } = usePreferencias();

  return useMemo(() => criar(colors), [colors, criar]);
}
