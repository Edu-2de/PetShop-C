export interface Tutor {
  tutorId: number;
  nome: string;
  telefone: string;
  email: string;
  endereco: string;
  dataCadastro?: Date;
  
  // Alias para compatibilidade com componentes antigos
  id?: number;
}
