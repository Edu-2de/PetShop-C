using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class TutorDto
    {
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? Endereco { get; set; }

        public DateTime DataCadastro { get; set; }
    }

    public class CreateTutorDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        // Campos novos necessários para criar o USUARIO vinculado
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Senha { get; set; } = string.Empty;

        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
    }

    public class UpdateTutorDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? Endereco { get; set; }
    }
}