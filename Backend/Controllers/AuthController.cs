using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
using SIGA_PET.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace SIGA_PET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Usuarios
                .Include(u => u.Funcionario)
                .Include(u => u.Tutor)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, user.PasswordHash))
                return Unauthorized("Credenciais inválidas");

            if (!user.Ativo)
                return Unauthorized("Usuário inativo");

            var token = GenerateJwtToken(user);

            var nome = user.Funcionario?.Nome ?? user.Tutor?.Nome ?? "Admin";
            var idVinculo = user.Funcionario?.FuncionarioId ?? user.Tutor?.TutorId ?? 0;

            return Ok(new
            {
                token,
                usuario = new
                {
                    email = user.Email,
                    nome = nome,
                    cargo = user.TipoUsuario,
                    id = idVinculo
                }
            });
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (await _context.Usuarios.AnyAsync()) return Ok("Usuários já existem.");

            // Criar Admin
            var adminUser = new Usuario
            {
                Email = "admin@sigapet.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                TipoUsuario = "Admin",
                Ativo = true
            };
            _context.Usuarios.Add(adminUser);
            await _context.SaveChangesAsync();

            // Criar um Funcionario vinculado a um Usuario
            var funcUser = new Usuario
            {
                Email = "func@sigapet.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("func123"),
                TipoUsuario = "Funcionario",
                Ativo = true
            };
            _context.Usuarios.Add(funcUser);
            await _context.SaveChangesAsync();

            var funcionario = new Funcionario
            {
                Nome = "João Funcionário",
                Cargo = "Atendente",
                UsuarioId = funcUser.UsuarioId
            };
            _context.Funcionarios.Add(funcionario);

            await _context.SaveChangesAsync();
            return Ok("Seed realizado com sucesso!");
        }

        private string GenerateJwtToken(Usuario user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSegura123!");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.UsuarioId.ToString()),
                new Claim("role", user.TipoUsuario)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}