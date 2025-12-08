using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Tutor
    {
        [Key]
        public int TutorId { get; set; }

        [Required]
        [StringLength(120)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(250)]
        public string? Endereco { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        // Relacionamento com Usuario (pode ser null - tutor sem login)
        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Relacionamento com Animais (um tutor pode ter muitos animais)
        public ICollection<Animal> Animais { get; set; } = new List<Animal>();

        // Relacionamento com Vendas (um tutor pode ter muitas vendas)
        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}