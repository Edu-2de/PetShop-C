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
    public class FornecedorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FornecedorController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Fornecedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorDto>>> GetFornecedores()
        {
            try
            {
                var fornecedores = await _context.Fornecedores
                    .AsNoTracking()
                    .ToListAsync();

                var fornecedoresDto = _mapper.Map<IEnumerable<FornecedorDto>>(fornecedores);
                return Ok(fornecedoresDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Fornecedor/search?name=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<FornecedorDto>>> SearchFornecedores([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O nome para busca não pode ser vazio.");
            }

            var fornecedores = await _context.Fornecedores
                .AsNoTracking()
                // Correção: Verifica se RazaoSocial não é nulo antes de chamar Contains
                .Where(f => f.Nome.Contains(name) || (f.RazaoSocial != null && f.RazaoSocial.Contains(name)))
                .ToListAsync();

            if (!fornecedores.Any())
            {
                return NotFound("Nenhum fornecedor encontrado com o nome ou razão social fornecida.");
            }

            var fornecedoresDto = _mapper.Map<IEnumerable<FornecedorDto>>(fornecedores);
            return Ok(fornecedoresDto);
        }

        // GET: api/Fornecedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorDto>> GetFornecedor(int id)
        {
            try
            {
                var fornecedor = await _context.Fornecedores.FindAsync(id);

                if (fornecedor == null)
                {
                    return NotFound($"Fornecedor com ID {id} não encontrado.");
                }

                var fornecedorDto = _mapper.Map<FornecedorDto>(fornecedor);
                return Ok(fornecedorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Fornecedor
        [HttpPost]
        public async Task<ActionResult<FornecedorDto>> CreateFornecedor([FromBody] CreateFornecedorDto createFornecedorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fornecedor = _mapper.Map<Fornecedor>(createFornecedorDto);

                _context.Fornecedores.Add(fornecedor);
                await _context.SaveChangesAsync();

                var fornecedorDto = _mapper.Map<FornecedorDto>(fornecedor);
                return CreatedAtAction(nameof(GetFornecedor), new { id = fornecedor.FornecedorId }, fornecedorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Fornecedor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFornecedor(int id, [FromBody] UpdateFornecedorDto updateFornecedorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fornecedor = await _context.Fornecedores.FindAsync(id);
                if (fornecedor == null)
                {
                    return NotFound($"Fornecedor com ID {id} não encontrado.");
                }

                _mapper.Map(updateFornecedorDto, fornecedor);

                _context.Entry(fornecedor).State = EntityState.Modified;
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

        // DELETE: api/Fornecedor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            try
            {
                var fornecedor = await _context.Fornecedores.FindAsync(id);
                if (fornecedor == null)
                {
                    return NotFound($"Fornecedor com ID {id} não encontrado.");
                }

                _context.Fornecedores.Remove(fornecedor);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Captura exceções de restrição de chave estrangeira
                return StatusCode(500, $"Erro ao deletar: O fornecedor pode estar associado a produtos. Detalhes: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}