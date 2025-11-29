// Adicione este método no VendaController
[HttpPost]
public async Task<ActionResult<VendaDto>> CreateVenda([FromBody] CreateVendaDto createVendaDto)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var venda = _mapper.Map<Venda>(createVendaDto);
        venda.DataVenda = DateTime.Now;

        decimal totalCalculado = 0;

        foreach (var item in venda.Itens)
        {
            // Lógica para Produtos
            if (item.ProdutoId.HasValue)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto == null) return BadRequest($"Produto {item.ProdutoId} não encontrado.");

                if (produto.Quantidade < item.Quantidade)
                    return BadRequest($"Estoque insuficiente para o produto: {produto.Nome}");

                // BAIXA NO ESTOQUE
                produto.Quantidade -= item.Quantidade;
                item.PrecoUnitario = produto.Preco; // Garante preço atual
                totalCalculado += (produto.Preco * item.Quantidade);
            }
            // Lógica para Serviços
            else if (item.ServicoId.HasValue)
            {
                var servico = await _context.Servicos.FindAsync(item.ServicoId);
                if (servico == null) return BadRequest($"Serviço {item.ServicoId} não encontrado.");

                item.PrecoUnitario = servico.Preco;
                totalCalculado += (servico.Preco * item.Quantidade);
            }
        }

        venda.ValorTotal = totalCalculado; // Garante que o total bata com os itens

        _context.Vendas.Add(venda);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        var vendaDto = _mapper.Map<VendaDto>(venda);
        return CreatedAtAction(nameof(GetVenda), new { id = venda.VendaId }, vendaDto);
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        return StatusCode(500, $"Erro ao processar venda: {ex.Message}");
    }
}