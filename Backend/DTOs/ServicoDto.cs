using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class ServicoDto
    {
        public int ServicoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// NOVO: Lista de cargos que podem realizar este serviço
        /// </summary>
        public List<string> CargosResponsaveis { get; set; } = new List<string>();

        /// <summary>
        /// NOVO: String com cargos separados por vírgula (para exibição)
        /// </summary>
        public string? CargosResponsaveisTexto { get; set; }

        // MANTIDO PARA COMPATIBILIDADE - Lista de funcionários aptos baseada nos cargos
        public List<FuncionarioSimplificadoDto> FuncionariosAptos { get; set; } = new List<FuncionarioSimplificadoDto>();
        
        // Para compatibilidade (será removido futuramente)
        public int? FuncionarioResponsavelId { get; set; }
        public string? FuncionarioResponsavelNome { get; set; }
    }

    public class CreateServicoDto
    {
        /// <summary>
        /// Nome do serviço
        /// </summary>
        /// <example>Consulta Veterinária Geral</example>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do serviço
        /// </summary>
        /// <example>Consulta clínica geral com exame físico completo do animal</example>
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Preço do serviço
        /// </summary>
        /// <example>80.00</example>
        [Range(0, 9999.99, ErrorMessage = "Preço deve estar entre 0 e 9999,99")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Duração em minutos
        /// </summary>
        /// <example>60</example>
        [Range(1, 480, ErrorMessage = "Duração deve estar entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; }

        /// <summary>
        /// Se o serviço está ativo
        /// </summary>
        /// <example>true</example>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// NOVO: Lista de cargos que podem realizar este serviço
        /// </summary>
        /// <example>["Veterinário", "Atendente"]</example>
        public List<string> CargosResponsaveis { get; set; } = new List<string>();

        /// <summary>
        /// MANTIDO PARA COMPATIBILIDADE: Lista de IDs dos funcionários aptos para este serviço
        /// </summary>
        /// <example>[1, 2]</example>
        public List<int> FuncionariosAptosIds { get; set; } = new List<int>();
    }

    public class UpdateServicoDto
    {
        /// <summary>
        /// Nome do serviço
        /// </summary>
        /// <example>Consulta Veterinária Especializada</example>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do serviço
        /// </summary>
        /// <example>Consulta especializada com exames complementares</example>
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Preço do serviço
        /// </summary>
        /// <example>120.00</example>
        [Range(0, 9999.99, ErrorMessage = "Preço deve estar entre 0 e 9999,99")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Duração em minutos
        /// </summary>
        /// <example>90</example>
        [Range(1, 480, ErrorMessage = "Duração deve estar entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; }

        /// <summary>
        /// Se o serviço está ativo
        /// </summary>
        /// <example>true</example>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// NOVO: Lista de cargos que podem realizar este serviço
        /// </summary>
        /// <example>["Veterinário", "Tosador"]</example>
        public List<string> CargosResponsaveis { get; set; } = new List<string>();

        /// <summary>
        /// MANTIDO PARA COMPATIBILIDADE: Lista de IDs dos funcionários aptos para este serviço
        /// </summary>
        /// <example>[1, 3]</example>
        public List<int> FuncionariosAptosIds { get; set; } = new List<int>();
    }
}
