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

  // Novos campos
  categoriaId?: number;
  nomeFornecedor?: string;
  imagens?: ProdutoImagem[];

  // Aliases para compatibilidade (opcionais)
  id?: number;
  categoria?: string;     // String para exibição
  fotoUrl?: string;
  fornecedorid?: number;  // Lowercase alias
}
