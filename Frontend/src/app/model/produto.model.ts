export interface ProdutoImagem {
  id: number;
  url: string;
}

export interface Produto {
  produtoId: number;
  nome: string;
  descricao: string;
  preco: number;
  quantidadeEstoque: number;
  ativo: boolean;
  fornecedorId: number;
  nomeFornecedor?: string;
  imagens?: ProdutoImagem[];
  
  // Aliases para compatibilidade com componentes antigos
  id?: number;
  categoria?: string;
  fotoUrl?: string;
  fornecedorid?: number;
}
