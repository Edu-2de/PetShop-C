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
using AutoMapper;

namespace SIGA_PET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var user = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Email == loginDto.Email);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, user.PasswordHash))
                return Unauthorized("Credenciais inválidas");

            var token = GenerateJwtToken(user);
            
            // Mapeia para DTO para não devolver senha/hash
            var userDto = _mapper.Map<FuncionarioDto>(user);
            
            return Ok(new { token, usuario = userDto });
        }

        // ROTA NOVA: Execute isso no Swagger para criar os usuários iniciais
        [HttpPost("seed")]
        public async Task<IActionResult> SeedUsers()
        {
            var msgs = new List<string>();

            // 1. Criar Admin
            if (!await _context.Funcionarios.AnyAsync(f => f.Email == "admin@sigapet.com"))
            {
                var admin = new Funcionario
                {
                    Nome = "Administrador",
                    Email = "admin@sigapet.com",
                    Login = "admin",
                    Cargo = "Gerente", // Cargo define que é Admin no Frontend
                    Telefone = "11999999999",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    DataContratacao = DateTime.UtcNow
                };
                _context.Funcionarios.Add(admin);
                msgs.Add("Admin criado (admin@sigapet.com / admin123)");
            }

            // 2. Criar Usuário Padrão (Funcionario com cargo menor ou apenas um registro para teste)
            if (!await _context.Funcionarios.AnyAsync(f => f.Email == "user@sigapet.com"))
            {
                var user = new Funcionario
                {
                    Nome = "Cliente Padrão",
                    Email = "user@sigapet.com",
                    Login = "user",
                    Cargo = "Cliente", // Cargo diferente de Gerente
                    Telefone = "11888888888",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    DataContratacao = DateTime.UtcNow
                };
                _context.Funcionarios.Add(user);
                msgs.Add("Usuário criado (user@sigapet.com / user123)");
            }

            await _context.SaveChangesAsync();

            if (msgs.Count == 0) return Ok("Usuários já existiam.");
            return Ok(msgs);
        }

        private string GenerateJwtToken(Funcionario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Definindo roles baseadas no cargo
            var role = (user.Cargo == "Gerente" || user.Cargo == "Administrador") ? "admin" : "user";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                new Claim("id", user.FuncionarioId.ToString()),
                new Claim("role", role),
                new Claim("nome", user.Nome)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}