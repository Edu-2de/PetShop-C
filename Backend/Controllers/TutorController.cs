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

        // GET: api/Tutor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutores()
        {
            var tutores = await _context.Tutores
                .Include(t => t.Usuario) // Importante: Incluir Usuario para pegar o email
                .AsNoTracking()
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TutorDto>>(tutores);
            return Ok(dtos);
        }
        

        // GET: api/Tutor/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TutorDto>>> SearchTutores([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O nome para busca não pode ser vazio.");
            }

            var tutores = await _context.Tutores
                .AsNoTracking()
                .Where(t => t.Nome.Contains(name))
                .ToListAsync();

            if (!tutores.Any())
            {
                return NotFound("Nenhum tutor encontrado com o nome fornecido.");
            }

            var tutoresDto = _mapper.Map<IEnumerable<TutorDto>>(tutores);
            return Ok(tutoresDto);
        }

        // GET: api/Tutor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TutorDto>> GetTutor(int id)
        {
            try
            {
                var tutor = await _context.Tutores.FindAsync(id);

                if (tutor == null)
                {
                    return NotFound($"Tutor com ID {id} não encontrado.");
                }

                var tutorDto = _mapper.Map<TutorDto>(tutor);
                return Ok(tutorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Tutor
        [HttpPost]
        public async Task<ActionResult<TutorDto>> CreateTutor([FromBody] CreateTutorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1. Validar se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Este e-mail já está em uso.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2. Criar Usuario (Login)
                var usuario = new Usuario
                {
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                    TipoUsuario = "Tutor",
                    Ativo = true
                };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // 3. Criar Tutor vinculado
                var tutor = new Tutor
                {
                    Nome = dto.Nome,
                    Telefone = dto.Telefone,
                    Endereco = dto.Endereco,
                    UsuarioId = usuario.UsuarioId // Vínculo importante!
                };
                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // 4. Retornar DTO
                var tutorDto = _mapper.Map<TutorDto>(tutor);
                // Preenchemos o email manualmente no retorno pois ele vem de Usuario
                tutorDto.Email = usuario.Email;

                return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutorDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao criar tutor: {ex.Message}");
            }
        }

        // PUT: api/Tutor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutor(int id, [FromBody] UpdateTutorDto updateTutorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var tutor = await _context.Tutores.FindAsync(id);
                if (tutor == null)
                {
                    return NotFound($"Tutor com ID {id} não encontrado.");
                }

                _mapper.Map(updateTutorDto, tutor);

                _context.Entry(tutor).State = EntityState.Modified;
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

        // DELETE: api/Tutor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            try
            {
                var tutor = await _context.Tutores.FindAsync(id);
                if (tutor == null)
                {
                    return NotFound($"Tutor com ID {id} não encontrado.");
                }

                _context.Tutores.Remove(tutor);
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
