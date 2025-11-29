using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Descrição deve ter no máximo 255 caracteres")]
        public string? Descricao { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}