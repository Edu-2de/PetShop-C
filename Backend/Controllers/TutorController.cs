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
            // SEMPRE incluir Usuario, pois é a entidade raiz
            var tutor = await _context.Tutores
                .Include(t => t.Usuario) // OBRIGATÓRIO - Todo tutor deve ter um usuário
                .FirstOrDefaultAsync(t => t.TutorId == id);
                
            if (tutor == null) 
                return NotFound("Tutor não encontrado.");
                
            var tutorDto = _mapper.Map<TutorDto>(tutor);
            return Ok(tutorDto);
        }

        [HttpPost]
        public async Task<ActionResult<TutorDto>> CreateTutor([FromBody] CreateTutorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email)) return BadRequest("Email já em uso.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. SEMPRE criar usuário primeiro (base)
                var usuario = new Usuario 
                { 
                    Nome = dto.Nome, // Nome sempre no Usuario
                    Email = dto.Email, 
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha), 
                    TipoUsuario = "Tutor", 
                    Ativo = true 
                };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // 2. Criar tutor vinculado
                var tutor = new Tutor 
                { 
                    Nome = dto.Nome, // Sincroniza com Usuario
                    Telefone = dto.Telefone, 
                    Endereco = dto.Endereco, 
                    UsuarioId = usuario.UsuarioId,
                    DataCadastro = DateTime.Now
                };
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

        /// <summary>
        /// 🆕 Criar tutor simplificado (sem senha) - EXCEÇÃO para casos específicos
        /// </summary>
        /// <param name="dto">Dados básicos do tutor</param>
        /// <remarks>
        /// ⚠️ ATENÇÃO: Este método é uma EXCEÇÃO à regra de que "Usuario é sempre obrigatório".
        /// Usado APENAS para casos específicos como:
        /// - Agendamentos rápidos onde cliente não quer criar conta
        /// - Compras de balcão onde só precisamos do nome para nota fiscal
        /// 
        /// **Regra geral: TODO tutor DEVE ter um Usuario. Use POST /api/Tutor normal.**
        /// 
        /// **Exemplo de requisição:**
        /// ```json
        /// {
        ///   "nome": "João Silva",
        ///   "telefone": "(11) 98765-4321",
        ///   "endereco": "Rua Exemplo, 123"
        /// }
        /// ```
        /// </remarks>
        [HttpPost("simplificado")]
        public async Task<ActionResult<TutorDto>> CreateTutorSimplificado([FromBody] CreateTutorSimplificadoDto dto)
        {
            try
            {
                if (!ModelState.IsValid) 
                    return BadRequest(ModelState);

                // Verificar se já existe tutor com mesmo telefone
                var tutorExistente = await _context.Tutores
                    .FirstOrDefaultAsync(t => t.Telefone == dto.Telefone);

                if (tutorExistente != null)
                {
                    // Retornar tutor existente
                    var tutorDtoExistente = _mapper.Map<TutorDto>(tutorExistente);
                    return Ok(tutorDtoExistente);
                }

                // ⚠️ EXCEÇÃO: Criar tutor sem usuário (UsuarioId = null)
                // Só para casos muito específicos de agendamento/compra rápida
                var tutor = new Tutor 
                { 
                    Nome = dto.Nome, 
                    Telefone = dto.Telefone, 
                    Endereco = dto.Endereco ?? "Não informado",
                    DataCadastro = DateTime.UtcNow,
                    UsuarioId = null // SEM USUÁRIO - exceção à regra
                };

                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                var tutorDto = _mapper.Map<TutorDto>(tutor);
                return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTutor(int id, [FromBody] UpdateTutorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // SEMPRE incluir Usuario para edição completa
            var tutor = await _context.Tutores
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.TutorId == id);

            if (tutor == null) return NotFound("Tutor não encontrado.");

            // ATENÇÃO: Usuario é a raiz - sempre atualizar dados principais no Usuario
            if (tutor.Usuario != null)
            {
                // Nome SEMPRE fica no Usuario (entidade raiz)
                tutor.Usuario.Nome = dto.Nome;
                
                // Email também fica no Usuario
                if (!string.IsNullOrEmpty(dto.Email))
                {
                    bool emailEmUso = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.UsuarioId != tutor.UsuarioId);
                    if (emailEmUso)
                    {
                        return BadRequest("Este email já está em uso.");
                    }
                    tutor.Usuario.Email = dto.Email;
                }
            }

            // Dados específicos do Tutor (telefone e endereço)
            tutor.Nome = dto.Nome; // Sincroniza com Usuario por segurança
            tutor.Telefone = dto.Telefone;
            tutor.Endereco = dto.Endereco;

            _context.Entry(tutor).State = EntityState.Modified;
            if (tutor.Usuario != null)
            {
                _context.Entry(tutor.Usuario).State = EntityState.Modified;
            }

            try 
            { 
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException) 
            { 
                return Conflict("Erro de concorrência."); 
            }

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

                // 2. Removemos o Usuario (Login) se existir
                if (tutor.UsuarioId.HasValue)
                {
                    var usuario = await _context.Usuarios.FindAsync(tutor.UsuarioId);
                    if (usuario != null) _context.Usuarios.Remove(usuario);
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // Captura exceções de restrição de chave estrangeira
                return StatusCode(500, $"Erro ao deletar: O tutor pode estar associado a outros registros (ex: vendas). Detalhes: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}