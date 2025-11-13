using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class AnimalDto
    {
        public int AnimalId { get; set; }

        [Required(ErrorMessage = "TutorId é obrigatório")]
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Espécie deve ter no máximo 50 caracteres")]
        public string? Especie { get; set; }

        [StringLength(100, ErrorMessage = "Raça deve ter no máximo 100 caracteres")]
        public string? Raca { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }

        [StringLength(20, ErrorMessage = "Sexo deve ter no máximo 20 caracteres")]
        public string? Sexo { get; set; }

        [StringLength(100, ErrorMessage = "Pelagem deve ter no máximo 100 caracteres")]
        public string? Pelagem { get; set; }
    }

    public class CreateAnimalDto
    {
        [Required(ErrorMessage = "TutorId é obrigatório")]
        public int TutorId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Espécie deve ter no máximo 50 caracteres")]
        public string? Especie { get; set; }

        [StringLength(100, ErrorMessage = "Raça deve ter no máximo 100 caracteres")]
        public string? Raca { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }

        [StringLength(20, ErrorMessage = "Sexo deve ter no máximo 20 caracteres")]
        public string? Sexo { get; set; }

        [StringLength(100, ErrorMessage = "Pelagem deve ter no máximo 100 caracteres")]
        public string? Pelagem { get; set; }
    }

    public class UpdateAnimalDto
    {
        // Campos opcionais para permitir atualizações parciais via PUT (seguindo o padrão anterior)
        public int? TutorId { get; set; }

        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string? Nome { get; set; }

        [StringLength(50, ErrorMessage = "Espécie deve ter no máximo 50 caracteres")]
        public string? Especie { get; set; }

        [StringLength(100, ErrorMessage = "Raça deve ter no máximo 100 caracteres")]
        public string? Raca { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }

        [StringLength(20, ErrorMessage = "Sexo deve ter no máximo 20 caracteres")]
        public string? Sexo { get; set; }

        [StringLength(100, ErrorMessage = "Pelagem deve ter no máximo 100 caracteres")]
        public string? Pelagem { get; set; }
    }
}