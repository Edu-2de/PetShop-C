using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Cargo { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        public DateTime DataContratacao { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<RegistroProntuario> Registros { get; set; } = new List<RegistroProntuario>();
        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }

    public class CreateFuncionarioDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;

        public string? Cargo { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataContratacao { get; set; }
    }
}