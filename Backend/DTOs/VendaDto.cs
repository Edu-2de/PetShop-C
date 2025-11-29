using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class VendaDto
    {
        public int VendaId { get; set; }
        public int? TutorId { get; set; }
        public int? FuncionarioId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public string? FormaPagamento { get; set; }
        public string? Observacoes { get; set; }

        // Lista de itens da venda
        public List<ItemVendaDto> Itens { get; set; } = new List<ItemVendaDto>();
    }

    public class ItemVendaDto
    {
        public int ItemVendaId { get; set; }
        public int? ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public int? ServicoId { get; set; }
        public string? ServicoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

    public class CreateVendaDto
    {
        public int? TutorId { get; set; }
        public int? FuncionarioId { get; set; }

        [StringLength(50)]
        public string? FormaPagamento { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        // Na criação, recebemos uma lista simplificada de itens
        public List<CreateItemVendaDto> Itens { get; set; } = new List<CreateItemVendaDto>();
    }

    public class CreateItemVendaDto
    {
        public int? ProdutoId { get; set; }
        public int? ServicoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser pelo menos 1")]
        public int Quantidade { get; set; }
    }
}