// ... imports
namespace SIGA_PET.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }

        // NOVO: Chave estrangeira para Usuario (Login)
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [Required]
        [StringLength(120)]
        public string Nome { get; set; } = string.Empty;

        // Email e Telefone podem continuar aqui para contato fácil, 
        // ou você pode remover Email daqui e usar só o do Usuario.
        // Vou manter para facilitar a migração.
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }

        public virtual ICollection<Animal> Animais { get; set; } = new List<Animal>();
        // ... outras props
    }
}