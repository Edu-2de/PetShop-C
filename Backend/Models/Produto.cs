using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a 0")]
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999.99, ErrorMessage = "Preço deve estar entre 0 e 99999,99")]
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public int? FornecedorId { get; set; }
        
        [StringLength(80, ErrorMessage = "Código de barras deve ter no máximo 80 caracteres")]
        public string? CodigoBarras { get; set; }
        
        public bool Ativo { get; set; } = true;

        // Navigation Properties
        [ForeignKey("FornecedorId")]
        public virtual Fornecedor? Fornecedor { get; set; }
        public virtual ICollection<ItemVenda> ItemVendas { get; set; } = new List<ItemVenda>();
        public virtual ICollection<ProdutoImagem> Imagens { get; set; } = new List<ProdutoImagem>();
    }
}