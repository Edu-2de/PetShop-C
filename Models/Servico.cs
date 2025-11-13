using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    public class Servico
    {
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 9999.99, ErrorMessage = "Preço deve estar entre 0 e 9999,99")]
        public decimal Preco { get; set; }

        [Range(1, 480, ErrorMessage = "Duração deve estar entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; } = 30;

        public bool Ativo { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<ItemVenda> ItemVendas { get; set; } = new List<ItemVenda>();
    }
}