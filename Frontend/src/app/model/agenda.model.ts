export interface Agenda {
  agendamentoId?: number;
  animalId: number;
  funcionarioId?: number;
  servicoId: number;
  dataHora: string;
  status: 'Agendado' | 'Concluído' | 'Cancelado';
  observacoes?: string;
  animalNome?: string;
  servicoNome?: string;
  funcionarioNome?: string;
}
