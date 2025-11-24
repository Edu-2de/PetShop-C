using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class ItemVenda
    {
        public int ItemVendaId { get; set; }

        [Required(ErrorMessage = "VendaId é obrigatório")]
        public int VendaId { get; set; }

        public int? ProdutoId { get; set; }

        public int? ServicoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int Quantidade { get; set; } = 1;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999.99, ErrorMessage = "Preço unitário deve estar entre 0 e 99999,99")]
        public decimal PrecoUnitario { get; set; }

        // Navigation Properties
        [ForeignKey("VendaId")]
        public virtual Venda Venda { get; set; } = null!;

        [ForeignKey("ProdutoId")]
        public virtual Produto? Produto { get; set; }

        [ForeignKey("ServicoId")]
        public virtual Servico? Servico { get; set; }
    }
}