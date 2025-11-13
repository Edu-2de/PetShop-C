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

        // GET: api/Animal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDto>> GetAnimal(int id)
        {
            try
            {
                var animal = await _context.Animais.FindAsync(id);

                if (animal == null)
                {
                    return NotFound($"Animal com ID {id} não encontrado.");
                }

                var animalDto = _mapper.Map<AnimalDto>(animal);
                return Ok(animalDto);
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

                var animal = _mapper.Map<Animal>(createAnimalDto);

                _context.Animais.Add(animal);
                await _context.SaveChangesAsync();

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
                    return NotFound($"Animal com ID {id} não encontrado.");
                }

                // Se o DTO permitir alterar TutorId e estiver nulo, mantemos o original.
                // O AutoMapper já aplicará apenas as propriedades não-nulas se configurado; aqui chamamos o Map direto.
                _mapper.Map(updateAnimalDto, animal);

                _context.Entry(animal).State = EntityState.Modified;
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

        // DELETE: api/Animal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                var animal = await _context.Animais.FindAsync(id);
                if (animal == null)
                {
                    return NotFound($"Animal com ID {id} não encontrado.");
                }

                _context.Animais.Remove(animal);
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