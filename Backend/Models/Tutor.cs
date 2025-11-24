using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres")]
        public string? Endereco { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Animal> Animais { get; set; } = new List<Animal>();
        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}