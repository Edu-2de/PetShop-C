export interface Venda {
  vendaId: number;
  tutorId?: number; // ➕ Adicionado
  usuarioId?: number; // ➕ Adicionado
  dataVenda: Date;
  valorTotal: number;
  formaPagamento: string;
  observacoes?: string; // ➕ Adicionado
  status?: string; // ➕ Adicionado
  itens: ItemVenda[];
  mostrarDetalhes?: boolean; // ➕ Para controle da UI
}

export interface ItemVenda {
  itemVendaId?: number; // ➕ Adicionado
  produtoId?: number; // ➕ Adicionado
  produtoNome?: string;
  servicoId?: number; // ➕ Adicionado
  servicoNome?: string;
  quantidade: number;
  precoUnitario: number;
}

export interface CreateVendaDto {
  tutorId?: number | null; // 🆕 Agora é opcional
  usuarioId?: number | null; // ➕ Adicionado para vincular a venda ao usuário
  funcionarioId?: number;
  formaPagamento: string;
  observacoes?: string;
  itens: CreateItemVendaDto[];

  // 🆕 NOVOS CAMPOS: Para criar tutor automaticamente
  nomeCliente?: string;
  emailCliente?: string;
  telefoneCliente?: string;
  enderecoCliente?: string;
}

export interface CreateItemVendaDto {
  produtoId?: number;
  servicoId?: number;
  quantidade: number;
}
