using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class VendaDto
    {
        public int VendaId { get; set; }
        public int? TutorId { get; set; }
        public int? UsuarioId { get; set; } // ? Adicionado para rastrear o usuário comprador
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
        // ?? TutorId agora é opcional - permite compras sem ser tutor
        public int? TutorId { get; set; }
        
        // ? Adicionado para vincular a venda a um usuário, mesmo que ele não seja um tutor
        public int? UsuarioId { get; set; }

        public int? FuncionarioId { get; set; }

        [Required(ErrorMessage = "A forma de pagamento é obrigatória.")]
        [StringLength(50, ErrorMessage = "Forma de pagamento deve ter no máximo 50 caracteres.")]
        public string? FormaPagamento { get; set; }

        [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres.")]
        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "A lista de itens não pode estar vazia.")]
        [MinLength(1, ErrorMessage = "A venda deve ter pelo menos um item.")]
        public List<CreateItemVendaDto> Itens { get; set; } = new List<CreateItemVendaDto>();

        // ?? NOVOS CAMPOS: Para criar tutor automaticamente durante a compra
        [StringLength(120, ErrorMessage = "Nome do cliente deve ter no máximo 120 caracteres.")]
        public string? NomeCliente { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter formato válido.")]
        [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres.")]
        public string? EmailCliente { get; set; }

        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}-?\d{4}$|^\d{10,11}$", ErrorMessage = "Telefone inválido. Use formato (XX) XXXXX-XXXX")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres.")]
        public string? TelefoneCliente { get; set; }

        [StringLength(250, ErrorMessage = "Endereço deve ter no máximo 250 caracteres.")]
        public string? EnderecoCliente { get; set; }
    }

    public class CreateItemVendaDto
    {
        // Pelo menos um dos dois (Produto ou Serviço) deve ser fornecido
        public int? ProdutoId { get; set; }
        public int? ServicoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, 100, ErrorMessage = "A quantidade deve ser entre 1 e 100.")]
        public int Quantidade { get; set; }
    }
}