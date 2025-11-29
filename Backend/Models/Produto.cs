using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        public int QuantidadeEstoque { get; set; } // Este é o campo oficial de estoque

        // Removemos "Quantidade" duplicado se existir, mantendo apenas QuantidadeEstoque ou vice-versa
        // Vou remover o antigo "int Quantidade" para limpar, use QuantidadeEstoque no DTO.

        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        public bool Ativo { get; set; } = true;

        [StringLength(80)]
        public string? CodigoBarras { get; set; }

        // Relacionamentos
        public int? FornecedorId { get; set; }
        [ForeignKey("FornecedorId")]
        public virtual Fornecedor? Fornecedor { get; set; }

        public int? CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        public virtual ICollection<ItemVenda> ItemVendas { get; set; } = new List<ItemVenda>();
        public virtual ICollection<ProdutoImagem> Imagens { get; set; } = new List<ProdutoImagem>();
    }
}