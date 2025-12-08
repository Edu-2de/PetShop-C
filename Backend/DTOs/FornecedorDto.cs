using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class FornecedorDto
    {
        public int FornecedorId { get; set; }

        [Required(ErrorMessage = "Nome do fornecedor é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$|^\d{14}$", ErrorMessage = "CNPJ inválido")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use formato (XX) XXXXX-XXXX")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Endereço não pode ter mais de 250 caracteres")]
        public string? Endereco { get; set; }
    }

    public class CreateFornecedorDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [RegularExpression(@"^\d{14}$|^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Use 14 dígitos ou o formato XX.XXX.XXX/XXXX-XX.")]
        [StringLength(18, ErrorMessage = "CNPJ deve ter no máximo 18 caracteres")]
        public string? Cnpj { get; set; }

        [StringLength(150, ErrorMessage = "Razão Social deve ter no máximo 150 caracteres")]
        public string? RazaoSocial { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use o formato (XX) XXXXX-XXXX ou apenas dígitos.")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(300, ErrorMessage = "Endereço deve ter no máximo 300 caracteres")]
        public string? Endereco { get; set; }
    }

    public class UpdateFornecedorDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [RegularExpression(@"^\d{14}$|^\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Use 14 dígitos ou o formato XX.XXX.XXX/XXXX-XX.")]
        [StringLength(18, ErrorMessage = "CNPJ deve ter no máximo 18 caracteres")]
        public string? Cnpj { get; set; }

        [StringLength(150, ErrorMessage = "Razão Social deve ter no máximo 150 caracteres")]
        public string? RazaoSocial { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use o formato (XX) XXXXX-XXXX ou apenas dígitos.")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(300, ErrorMessage = "Endereço deve ter no máximo 300 caracteres")]
        public string? Endereco { get; set; }
    }
}