using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class RegistroProntuario
    {
        public int RegistroProntuarioId { get; set; }

        [Required(ErrorMessage = "AnimalId é obrigatório")]
        public int AnimalId { get; set; }

        public DateTime DataAtendimento { get; set; } = DateTime.UtcNow;

        [StringLength(80, ErrorMessage = "Tipo de atendimento deve ter no máximo 80 caracteres")]
        public string? TipoAtendimento { get; set; }

        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }

        public int? FuncionarioId { get; set; }

        [StringLength(500, ErrorMessage = "Prescrições deve ter no máximo 500 caracteres")]
        public string? Prescricoes { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 999.99, ErrorMessage = "Peso deve estar entre 0 e 999,99 kg")]
        public decimal? Peso { get; set; }

        // Navigation Properties
        [ForeignKey("AnimalId")]
        public virtual Animal Animal { get; set; } = null!;

        [ForeignKey("FuncionarioId")]
        public virtual Funcionario? Funcionario { get; set; }
    }
}