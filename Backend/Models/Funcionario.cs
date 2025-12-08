using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cargo é obrigatório")]
        [StringLength(50, ErrorMessage = "Cargo deve ter no máximo 50 caracteres")]
        public string Cargo { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefone { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataContratacao { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        // Relacionamento com Usuario
        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        // Navigation Properties
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<Servico> ServicosResponsavel { get; set; } = new List<Servico>();
        
        // NOVO: Relacionamento muitos-para-muitos com serviços
        public virtual ICollection<ServicoFuncionario> ServicoFuncionarios { get; set; } = new List<ServicoFuncionario>();
    }
}