using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.Models
{
    /// <summary>
    /// Relacionamento muitos-para-muitos entre Serviços e Funcionários
    /// Permite que um serviço tenha múltiplos funcionários aptos
    /// </summary>
    public class ServicoFuncionario
    {
        public int ServicoId { get; set; }
        public int FuncionarioId { get; set; }

        // Navigation Properties
        [ForeignKey("ServicoId")]
        public virtual Servico Servico { get; set; } = null!;

        [ForeignKey("FuncionarioId")]
        public virtual Funcionario Funcionario { get; set; } = null!;
    }
}