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
    public class TutorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TutorController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutores()
        {
            var tutores = await _context.Tutores.Include(t => t.Usuario).AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<IEnumerable<TutorDto>>(tutores);
            return Ok(dtos);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TutorDto>>> SearchTutores([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Nome vazio.");
            var tutores = await _context.Tutores.Include(t => t.Usuario).AsNoTracking().Where(t => t.Nome.Contains(name)).ToListAsync();
            if (!tutores.Any()) return NotFound("Nenhum tutor encontrado.");
            return Ok(_mapper.Map<IEnumerable<TutorDto>>(tutores));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TutorDto>> GetTutor(int id)
        {
            var tutor = await _context.Tutores.Include(t => t.Usuario).FirstOrDefaultAsync(t => t.TutorId == id);
            if (tutor == null) return NotFound("Tutor não encontrado.");
            return Ok(_mapper.Map<TutorDto>(tutor));
        }

        [HttpPost]
        public async Task<ActionResult<TutorDto>> CreateTutor([FromBody] CreateTutorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email)) return BadRequest("Email já em uso.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var usuario = new Usuario { Email = dto.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha), TipoUsuario = "Tutor", Ativo = true };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var tutor = new Tutor { Nome = dto.Nome, Telefone = dto.Telefone, Endereco = dto.Endereco, UsuarioId = usuario.UsuarioId };
                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                var tutorDto = _mapper.Map<TutorDto>(tutor);
                tutorDto.Email = usuario.Email;
                return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutorDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutor(int id, [FromBody] UpdateTutorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // CORREÇÃO: Incluir Usuario para editar email
            var tutor = await _context.Tutores
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.TutorId == id);

            if (tutor == null) return NotFound("Tutor não encontrado.");

            _mapper.Map(dto, tutor);

            // Atualiza Email se necessário
            if (!string.IsNullOrEmpty(dto.Email) && tutor.Usuario != null)
            {
                bool emailEmUso = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.UsuarioId != tutor.UsuarioId);
                if (emailEmUso)
                {
                    return BadRequest("Este email já está em uso.");
                }
                tutor.Usuario.Email = dto.Email;
            }

            _context.Entry(tutor).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { return Conflict("Erro de concorrência."); }

            return NoContent();
        }

        // DELETE CORRIGIDO PARA EVITAR ERRO DE CICLO SQL
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            try
            {
                var tutor = await _context.Tutores.FindAsync(id);
                if (tutor == null) return NotFound("Tutor não encontrado.");

                // 1. Removemos o Tutor (dispara cascade para Animais, etc)
                _context.Tutores.Remove(tutor);

                // 2. Removemos o Usuario (Login)
                var usuario = await _context.Usuarios.FindAsync(tutor.UsuarioId);
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