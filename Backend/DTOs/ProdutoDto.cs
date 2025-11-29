using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGA_PET.DTOs
{
    public class ProdutoDto
    {
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999.99, ErrorMessage = "Preço deve estar entre 0 e 99999,99")]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a 0")]
        public int QuantidadeEstoque { get; set; }

        public bool Ativo { get; set; }
        public int? FornecedorId { get; set; }

        // Informações do fornecedor para exibição
        public string NomeFornecedor { get; set; } = string.Empty;

        // Coleção de imagens do produto
        public ICollection<ProdutoImagemDto> Imagens { get; set; } = new List<ProdutoImagemDto>();
    }


    public class CreateProdutoDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999.99, ErrorMessage = "Preço deve estar entre 0 e 99999,99")]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a 0")]
        public int QuantidadeEstoque { get; set; }

        public int? FornecedorId { get; set; }

        public bool Ativo { get; set; } = true;
    }

    public class UpdateProdutoDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999.99, ErrorMessage = "Preço deve estar entre 0 e 99999,99")]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a 0")]
        public int QuantidadeEstoque { get; set; }

        public int? FornecedorId { get; set; }

        public bool Ativo { get; set; }
    }

    public class ProdutoImagemDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
