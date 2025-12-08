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
    public class AnimalController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AnimalController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Animal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimais()
        {
            try
            {
                var animais = await _context.Animais
                    .Include(a => a.Tutor)
                    .AsNoTracking()
                    .ToListAsync();

                var animaisDto = _mapper.Map<IEnumerable<AnimalDto>>(animais);
                return Ok(animaisDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Animal/search?name=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> SearchAnimais([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O nome para busca n�o pode ser vazio.");
            }

            var animais = await _context.Animais
                .Include(a => a.Tutor)
                .AsNoTracking()
                .Where(a => a.Nome.Contains(name))
                .ToListAsync();

            if (!animais.Any())
            {
                return NotFound("Nenhum animal encontrado com o nome fornecido.");
            }

            var animaisDto = _mapper.Map<IEnumerable<AnimalDto>>(animais);
            return Ok(animaisDto);
        }

        // GET: api/Animal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDto>> GetAnimal(int id)
        {
            try
            {
                var animal = await _context.Animais
                    .Include(a => a.Tutor)
                    .FirstOrDefaultAsync(a => a.AnimalId == id);

                if (animal == null)
                {
                    return NotFound($"Animal com ID {id} n�o encontrado.");
                }

                var animalDto = _mapper.Map<AnimalDto>(animal);
                return Ok(animalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Animal/tutor/5
        [HttpGet("tutor/{tutorId}")]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimaisByTutor(int tutorId)
        {
            try
            {
                var animais = await _context.Animais
                    .Include(a => a.Tutor)
                    .Where(a => a.TutorId == tutorId)
                    .AsNoTracking()
                    .ToListAsync();

                var animaisDto = _mapper.Map<IEnumerable<AnimalDto>>(animais);
                return Ok(animaisDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Animal
        [HttpPost]
        public async Task<ActionResult<AnimalDto>> CreateAnimal([FromBody] CreateAnimalDto createAnimalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o tutor existe
                var tutorExists = await _context.Tutores.AnyAsync(t => t.TutorId == createAnimalDto.TutorId);
                if (!tutorExists)
                {
                    return BadRequest($"Tutor com ID {createAnimalDto.TutorId} n�o encontrado.");
                }

                var animal = _mapper.Map<Animal>(createAnimalDto);

                _context.Animais.Add(animal);
                await _context.SaveChangesAsync();

                // Recarregar com os dados do tutor
                await _context.Entry(animal).Reference(a => a.Tutor).LoadAsync();

                var animalDto = _mapper.Map<AnimalDto>(animal);
                return CreatedAtAction(nameof(GetAnimal), new { id = animal.AnimalId }, animalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Animal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] UpdateAnimalDto updateAnimalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var animal = await _context.Animais.FindAsync(id);
                if (animal == null)
                {
                    return NotFound($"Animal com ID {id} n�o encontrado.");
                }

                // Verificar se o tutor existe
                var tutorExists = await _context.Tutores.AnyAsync(t => t.TutorId == updateAnimalDto.TutorId);
                if (!tutorExists)
                {
                    return BadRequest($"Tutor com ID {updateAnimalDto.TutorId} n�o encontrado.");
                }

                _mapper.Map(updateAnimalDto, animal);

                _context.Entry(animal).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Erro de concorr�ncia. O registro foi modificado por outro usu�rio.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // DELETE: api/Animal/5 - COM VERIFICAÇÃO E LIMPEZA DE AGENDAMENTOS
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                var animal = await _context.Animais
                    .Include(a => a.Agendamentos)
                    .FirstOrDefaultAsync(a => a.AnimalId == id);

                if (animal == null)
                {
                    return NotFound($"Animal com ID {id} não encontrado.");
                }

                // Verificar se há agendamentos associados
                if (animal.Agendamentos != null && animal.Agendamentos.Any())
                {
                    // Deletar todos os agendamentos do animal primeiro
                    _context.Agendamentos.RemoveRange(animal.Agendamentos);
                }

                // Remover o animal
                _context.Animais.Remove(animal);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao deletar animal: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}