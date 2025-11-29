using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class CategoriaDto
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class CreateCategoriaDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }
    }
}