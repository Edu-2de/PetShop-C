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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioDto>>> GetFuncionarios()
        {
            var funcionarios = await _context.Funcionarios.Include(f => f.Usuario).AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<FuncionarioDto>>(funcionarios));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioDto>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.Include(f => f.Usuario).FirstOrDefaultAsync(f => f.FuncionarioId == id);
            if (funcionario == null) return NotFound("Funcionário não encontrado.");
            return Ok(_mapper.Map<FuncionarioDto>(funcionario));
        }

        [HttpPost]
        public async Task<ActionResult<FuncionarioDto>> CreateFuncionario([FromBody] CreateFuncionarioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email)) return BadRequest("Email já em uso.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var tipo = (dto.Cargo != null && dto.Cargo.Contains("Gerente", StringComparison.OrdinalIgnoreCase)) ? "Admin" : "Funcionario";
                var usuario = new Usuario { Email = dto.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha), TipoUsuario = tipo, Ativo = true };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var func = new Funcionario { Nome = dto.Nome, Cargo = dto.Cargo, Telefone = dto.Telefone, DataContratacao = dto.DataContratacao ?? DateTime.UtcNow, UsuarioId = usuario.UsuarioId };
                _context.Funcionarios.Add(func);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                var funcDto = _mapper.Map<FuncionarioDto>(func);
                funcDto.Email = usuario.Email;
                return CreatedAtAction(nameof(GetFuncionario), new { id = func.FuncionarioId }, funcDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuncionario(int id, [FromBody] UpdateFuncionarioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var func = await _context.Funcionarios.FindAsync(id);
            if (func == null) return NotFound("Funcionário não encontrado.");

            _mapper.Map(dto, func);
            _context.Entry(func).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { return Conflict("Erro de concorrência."); }

            return NoContent();
        }

        // DELETE CORRIGIDO
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            try
            {
                var func = await _context.Funcionarios.FindAsync(id);
                if (func == null) return NotFound("Funcionário não encontrado.");

                // 1. Remove Funcionario
                _context.Funcionarios.Remove(func);

                // 2. Remove Usuario
                var usuario = await _context.Usuarios.FindAsync(func.UsuarioId);
                if (usuario != null) _context.Usuarios.Remove(usuario);

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