export interface ServicoPet {
  servicoId?: number;
  nome: string;
  preco: number;
  descricao?: string;
  duracaoMinutos: number;
  ativo: boolean;
}
