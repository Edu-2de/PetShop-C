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
            var produto = await _context.Produtos.FindAsync(produtoId);
            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            var uploadsFolderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "..", "Frontend", "src", "assets", "images", "products");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"assets/images/products/{fileName}";

            var produtoImagem = new ProdutoImagem
            {
                Url = imageUrl,
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
