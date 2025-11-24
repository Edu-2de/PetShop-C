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
    public class AgendamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AgendamentoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Agendamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentos()
        {
            try
            {
                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Agendamento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgendamentoDto>> GetAgendamento(int id)
        {
            try
            {
                var agendamento = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .FirstOrDefaultAsync(a => a.AgendamentoId == id);

                if (agendamento == null)
                {
                    return NotFound($"Agendamento com ID {id} não encontrado.");
                }

                var agendamentoDto = _mapper.Map<AgendamentoDto>(agendamento);
                return Ok(agendamentoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Agendamento/animal/5
        [HttpGet("animal/{animalId}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByAnimal(int animalId)
        {
            try
            {
                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.AnimalId == animalId)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Agendamento/data/2024-01-01
        [HttpGet("data/{data}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByData(DateTime data)
        {
            try
            {
                var dataInicio = data.Date;
                var dataFim = dataInicio.AddDays(1);

                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.DataHora >= dataInicio && a.DataHora < dataFim)
                    .AsNoTracking()
                    .ToListAsync();

                var agendamentosDto = _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
                return Ok(agendamentosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Agendamento
        [HttpPost]
        public async Task<ActionResult<AgendamentoDto>> CreateAgendamento([FromBody] CreateAgendamentoDto createAgendamentoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o animal existe
                var animalExists = await _context.Animais.AnyAsync(a => a.AnimalId == createAgendamentoDto.AnimalId);
                if (!animalExists)
                {
                    return BadRequest($"Animal com ID {createAgendamentoDto.AnimalId} não encontrado.");
                }

                // Verificar se o serviço existe
                var servicoExists = await _context.Servicos.AnyAsync(s => s.ServicoId == createAgendamentoDto.ServicoId);
                if (!servicoExists)
                {
                    return BadRequest($"Serviço com ID {createAgendamentoDto.ServicoId} não encontrado.");
                }

                // Verificar se o funcionário existe (se fornecido)
                if (createAgendamentoDto.FuncionarioId.HasValue)
                {
                    var funcionarioExists = await _context.Funcionarios.AnyAsync(f => f.FuncionarioId == createAgendamentoDto.FuncionarioId.Value);
                    if (!funcionarioExists)
                    {
                        return BadRequest($"Funcionário com ID {createAgendamentoDto.FuncionarioId} não encontrado.");
                    }
                }

                var agendamento = _mapper.Map<Agendamento>(createAgendamentoDto);

                _context.Agendamentos.Add(agendamento);
                await _context.SaveChangesAsync();

                // Recarregar com os dados relacionados
                await _context.Entry(agendamento).Reference(a => a.Animal).LoadAsync();
                await _context.Entry(agendamento).Reference(a => a.Servico).LoadAsync();
                if (agendamento.FuncionarioId.HasValue)
                {
                    await _context.Entry(agendamento).Reference(a => a.Funcionario).LoadAsync();
                }

                var agendamentoDto = _mapper.Map<AgendamentoDto>(agendamento);
                return CreatedAtAction(nameof(GetAgendamento), new { id = agendamento.AgendamentoId }, agendamentoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Agendamento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgendamento(int id, [FromBody] UpdateAgendamentoDto updateAgendamentoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var agendamento = await _context.Agendamentos.FindAsync(id);
                if (agendamento == null)
                {
                    return NotFound($"Agendamento com ID {id} não encontrado.");
                }

                // Verificar se o animal existe
                var animalExists = await _context.Animais.AnyAsync(a => a.AnimalId == updateAgendamentoDto.AnimalId);
                if (!animalExists)
                {
                    return BadRequest($"Animal com ID {updateAgendamentoDto.AnimalId} não encontrado.");
                }

                // Verificar se o serviço existe
                var servicoExists = await _context.Servicos.AnyAsync(s => s.ServicoId == updateAgendamentoDto.ServicoId);
                if (!servicoExists)
                {
                    return BadRequest($"Serviço com ID {updateAgendamentoDto.ServicoId} não encontrado.");
                }

                // Verificar se o funcionário existe (se fornecido)
                if (updateAgendamentoDto.FuncionarioId.HasValue)
                {
                    var funcionarioExists = await _context.Funcionarios.AnyAsync(f => f.FuncionarioId == updateAgendamentoDto.FuncionarioId.Value);
                    if (!funcionarioExists)
                    {
                        return BadRequest($"Funcionário com ID {updateAgendamentoDto.FuncionarioId} não encontrado.");
                    }
                }

                _mapper.Map(updateAgendamentoDto, agendamento);

                _context.Entry(agendamento).State = EntityState.Modified;
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

        // DELETE: api/Agendamento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgendamento(int id)
        {
            try
            {
                var agendamento = await _context.Agendamentos.FindAsync(id);
                if (agendamento == null)
                {
                    return NotFound($"Agendamento com ID {id} não encontrado.");
                }

                _context.Agendamentos.Remove(agendamento);
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
