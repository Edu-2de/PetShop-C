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
        public string Status { get; set; } = "Agendado";
        public string? Observacoes { get; set; }
        
        // Informações relacionadas para exibição
        public string? AnimalNome { get; set; }
        public string? ServicoNome { get; set; }
        public string? FuncionarioNome { get; set; }
    }

    public class CreateAgendamentoDto
    {
        [Required(ErrorMessage = "AnimalId é obrigatório")]
        public int AnimalId { get; set; }

        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "ServicoId é obrigatório")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "DataHora é obrigatória")]
        public DateTime DataHora { get; set; }

        [StringLength(50, ErrorMessage = "Status deve ter no máximo 50 caracteres")]
        public string Status { get; set; } = "Agendado";

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }

    public class UpdateAgendamentoDto
    {
        [Required(ErrorMessage = "AnimalId é obrigatório")]
        public int AnimalId { get; set; }

        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "ServicoId é obrigatório")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "DataHora é obrigatória")]
        public DateTime DataHora { get; set; }

        [StringLength(50, ErrorMessage = "Status deve ter no máximo 50 caracteres")]
        public string Status { get; set; } = "Agendado";

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }
}
