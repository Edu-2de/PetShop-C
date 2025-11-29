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
            try
            {
                var tutores = await _context.Tutores
                    .AsNoTracking()
                    .ToListAsync();

                var tutoresDto = _mapper.Map<IEnumerable<TutorDto>>(tutores);
                return Ok(tutoresDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
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
        public async Task<ActionResult<TutorDto>> CreateTutor([FromBody] CreateTutorDto createTutorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var tutor = _mapper.Map<Tutor>(createTutorDto);

                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                var tutorDto = _mapper.Map<TutorDto>(tutor);
                return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
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
