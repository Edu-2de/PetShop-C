export interface ServicoPet {
  servicoId: number;
  nome: string;
  descricao?: string;
  preco: number;
  duracaoMinutos: number;
  ativo: boolean;
  
  // NOVO: Sistema de cargos
  cargosResponsaveis: string[];
  cargosResponsaveisTexto?: string;
  
  // MANTIDO: Lista de funcionários aptos (agora baseada nos cargos)
  funcionariosAptos?: FuncionarioSimples[];
  
  // Para compatibilidade (será removido)
  funcionarioResponsavelId?: number;
  funcionarioResponsavelNome?: string;
}

export interface FuncionarioSimples {
  funcionarioId: number;
  nome: string;
  cargo: string;
}

export interface CreateServicoPet {
  nome: string;
  descricao?: string;
  preco: number;
  duracaoMinutos: number;
  ativo?: boolean;
  
  // NOVO: Sistema de cargos
  cargosResponsaveis: string[];
  
  // MANTIDO para compatibilidade
  funcionariosAptosIds?: number[];
}

export interface UpdateServicoPet {
  nome: string;
  descricao?: string;
  preco: number;
  duracaoMinutos: number;
  ativo?: boolean;
  
  // NOVO: Sistema de cargos
  cargosResponsaveis: string[];
  
  // MANTIDO para compatibilidade
  funcionariosAptosIds?: number[];
}
