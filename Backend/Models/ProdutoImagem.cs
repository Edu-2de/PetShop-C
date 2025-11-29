using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class ProdutoImagem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Url { get; set; } = string.Empty;

        public int ProdutoId { get; set; }
        public virtual Produto? Produto { get; set; }
    }
}
