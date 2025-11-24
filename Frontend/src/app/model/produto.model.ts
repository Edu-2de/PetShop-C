export interface Produto {
  produtoId?: number;
  nome: string;
  descricao?: string;
  quantidade: number;
  preco: number;
  fornecedorId?: number;
  codigoBarras?: string;
  ativo: boolean;
  fornecedorNome?: string;
}
