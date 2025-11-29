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
    public class ServicoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ServicoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Servico
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicoDto>>> GetServicos()
        {
            try
            {
                var servicos = await _context.Servicos
                    .AsNoTracking()
                    .ToListAsync();

                var servicosDto = _mapper.Map<IEnumerable<ServicoDto>>(servicos);
                return Ok(servicosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Servico/search?name=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ServicoDto>>> SearchServicos([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O nome para busca não pode ser vazio.");
            }

            var servicos = await _context.Servicos
                .AsNoTracking()
                .Where(s => s.Nome.Contains(name))
                .ToListAsync();

            if (!servicos.Any())
            {
                return NotFound("Nenhum serviço encontrado com o nome fornecido.");
            }

            var servicosDto = _mapper.Map<IEnumerable<ServicoDto>>(servicos);
            return Ok(servicosDto);
        }

        // GET: api/Servico/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicoDto>> GetServico(int id)
        {
            try
            {
                var servico = await _context.Servicos.FindAsync(id);

                if (servico == null)
                {
                    return NotFound($"Serviço com ID {id} não encontrado.");
                }

                var servicoDto = _mapper.Map<ServicoDto>(servico);
                return Ok(servicoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Servico/ativos
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<ServicoDto>>> GetServicosAtivos()
        {
            try
            {
                var servicos = await _context.Servicos
                    .Where(s => s.Ativo)
                    .AsNoTracking()
                    .ToListAsync();

                var servicosDto = _mapper.Map<IEnumerable<ServicoDto>>(servicos);
                return Ok(servicosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Servico
        [HttpPost]
        public async Task<ActionResult<ServicoDto>> CreateServico([FromBody] CreateServicoDto createServicoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var servico = _mapper.Map<Servico>(createServicoDto);

                _context.Servicos.Add(servico);
                await _context.SaveChangesAsync();

                var servicoDto = _mapper.Map<ServicoDto>(servico);
                return CreatedAtAction(nameof(GetServico), new { id = servico.ServicoId }, servicoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Servico/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServico(int id, [FromBody] UpdateServicoDto updateServicoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var servico = await _context.Servicos.FindAsync(id);
                if (servico == null)
                {
                    return NotFound($"Serviço com ID {id} não encontrado.");
                }

                _mapper.Map(updateServicoDto, servico);

                _context.Entry(servico).State = EntityState.Modified;
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

        // DELETE: api/Servico/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServico(int id)
        {
            try
            {
                var servico = await _context.Servicos.FindAsync(id);
                if (servico == null)
                {
                    return NotFound($"Serviço com ID {id} não encontrado.");
                }

                _context.Servicos.Remove(servico);
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
