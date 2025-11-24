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
        public bool Ativo { get; set; }
    }

    public class CreateServicoDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [Range(0, 9999.99, ErrorMessage = "Preço deve estar entre 0 e 9999,99")]
        public decimal Preco { get; set; }

        [Range(1, 480, ErrorMessage = "Duração deve estar entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; } = 30;

        public bool Ativo { get; set; } = true;
    }

    public class UpdateServicoDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(120, ErrorMessage = "Nome deve ter no máximo 120 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [Range(0, 9999.99, ErrorMessage = "Preço deve estar entre 0 e 9999,99")]
        public decimal Preco { get; set; }

        [Range(1, 480, ErrorMessage = "Duração deve estar entre 1 e 480 minutos")]
        public int DuracaoMinutos { get; set; }

        public bool Ativo { get; set; }
    }
}
