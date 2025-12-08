using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class AgendamentoDto
    {
        public int AgendamentoId { get; set; }
        public int AnimalId { get; set; }
        public int? FuncionarioId { get; set; }
        public int ServicoId { get; set; }
        public DateTime DataHora { get; set; }

        // Mantemos string para facilitar o JSON, o AutoMapper converte o Enum para String aqui
        public string Status { get; set; } = "Pendente";

        public string? Observacoes { get; set; }

        // Informações relacionadas para exibição
        public string? AnimalNome { get; set; }
        public string? ServicoNome { get; set; }
        public string? FuncionarioNome { get; set; }
    }

    public class CreateAgendamentoDto
    {
        [Required(ErrorMessage = "O ID do animal é obrigatório.")]
        public int AnimalId { get; set; }

        // O funcionário pode ser opcional, dependendo da regra de negócio
        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "O ID do serviço é obrigatório.")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        // Adicionar uma validação customizada se necessário para horários futuros
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [RegularExpression("^(Pendente|Confirmado|EmAndamento|Concluido|Cancelado)$", ErrorMessage = "Status inválido.")]
        public string Status { get; set; } = "Pendente";

        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres.")]
        public string? Observacoes { get; set; }
    }

    public class UpdateAgendamentoDto
    {
        [Required(ErrorMessage = "O ID do animal é obrigatório.")]
        public int AnimalId { get; set; }

        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "O ID do serviço é obrigatório.")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [RegularExpression("^(Pendente|Confirmado|EmAndamento|Concluido|Cancelado)$", ErrorMessage = "Status inválido.")]
        public string Status { get; set; } = "Pendente";

        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres.")]
        public string? Observacoes { get; set; }
    }

    /// <summary>
    /// DTO para criar agendamento completo com tutor e animal automaticamente
    /// </summary>
    public class CreateAgendamentoCompletoDto
    {
        // Dados do agendamento
        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "O ID do serviço é obrigatório.")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias.")]
        public DateTime DataHora { get; set; }

        public string Status { get; set; } = "Pendente";

        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres.")]
        public string? Observacoes { get; set; }

        // Dados do tutor (se não existir, será criado)
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Nome do tutor deve ter entre 3 e 120 caracteres")]
        public string? NomeTutor { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? EmailTutor { get; set; }

        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use formato (XX) XXXXX-XXXX")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? TelefoneTutor { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? EnderecoTutor { get; set; }

        // Dados do animal (se não existir, será criado)
        [Required(ErrorMessage = "Nome do animal é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome do animal deve ter entre 2 e 100 caracteres")]
        public string NomeAnimal { get; set; } = string.Empty;

        [Required(ErrorMessage = "Espécie é obrigatória")]
        [RegularExpression("^(Cão|Gato|Pássaro|Outros)$", ErrorMessage = "Espécie deve ser: Cão, Gato, Pássaro ou Outros")]
        public string EspecieAnimal { get; set; } = "Cão";

        [StringLength(50, ErrorMessage = "Raça deve ter no máximo 50 caracteres")]
        public string? RacaAnimal { get; set; }

        [Required(ErrorMessage = "Sexo é obrigatório")]
        [RegularExpression("^(Macho|Fêmea)$", ErrorMessage = "Sexo deve ser: Macho ou Fêmea")]
        public string SexoAnimal { get; set; } = "Macho";

        public DateTime? DataNascimentoAnimal { get; set; }

        [RegularExpression("^(Curta|Média|Longa)$", ErrorMessage = "Pelagem deve ser: Curta, Média ou Longa")]
        public string PelagemAnimal { get; set; } = "Curta";

        [StringLength(500, ErrorMessage = "Observações do animal devem ter no máximo 500 caracteres")]
        public string? ObservacoesAnimal { get; set; }
    }
}