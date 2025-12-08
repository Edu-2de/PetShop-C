using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    /// <summary>
    /// Representa um usuário do sistema SIGA-PET
    /// </summary>
    /// <remarks>
    /// Um usuário pode ser um Tutor, Funcionário ou Administrador.
    /// Cada usuário possui credenciais de autenticação e pode ter relacionamentos
    /// com tutores, funcionários e vendas.
    /// </remarks>
    public class Usuario
    {
        public int UsuarioId { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

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
        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}