using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(80, ErrorMessage = "Cargo deve ter no máximo 80 caracteres")]
        public string? Cargo { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(60, ErrorMessage = "Login deve ter no máximo 60 caracteres")]
        public string? Login { get; set; }

        // Em produção, use hash + salt para senhas
        [StringLength(255, ErrorMessage = "Password deve ter no máximo 255 caracteres")]
        public string? PasswordHash { get; set; }

        public DateTime DataContratacao { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<RegistroProntuario> Registros { get; set; } = new List<RegistroProntuario>();
        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}