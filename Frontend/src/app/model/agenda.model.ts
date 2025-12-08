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
  
  // ➕ Objetos completos para melhor rastreamento
  pet?: {
    animalId: number;
    nome: string;
    especie: string;
    raca: string;
    tutorId: number;
  };
  
  servico?: {
    servicoId: number;
    nome: string;
    preco: number;
    duracaoMinutos: number;
  };
  
  funcionario?: {
    funcionarioId: number;
    nome: string;
    cargo: string;
  };
  
  // Aliases para compatibilidade com componentes antigos
  id?: number;
  petid?: number;
  data?: Date;
}
