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
            var user = await _context.Funcionarios.FirstOrDefaultAsync(f => f.Email == loginDto.Email);

            // Em produção, use BCrypt.Verify(loginDto.Senha, user.PasswordHash)
            if (user == null || loginDto.Senha != "admin123") // Simplificado para teste imediato
                return Unauthorized("Credenciais inválidas");

            var token = GenerateJwtToken(user);
            return Ok(new { token, user = new { user.Nome, user.Email, Role = "Admin" } });
        }

        private string GenerateJwtToken(Funcionario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSegura123!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                new Claim("id", user.FuncionarioId.ToString()),
                new Claim("role", "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: "SigaPet",
                audience: "SigaPetApp",
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}