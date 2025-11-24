export interface Fornecedor {
  fornecedorId: number;
  nome: string;
  cnpj: string;
  telefone: string;
  email: string;
  endereco: string;
  
  // Aliases para compatibilidade com componentes antigos
  id?: number;
  contato?: string;
}
