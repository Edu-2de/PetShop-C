using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class TutorDto
    {
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use formato (XX) XXXXX-XXXX")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public string? Endereco { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }

    public class CreateTutorDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        // Campos novos necessários para criar o USUARIO vinculado
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150, ErrorMessage = "Email não pode ter mais de 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use o formato (XX) XXXXX-XXXX ou apenas dígitos.")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? Endereco { get; set; }
    }

    /// <summary>
    /// DTO para criar tutor simplificado (sem usuário/senha)
    /// Usado principalmente em agendamentos rápidos
    /// </summary>
    public class CreateTutorSimplificadoDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use o formato (XX) XXXXX-XXXX ou apenas dígitos.")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? Endereco { get; set; }
    }

    public class UpdateTutorDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use o formato (XX) XXXXX-XXXX ou apenas dígitos.")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? Endereco { get; set; }
    }
}