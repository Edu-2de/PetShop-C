using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    /// <summary>
    /// Representa um serviço oferecido pela clínica (consulta, banho, tosa, etc.)
    /// </summary>
    public class Servico
    {
        /// <summary>
        /// Identificador único do serviço
        /// </summary>
        public int ServicoId { get; set; }

        /// <summary>
        /// Nome do serviço
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do serviço
        /// </summary>
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Preço do serviço
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 9999.99, ErrorMessage = "Preço deve estar entre 0 e 9999,99")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Duração estimada em minutos
        /// </summary>
        [Range(1, 480, ErrorMessage = "Duração deve estar entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; }

        /// <summary>
        /// Se o serviço está ativo
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// NOVO: Cargos que podem realizar este serviço (separados por vírgula)
        /// Ex: "Veterinário,Atendente" ou "Tosador"
        /// </summary>
        [StringLength(500)]
        public string? CargosResponsaveis { get; set; }

        // Relacionamentos
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
        public virtual ICollection<ItemVenda> ItemVendas { get; set; } = new List<ItemVenda>();

        // MANTIDO PARA COMPATIBILIDADE (será removido posteriormente)
        public int? FuncionarioResponsavelId { get; set; }
        public virtual Funcionario? FuncionarioResponsavel { get; set; }
        public virtual ICollection<ServicoFuncionario> ServicoFuncionarios { get; set; } = new List<ServicoFuncionario>();
    }
}