using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class FuncionarioDto
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; } 
        public DateTime DataContratacao { get; set; }
        public bool Ativo { get; set; }
    }

    // DTO simplificado para dropdowns/seleção
    public class FuncionarioSimplificadoDto
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }

    public class CreateFuncionarioDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cargo é obrigatório")]
        [StringLength(50, ErrorMessage = "Cargo deve ter no máximo 50 caracteres")]
        public string Cargo { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(256, ErrorMessage = "Email deve ter no máximo 256 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; } = string.Empty;

        public DateTime? DataContratacao { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class UpdateFuncionarioDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cargo é obrigatório")]
        [StringLength(50, ErrorMessage = "Cargo deve ter no máximo 50 caracteres")]
        public string Cargo { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(256, ErrorMessage = "Email deve ter no máximo 256 caracteres")]
        public string? Email { get; set; }

        public bool Ativo { get; set; } = true;
    }
}