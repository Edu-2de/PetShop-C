export interface Pet {
  animalId: number;
  nome: string;
  especie: string;
  raca: string;
  dataNascimento: Date;
  sexo: string;
  pelagem: string;
  observacoes?: string;
  tutorId: number;
  tutorNome?: string;
  
  // Aliases para compatibilidade com componentes antigos
  id?: number;
  nascimento?: Date;
}
