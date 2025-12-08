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

  // Campos para exibição
  categoriaId?: number;
  nomeCategoria?: string;     // [NOVO] - Nome da categoria para exibição
  nomeFornecedor?: string;
  imagens?: ProdutoImagem[];

  // Aliases para compatibilidade (opcionais)
  id?: number;
  categoria?: string;     // String para exibição (alias para nomeCategoria)
  fotoUrl?: string;
  fornecedorid?: number;
}
