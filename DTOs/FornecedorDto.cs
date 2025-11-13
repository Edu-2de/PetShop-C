using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class FornecedorDto
    {
        public int FornecedorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Contato deve ter no máximo 150 caracteres")]
        public string? Contato { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [StringLength(300, ErrorMessage = "Endereço deve ter no máximo 300 caracteres")]
        public string? Endereco { get; set; }
    }

    public class CreateFornecedorDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Contato deve ter no máximo 150 caracteres")]
        public string? Contato { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [StringLength(300, ErrorMessage = "Endereço deve ter no máximo 300 caracteres")]
        public string? Endereco { get; set; }
    }

    public class UpdateFornecedorDto
    {
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string? Nome { get; set; }

        [StringLength(150, ErrorMessage = "Contato deve ter no máximo 150 caracteres")]
        public string? Contato { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [StringLength(300, ErrorMessage = "Endereço deve ter no máximo 300 caracteres")]
        public string? Endereco { get; set; }
    }
}