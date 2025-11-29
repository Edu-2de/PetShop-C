using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class FuncionarioDto
    {
        public int FuncionarioId { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; } // Vem do Usuario
        public DateTime DataContratacao { get; set; }
    }

    public class CreateFuncionarioDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(80, ErrorMessage = "Cargo deve ter no máximo 80 caracteres")]
        public string? Cargo { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        public DateTime? DataContratacao { get; set; }
    }

    public class UpdateFuncionarioDto
    {
        [StringLength(120)]
        public string? Nome { get; set; }

        [StringLength(80)]
        public string? Cargo { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        public DateTime? DataContratacao { get; set; }
    }
}