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
        public async Task<ActionResult<FuncionarioDto>> CreateFuncionario([FromBody] CreateFuncionarioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Este e-mail já está em uso.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Define se é Admin ou Funcionario baseado no Cargo
                var tipoUsuario = (dto.Cargo != null && dto.Cargo.Contains("Gerente", StringComparison.OrdinalIgnoreCase))
                                  ? "Admin"
                                  : "Funcionario";

                var usuario = new Usuario
                {
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                    TipoUsuario = tipoUsuario,
                    Ativo = true
                };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var funcionario = new Funcionario
                {
                    Nome = dto.Nome,
                    Cargo = dto.Cargo,
                    Telefone = dto.Telefone,
                    DataContratacao = dto.DataContratacao ?? DateTime.UtcNow,
                    UsuarioId = usuario.UsuarioId
                };
                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var funcDto = _mapper.Map<FuncionarioDto>(funcionario);
                funcDto.Email = usuario.Email;

                return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.FuncionarioId }, funcDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao criar funcionário: {ex.Message}");
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