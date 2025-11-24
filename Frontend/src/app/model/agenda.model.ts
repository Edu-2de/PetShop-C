export interface Agenda {
  agendamentoId: number;
  animalId: number;
  servicoId: number;
  funcionarioId?: number;
  dataHora: Date;
  status: string;
  observacoes?: string;
  animalNome?: string;
  servicoNome?: string;
  funcionarioNome?: string;
  
  // Aliases para compatibilidade com componentes antigos
  id?: number;
  petid?: number;
  data?: Date;
}
