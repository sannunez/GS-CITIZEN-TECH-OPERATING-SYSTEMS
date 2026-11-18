export type EnderecoDenuncia = {
  cep: string;
  logradouro: string;
  complemento: string;
  bairro: string;
  cidade: string;
  uf: string;
};

export type DadosDenuncia = EnderecoDenuncia & {
  numero: string;
  pontoReferencia: string | null;
  texto: string;
  imagensUrls: string | null;
};

export type Denuncia = DadosDenuncia & {
  id: number;
  usuarioId: number;
  autorNome: string;
  solucionado: boolean;
  criadaEm: string;
};
