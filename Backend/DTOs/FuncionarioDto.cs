using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class FuncionarioDto
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

        // Em APIs públicas normalmente não se retorna o hash da senha.
        // Incluí apenas para compatibilidade com o model original, mas avalie não expor isso.
        [StringLength(255, ErrorMessage = "PasswordHash deve ter no máximo 255 caracteres")]
        public string? PasswordHash { get; set; }

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

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(60, ErrorMessage = "Login deve ter no máximo 60 caracteres")]
        public string? Login { get; set; }

        // Na criação você pode receber uma senha e gerar o hash antes de salvar.
        // Aqui mantenho PasswordHash por semelhança com o model; considerar receber uma senha plain e hashear.
        [StringLength(255, ErrorMessage = "PasswordHash deve ter no máximo 255 caracteres")]
        public string? PasswordHash { get; set; }

        // Opcional: permitir o cliente enviar DataContratacao ou deixar o server preencher
        public DateTime? DataContratacao { get; set; }
    }

    public class UpdateFuncionarioDto
    {
        // Campos opcionais para atualizações parciais (seguindo padrão usado anteriormente)
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string? Nome { get; set; }

        [StringLength(80, ErrorMessage = "Cargo deve ter no máximo 80 caracteres")]
        public string? Cargo { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(60, ErrorMessage = "Login deve ter no máximo 60 caracteres")]
        public string? Login { get; set; }

        [StringLength(255, ErrorMessage = "PasswordHash deve ter no máximo 255 caracteres")]
        public string? PasswordHash { get; set; }

        public DateTime? DataContratacao { get; set; }
    }
}