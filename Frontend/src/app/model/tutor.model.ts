export interface Tutor {
  tutorId: number;
  id?: number; // Alias para tutorId para compatibilidade
  nome: string;
  email?: string;
  telefone?: string;
  endereco?: string;
  dataCadastro?: Date;
  usuarioId?: number;
}
