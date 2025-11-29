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
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var categorias = await _context.Categorias.AsNoTracking().ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CategoriaDto>>(categorias));
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> CreateCategoria(CreateCategoriaDto dto)
        {
            var categoria = _mapper.Map<Categoria>(dto);
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<CategoriaDto>(categoria));
        }

        // Pode adicionar PUT e DELETE conforme necessidade
    }
}