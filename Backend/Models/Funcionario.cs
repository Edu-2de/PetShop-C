using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        // NOVO: Link com Usuario
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string? Cargo { get; set; }

        public DateTime DataContratacao { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<RegistroProntuario> Registros { get; set; } = new List<RegistroProntuario>();
        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}