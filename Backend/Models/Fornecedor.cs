using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class Fornecedor
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

        [StringLength(150)]
        public string? Cnpj { get; set; }

        [StringLength(150)]
        public string? RazaoSocial { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(300, ErrorMessage = "Endereço deve ter no máximo 300 caracteres")]
        public string? Endereco { get; set; }

        // Navigation Properties
        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}