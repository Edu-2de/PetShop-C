using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIGA_PET.Data;
using SIGA_PET.DTOs;
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
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Email == loginDto.Email);

            if (funcionario == null)
                return Unauthorized("Usuário ou senha inválidos.");

            // Verifica a senha (assumindo que no banco já estará com Hash, veja o passo de registro abaixo)
            // Para testes iniciais sem hash no banco, você teria que comparar string pura, mas vamos fazer o certo:
            if (string.IsNullOrEmpty(funcionario.PasswordHash) || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, funcionario.PasswordHash))
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            var token = GerarTokenJwt(funcionario);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Usuario = _mapper.Map<FuncionarioDto>(funcionario)
            });
        }

        // Endpoint auxiliar para criar usuário com senha hash (Use este para criar seu primeiro admin via Postman/Swagger)
        [HttpPost("registrar-admin")]
        public async Task<IActionResult> RegistrarAdmin()
        {
            if (await _context.Funcionarios.AnyAsync(f => f.Email == "admin@sigapet.com"))
                return BadRequest("Admin já existe");

            var admin = new Models.Funcionario
            {
                Nome = "Administrador",
                Email = "admin@sigapet.com",
                Login = "admin",
                Cargo = "Gerente",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Cria o hash
                Telefone = "000000000"
            };

            _context.Funcionarios.Add(admin);
            await _context.SaveChangesAsync();

            return Ok("Admin criado com senha 'admin123'");
        }

        private string GerarTokenJwt(Models.Funcionario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email ?? ""),
                new Claim("id", usuario.FuncionarioId.ToString()),
                new Claim("role", usuario.Cargo ?? "User") // Use o Cargo como Role
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