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
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var categorias = await _context.Categorias.AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CategoriaDto>>(categorias));
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> CreateCategoria(CreateCategoriaDto dto)
        {
            var categoria = _mapper.Map<Categoria>(dto);
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<CategoriaDto>(categoria));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] CategoriaDto categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest("ID da categoria n�o corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoria n�o encontrada.");
            }

            _mapper.Map(categoriaDto, categoria);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categorias.Any(e => e.CategoriaId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                    .Include(c => c.Produtos)
                    .FirstOrDefaultAsync(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound("Categoria não encontrada.");
                }

                // Verificar se há produtos associados
                if (categoria.Produtos != null && categoria.Produtos.Any())
                {
                    return BadRequest($"Não é possível excluir a categoria pois existem {categoria.Produtos.Count} produto(s) associado(s). Remova os produtos ou altere suas categorias primeiro.");
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao deletar categoria: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}