using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;

namespace SIGA_PET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProdutoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Produto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
        {
            try
            {
                var produtos = await _context.Produtos
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Imagens) // Incluir imagens
                    .AsNoTracking()
                    .ToListAsync();

                var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Produto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Imagens)
                    .FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var produtoDto = _mapper.Map<ProdutoDto>(produto);
                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Produto/ativos
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosAtivos()
        {
            try
            {
                var produtos = await _context.Produtos
                    .Include(p => p.Fornecedor)
                    .Include(p => p.Imagens)
                    .Where(p => p.Ativo)
                    .AsNoTracking()
                    .ToListAsync();

                var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Produto/search?name=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> SearchProdutos([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O nome para busca não pode ser vazio.");
            }

            var produtos = await _context.Produtos
                .Include(p => p.Fornecedor)
                .Include(p => p.Imagens)
                .AsNoTracking()
                .Where(p => p.Nome.Contains(name))
                .ToListAsync();

            if (!produtos.Any())
            {
                return NotFound("Nenhum produto encontrado com o nome fornecido.");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            return Ok(produtosDto);
        }

        // POST: api/Produto
        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> CreateProduto([FromBody] CreateProdutoDto createProdutoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o fornecedor existe (se fornecido)
                if (createProdutoDto.FornecedorId.HasValue)
                {
                    var fornecedorExists = await _context.Fornecedores.AnyAsync(f => f.FornecedorId == createProdutoDto.FornecedorId.Value);
                    if (!fornecedorExists)
                    {
                        return BadRequest($"Fornecedor com ID {createProdutoDto.FornecedorId} não encontrado.");
                    }
                }

                // Verificar se a categoria existe (se fornecido) [NOVO]
                if (createProdutoDto.CategoriaId.HasValue)
                {
                    var categoriaExists = await _context.Categorias.AnyAsync(c => c.CategoriaId == createProdutoDto.CategoriaId.Value);
                    if (!categoriaExists)
                    {
                        return BadRequest($"Categoria com ID {createProdutoDto.CategoriaId} não encontrado.");
                    }
                }

                var produto = _mapper.Map<Produto>(createProdutoDto);

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                // Recarregar com os dados do fornecedor e imagens (vazio)
                if (produto.FornecedorId.HasValue)
                {
                    await _context.Entry(produto).Reference(p => p.Fornecedor).LoadAsync();
                }

                var produtoDto = _mapper.Map<ProdutoDto>(produto);
                return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Produto/5
        [HttpPut("{id}")]
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
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                // Verificar se o fornecedor existe (se fornecido)
                if (updateProdutoDto.FornecedorId.HasValue)
                {
                    var fornecedorExists = await _context.Fornecedores.AnyAsync(f => f.FornecedorId == updateProdutoDto.FornecedorId.Value);
                    if (!fornecedorExists)
                    {
                        return BadRequest($"Fornecedor com ID {updateProdutoDto.FornecedorId} não encontrado.");
                    }
                }

                // Verificar se a categoria existe (se fornecido) [NOVO]
                if (updateProdutoDto.CategoriaId.HasValue)
                {
                    var categoriaExists = await _context.Categorias.AnyAsync(c => c.CategoriaId == updateProdutoDto.CategoriaId.Value);
                    if (!categoriaExists)
                    {
                        return BadRequest($"Categoria com ID {updateProdutoDto.CategoriaId} não encontrado.");
                    }
                }

                _mapper.Map(updateProdutoDto, produto);

                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Erro de concorrência. O registro foi modificado por outro usuário.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // DELETE: api/Produto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}