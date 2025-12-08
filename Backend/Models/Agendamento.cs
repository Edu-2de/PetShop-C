using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Agendamento
    {
        public int AgendamentoId { get; set; }

        [Required(ErrorMessage = "AnimalId é obrigatório")]
        public int AnimalId { get; set; }

        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "ServicoId é obrigatório")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "DataHora é obrigatória")]
        public DateTime DataHora { get; set; }

        // TEMPORÁRIO: Mantendo como string para evitar problemas de migração
        [StringLength(20)]
        public string Status { get; set; } = "Pendente";

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }

        // Navigation Properties
        [ForeignKey("AnimalId")]
        public virtual Animal Animal { get; set; } = null!;

        [ForeignKey("FuncionarioId")]
        public virtual Funcionario? Funcionario { get; set; }

        [ForeignKey("ServicoId")]
        public virtual Servico Servico { get; set; } = null!;
    }
}