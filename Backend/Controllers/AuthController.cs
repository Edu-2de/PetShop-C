using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIGA_PET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Funcionario)
                .Include(u => u.Tutor)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.PasswordHash))
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var token = GenerateJwtToken(usuario);
            var userInfo = _mapper.Map<UserInfo>(usuario);

            return Ok(new LoginResponseDto { Token = token, Usuario = userInfo });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateTutorDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se email já existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return BadRequest("Este email já está cadastrado.");
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // 1. CRIAR USUÁRIO PRIMEIRO (sempre obrigatório)
                    var usuario = new Usuario
                    {
                        Nome = registerDto.Nome, // Nome sempre vem do Usuario
                        Email = registerDto.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Senha),
                        TipoUsuario = "Tutor",
                        Ativo = true
                    };

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    // 2. Criar tutor vinculado ao usuário
                    var tutor = new Tutor
                    {
                        Nome = registerDto.Nome, // Sincroniza nome com Usuario
                        Telefone = registerDto.Telefone,
                        Endereco = registerDto.Endereco ?? "Não informado",
                        UsuarioId = usuario.UsuarioId,
                        DataCadastro = DateTime.UtcNow
                    };

                    _context.Tutores.Add(tutor);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    // Gerar token e retornar como login
                    await _context.Entry(usuario).Reference(u => u.Tutor).LoadAsync();
                    var token = GenerateJwtToken(usuario);
                    var userInfo = _mapper.Map<UserInfo>(usuario);

                    return Ok(new LoginResponseDto
                    {
                        Token = token,
                        Usuario = userInfo
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Erro ao criar conta: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured."));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                // A role é o 'Cargo' para funcionários ou 'TipoUsuario' para outros (ex: Tutor)
                new Claim(ClaimTypes.Role, usuario.Funcionario?.Cargo ?? usuario.TipoUsuario)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "8")),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}