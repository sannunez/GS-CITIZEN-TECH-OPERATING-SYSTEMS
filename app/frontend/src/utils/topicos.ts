export function separarItensTopico(assuntos: string[]) {
  const itens = assuntos
    .flatMap(assunto => assunto.split(';'))
    .map(item => item.trim())
    .filter(item => item.length > 0);

  return Array.from(new Set(itens));
}
