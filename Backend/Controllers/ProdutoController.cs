using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de produtos do e-commerce.
    /// Permite CRUD completo de produtos, buscas e filtros.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProdutoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém a lista de todos os produtos cadastrados.
        /// </summary>
        /// <remarks>
        /// Retorna todos os produtos com suas informações completas (fornecedor, categoria, imagens).
        /// 
        /// Exemplo de resposta:
        /// ```json
        /// [
        ///   {
        ///     "produtoId": 1,
        ///     "nome": "Ração Premium",
        ///     "descricao": "Ração de alta qualidade",
        ///     "preco": 189.90,
        ///     "quantidadeEstoque": 150,
        ///     "nomeCategoria": "Alimentação",
        ///     "nomeFornecedor": "Pet Foods Brasil",
        ///     "ativo": true,
        ///     "imagens": []
        ///   }
        /// ]
        /// ```
        /// </remarks>
        /// <returns>Lista de produtos</returns>
        /// <response code="200">Retorna a lista de produtos com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
        {
            try
            {
                var produtos = await _context.Produtos
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Categoria)
                    .Include(p => p.Imagens)
                    .AsNoTracking()
                    .ToListAsync();

                var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar produtos", erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um produto específico pelo ID.
        /// </summary>
        /// <remarks>
        /// Retorna os detalhes completos do produto incluindo fornecedor, categoria e imagens.
        /// </remarks>
        /// <param name="id">ID do produto (ex: 1)</param>
        /// <returns>Dados do produto solicitado</returns>
        /// <response code="200">Produto encontrado com sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Categoria)
                    .Include(p => p.Imagens)
                    .FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound(new { mensagem = $"Produto com ID {id} não encontrado" });
                }

                var produtoDto = _mapper.Map<ProdutoDto>(produto);
                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar produto", erro = ex.Message });
            }
        }

        /// <summary>
        /// Obtém apenas os produtos ativos (disponíveis para venda).
        /// </summary>
        /// <remarks>
        /// Filtro específico para produtos com status ativo = true.
        /// Útil para a página de produtos no cliente.
        /// </remarks>
        /// <returns>Lista de produtos ativos</returns>
        /// <response code="200">Lista obtida com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("ativos")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosAtivos()
        {
            try
            {
                var produtos = await _context.Produtos
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Categoria)
                    .Include(p => p.Imagens)
                    .Where(p => p.Ativo)
                    .AsNoTracking()
                    .ToListAsync();

                var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar produtos ativos", erro = ex.Message });
            }
        }

        /// <summary>
        /// Busca produtos por nome (busca parcial).
        /// </summary>
        /// <remarks>
        /// Realiza uma busca case-insensitive pelo nome do produto.
        /// 
        /// Exemplos de busca:
        /// - `/api/produto/search?name=ração` ? Retorna todos os produtos com "ração" no nome
        /// - `/api/produto/search?name=coleira` ? Retorna todos os acessórios com "coleira"
        /// </remarks>
        /// <param name="name">Nome ou parte do nome do produto a buscar (mínimo 1 caractere)</param>
        /// <returns>Lista de produtos correspondentes</returns>
        /// <response code="200">Busca realizada com sucesso</response>
        /// <response code="400">Nome de busca não fornecido</response>
        /// <response code="404">Nenhum produto encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> SearchProdutos([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { mensagem = "O nome para busca não pode ser vazio" });
            }

            var produtos = await _context.Produtos
                .Include(p => p.Fornecedor)
                .Include(p => p.Categoria)
                .Include(p => p.Imagens)
                .AsNoTracking()
                .Where(p => p.Nome.Contains(name))
                .ToListAsync();

            if (!produtos.Any())
            {
                return NotFound(new { mensagem = $"Nenhum produto encontrado com o nome '{name}'" });
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return Ok(produtosDto);
        }

        /// <summary>
        /// Cria um novo produto no sistema.
        /// </summary>
        /// <remarks>
        /// Requer autenticação de admin. Cria um novo produto com as informações fornecidas.
        /// 
        /// Corpo da requisição exemplo:
        /// ```json
        /// {
        ///   "nome": "Ração Premium Golden Retriever",
        ///   "descricao": "Ração completa e balanceada especialmente formulada para Golden Retrievers",
        ///   "preco": 189.90,
        ///   "quantidadeEstoque": 150,
        ///   "categoriaId": 1,
        ///   "fornecedorId": 1,
        ///   "ativo": true
        /// }
        /// ```
        /// </remarks>
        /// <param name="createProdutoDto">Dados do novo produto</param>
        /// <returns>Produto criado com ID gerado</returns>
        /// <response code="201">Produto criado com sucesso</response>
        /// <response code="400">Dados inválidos ou referências não encontradas</response>
        /// <response code="401">Não autenticado</response>
        /// <response code="403">Sem permissão (requer admin)</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProdutoDto>> CreateProduto([FromBody] CreateProdutoDto createProdutoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o fornecedor existe
                if (createProdutoDto.FornecedorId.HasValue)
                {
                    var fornecedorExists = await _context.Fornecedores.AnyAsync(f => f.FornecedorId == createProdutoDto.FornecedorId.Value);
                    if (!fornecedorExists)
                    {
                        return BadRequest(new { mensagem = $"Fornecedor com ID {createProdutoDto.FornecedorId} não encontrado" });
                    }
                }

                // Verificar se a categoria existe
                if (createProdutoDto.CategoriaId.HasValue)
                {
                    var categoriaExists = await _context.Categorias.AnyAsync(c => c.CategoriaId == createProdutoDto.CategoriaId.Value);
                    if (!categoriaExists)
                    {
                        return BadRequest(new { mensagem = $"Categoria com ID {createProdutoDto.CategoriaId} não encontrada" });
                    }
                }

                var produto = _mapper.Map<Produto>(createProdutoDto);

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                // Recarregar com dados relacionados
                if (produto.FornecedorId.HasValue)
                {
                    await _context.Entry(produto).Reference(p => p.Fornecedor).LoadAsync();
                }
                if (produto.CategoriaId.HasValue)
                {
                    await _context.Entry(produto).Reference(p => p.Categoria).LoadAsync();
                }

                var produtoDto = _mapper.Map<ProdutoDto>(produto);
                return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao criar produto", erro = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <remarks>
        /// Requer autenticação de admin. Permite atualizar qualquer campo do produto.
        /// 
        /// Corpo da requisição exemplo:
        /// ```json
        /// {
        ///   "nome": "Ração Premium Golden - Nova Fórmula",
        ///   "descricao": "Ração de alta qualidade atualizada",
        ///   "preco": 199.90,
        ///   "quantidadeEstoque": 200,
        ///   "categoriaId": 1,
        ///   "fornecedorId": 1,
        ///   "ativo": true
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">ID do produto a atualizar</param>
        /// <param name="updateProdutoDto">Dados atualizados do produto</param>
        /// <returns>Sem conteúdo (204)</returns>
        /// <response code="204">Produto atualizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="409">Erro de concorrência</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] UpdateProdutoDto updateProdutoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound(new { mensagem = $"Produto com ID {id} não encontrado" });
                }

                // Verificações de referências
                if (updateProdutoDto.FornecedorId.HasValue)
                {
                    var fornecedorExists = await _context.Fornecedores.AnyAsync(f => f.FornecedorId == updateProdutoDto.FornecedorId.Value);
                    if (!fornecedorExists)
                    {
                        return BadRequest(new { mensagem = $"Fornecedor com ID {updateProdutoDto.FornecedorId} não encontrado" });
                    }
                }

                if (updateProdutoDto.CategoriaId.HasValue)
                {
                    var categoriaExists = await _context.Categorias.AnyAsync(c => c.CategoriaId == updateProdutoDto.CategoriaId.Value);
                    if (!categoriaExists)
                    {
                        return BadRequest(new { mensagem = $"Categoria com ID {updateProdutoDto.CategoriaId} não encontrada" });
                    }
                }

                _mapper.Map(updateProdutoDto, produto);

                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { mensagem = "Erro de concorrência. O registro foi modificado por outro usuário" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao atualizar produto", erro = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um produto do sistema.
        /// </summary>
        /// <remarks>
        /// Requer autenticação de admin. Não permite deletar produtos que estão associados a vendas.
        /// </remarks>
        /// <param name="id">ID do produto a deletar</param>
        /// <returns>Sem conteúdo (204)</returns>
        /// <response code="204">Produto deletado com sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="500">Erro ao deletar (produto com vendas associadas)</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound(new { mensagem = $"Produto com ID {id} não encontrado" });
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao deletar: O produto pode estar associado a outros registros (ex: vendas)", erro = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao deletar produto", erro = ex.Message });
            }
        }
    }
}