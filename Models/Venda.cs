using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Venda
    {
        public int VendaId { get; set; }

        public int? TutorId { get; set; }

        public int? FuncionarioId { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999.99, ErrorMessage = "Valor total deve estar entre 0 e 99999,99")]
        public decimal ValorTotal { get; set; }

        [StringLength(50, ErrorMessage = "Forma de pagamento deve ter no máximo 50 caracteres")]
        public string? FormaPagamento { get; set; }

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }

        // Navigation Properties
        [ForeignKey("TutorId")]
        public virtual Tutor? Tutor { get; set; }

        [ForeignKey("FuncionarioId")]
        public virtual Funcionario? Funcionario { get; set; }
        public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}