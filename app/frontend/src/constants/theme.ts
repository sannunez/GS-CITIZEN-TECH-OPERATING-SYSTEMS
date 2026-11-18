const azulAja = '#3F7CEE';

const claro = {
  blue: azulAja,
  primary: azulAja,
  ink: '#142238',
  muted: '#5F6E83',
  line: '#E5EBF3',
  background: '#F6F8FC',
  surface: '#FFFFFF',
  softBlue: '#EDF3FF',
  green: '#16704A',
  softGreen: '#EAF7F0',
  onPrimary: '#FFFFFF',
  brandBackground: azulAja,
  onBrand: '#FFFFFF',
  danger: '#B42318',
};

export type ThemeColors = typeof claro;

const escuro: ThemeColors = {
  blue: azulAja,
  primary: azulAja,
  ink: '#F1F5FC',
  muted: '#B5C3D6',
  line: '#52647C',
  background: '#0B1220',
  surface: '#142033',
  softBlue: '#213854',
  green: '#8EE6B9',
  softGreen: '#153C2C',
  onPrimary: '#0A1730',
  brandBackground: azulAja,
  onBrand: '#FFFFFF',
  danger: '#FFB4AB',
};

const contrasteClaro: ThemeColors = {
  ...claro,
  primary: '#003B80',
  ink: '#000000',
  muted: '#1A1A1A',
  line: '#000000',
  background: '#FFFFFF',
  softBlue: '#E2EEFF',
  green: '#004421',
  softGreen: '#E2FFEF',
  brandBackground: '#00265C',
  danger: '#8B0000',
};

const contrasteEscuro: ThemeColors = {
  ...escuro,
  primary: '#FFE66D',
  ink: '#FFFFFF',
  muted: '#FFFFFF',
  line: '#FFFFFF',
  background: '#000000',
  surface: '#000000',
  softBlue: '#171717',
  green: '#A0FFC4',
  softGreen: '#0E2518',
  onPrimary: '#000000',
  brandBackground: '#000000',
  danger: '#FFB4AB',
};

export function obterCores(modoEscuro: boolean, altoContraste: boolean): ThemeColors {
  if (altoContraste) return modoEscuro ? contrasteEscuro : contrasteClaro;

  return modoEscuro ? escuro : claro;
}
