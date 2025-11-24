export interface ServicoPet {
  servicoId: number;
  nome: string;
  descricao: string;
  preco: number;
  ativo: boolean;
  
  // Aliases para compatibilidade com componentes antigos
  id?: number;
  duracao?: number;
}
