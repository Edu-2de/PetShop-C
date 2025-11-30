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
    public class ProdutoImagemController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public ProdutoImagemController(AppDbContext context, IWebHostEnvironment hostingEnvironment, IMapper mapper)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        [HttpPost("{produtoId}/upload")]
        public async Task<ActionResult<ProdutoImagemDto>> UploadImagem(int produtoId, IFormFile file)
        {
            // ... (verificações iniciais de produto e file null mantêm iguais)

            // 1. Define o caminho físico onde vai salvar (na pasta da API)
            var folderName = Path.Combine("wwwroot", "imagens");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            // 2. Gera nome único
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(pathToSave, fileName);

            // 3. Salva o arquivo
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 4. Cria a URL pública (IMPORTANTE: Caminho relativo para o frontend usar com a URL da API)
            // O frontend vai montar: http://localhost:5000/imagens/nome-do-arquivo.jpg
            var dbPath = $"imagens/{fileName}";

            var produtoImagem = new ProdutoImagem
            {
                Url = dbPath, // Salva apenas o caminho relativo
                ProdutoId = produtoId
            };

            _context.ProdutoImagens.Add(produtoImagem);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProdutoImagemDto>(produtoImagem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagem(int id)
        {
            var imagem = await _context.ProdutoImagens.FindAsync(id);
            if (imagem == null)
            {
                return NotFound();
            }

            var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "..", "Frontend", "src", imagem.Url.Replace('/', '\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.ProdutoImagens.Remove(imagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
