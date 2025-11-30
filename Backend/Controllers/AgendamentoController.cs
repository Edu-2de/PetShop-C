using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;
using SIGA_PET.Enums;

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

        // GET: api/Agendamento/tutor/5
        [HttpGet("tutor/{tutorId}")]
        public async Task<ActionResult<IEnumerable<AgendamentoDto>>> GetAgendamentosByTutor(int tutorId)
        {
            try
            {
                var agendamentos = await _context.Agendamentos
                    .Include(a => a.Animal)
                    .Include(a => a.Servico)
                    .Include(a => a.Funcionario)
                    .Where(a => a.Animal.TutorId == tutorId) // Filtra pelos animais do tutor
                    .OrderByDescending(a => a.DataHora)      // Mais recentes primeiro
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

                var animalExists = await _context.Animais.AnyAsync(a => a.AnimalId == createAgendamentoDto.AnimalId);
                if (!animalExists) return BadRequest($"Animal com ID {createAgendamentoDto.AnimalId} não encontrado.");

                var servicoExists = await _context.Servicos.AnyAsync(s => s.ServicoId == createAgendamentoDto.ServicoId);
                if (!servicoExists) return BadRequest($"Serviço com ID {createAgendamentoDto.ServicoId} não encontrado.");

                if (createAgendamentoDto.FuncionarioId.HasValue)
                {
                    var conflitoFuncionario = await _context.Agendamentos
                        .AnyAsync(a => a.FuncionarioId == createAgendamentoDto.FuncionarioId
                                       && a.DataHora == createAgendamentoDto.DataHora
                                       && a.Status != StatusAgendamento.Cancelado);

                    if (conflitoFuncionario)
                        return BadRequest("Este funcionário já possui um agendamento neste horário.");
                }

                var conflitoPet = await _context.Agendamentos
                    .AnyAsync(a => a.AnimalId == createAgendamentoDto.AnimalId
                                   && a.DataHora == createAgendamentoDto.DataHora
                                   && a.Status != StatusAgendamento.Cancelado);

                if (conflitoPet)
                    return BadRequest("Este pet já possui um agendamento neste horário.");

                var agendamento = _mapper.Map<Agendamento>(createAgendamentoDto);

                _context.Agendamentos.Add(agendamento);
                await _context.SaveChangesAsync();

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
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var agendamento = await _context.Agendamentos.FindAsync(id);
                if (agendamento == null) return NotFound($"Agendamento com ID {id} não encontrado.");

                // Validações básicas de existência
                if (!await _context.Animais.AnyAsync(a => a.AnimalId == updateAgendamentoDto.AnimalId))
                    return BadRequest("Animal não encontrado.");

                if (!await _context.Servicos.AnyAsync(s => s.ServicoId == updateAgendamentoDto.ServicoId))
                    return BadRequest("Serviço não encontrado.");

                _mapper.Map(updateAgendamentoDto, agendamento);

                _context.Entry(agendamento).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Erro de concorrência.");
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
                if (agendamento == null) return NotFound($"Agendamento com ID {id} não encontrado.");

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