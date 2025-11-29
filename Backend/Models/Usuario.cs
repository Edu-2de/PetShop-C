using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TipoUsuario { get; set; } = "Tutor"; // "Admin", "Funcionario", "Tutor"

        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public virtual Tutor? Tutor { get; set; }
        public virtual Funcionario? Funcionario { get; set; }
    }
}