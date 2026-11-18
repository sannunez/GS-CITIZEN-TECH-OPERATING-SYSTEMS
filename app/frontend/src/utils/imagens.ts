export const imagensDemonstracao = [1, 2, 3].map(
  i => `https://placehold.co/800x500/png?text=AJA+DEMO+FOTO+${i}`,
);

export function separarImagens(valor: string | null) {
  return (valor ?? '')
    .split(';')
    .map(url => url.trim())
    .filter(Boolean);
}

export function validarImagens(valor: string) {
  const urls = separarImagens(valor);

  if (urls.length > 10) return 'Adicione no máximo dez imagens.';

  if (valor.length > 10000) return 'A lista de imagens deve ter até 10000 caracteres.';

  for (const item of urls) {
    try {
      const url = new URL(item);

      if (item.length > 1000 || !['http:', 'https:'].includes(url.protocol)) throw new Error();
    } catch {
      return 'Informe URLs HTTP ou HTTPS válidas, separadas por ponto e vírgula, com até 1000 caracteres cada.';
    }
  }

  return null;
}
