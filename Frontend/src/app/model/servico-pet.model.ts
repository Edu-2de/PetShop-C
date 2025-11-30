export interface ServicoPet {
  servicoId: number;
  nome: string;
  descricao: string;
  preco: number;
  ativo: boolean;

  // Correção: Usar o nome exato do DTO do backend
  duracaoMinutos: number;

  // Alias para compatibilidade (opcional, mas duracaoMinutos é o principal agora)
  id?: number;
}
