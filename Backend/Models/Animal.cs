using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }

        [Required(ErrorMessage = "TutorId é obrigatório")]
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Espécie deve ter no máximo 50 caracteres")]
        public string? Especie { get; set; }

        [StringLength(100, ErrorMessage = "Raça deve ter no máximo 100 caracteres")]
        public string? Raca { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }

        [StringLength(20, ErrorMessage = "Sexo deve ter no máximo 20 caracteres")]
        public string? Sexo { get; set; }

        [StringLength(100, ErrorMessage = "Pelagem deve ter no máximo 100 caracteres")]
        public string? Pelagem { get; set; }

        // Navigation Properties
        [ForeignKey("TutorId")]
        public virtual Tutor Tutor { get; set; } = null!;
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<RegistroProntuario> Registros { get; set; } = new List<RegistroProntuario>();
    }
}