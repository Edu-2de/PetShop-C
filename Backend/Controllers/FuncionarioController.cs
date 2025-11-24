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
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FuncionarioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Funcionario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioDto>>> GetFuncionarios()
        {
            try
            {
                var funcionarios = await _context.Funcionarios
                    .AsNoTracking()
                    .ToListAsync();

                var funcionariosDto = _mapper.Map<IEnumerable<FuncionarioDto>>(funcionarios);
                return Ok(funcionariosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Funcionario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioDto>> GetFuncionario(int id)
        {
            try
            {
                var funcionario = await _context.Funcionarios.FindAsync(id);

                if (funcionario == null)
                {
                    return NotFound($"Funcionário com ID {id} não encontrado.");
                }

                var funcionarioDto = _mapper.Map<FuncionarioDto>(funcionario);
                return Ok(funcionarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Funcionario
        [HttpPost]
        public async Task<ActionResult<FuncionarioDto>> CreateFuncionario([FromBody] CreateFuncionarioDto createFuncionarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var funcionario = _mapper.Map<Funcionario>(createFuncionarioDto);

                // Se DataContratacao não for fornecida, o model define um default (UtcNow), mas você pode ajustar aqui.
                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();

                var funcionarioDto = _mapper.Map<FuncionarioDto>(funcionario);
                return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.FuncionarioId }, funcionarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Funcionario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuncionario(int id, [FromBody] UpdateFuncionarioDto updateFuncionarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    return NotFound($"Funcionário com ID {id} não encontrado.");
                }

                _mapper.Map(updateFuncionarioDto, funcionario);

                _context.Entry(funcionario).State = EntityState.Modified;
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

        // DELETE: api/Funcionario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            try
            {
                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    return NotFound($"Funcionário com ID {id} não encontrado.");
                }

                _context.Funcionarios.Remove(funcionario);
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