using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIGA_PET.Data;
using SIGA_PET.Profiles;
using System.Text;
using System.Text.Json.Serialization; // IMPORTANTE: Necessário para o IgnoreCycles

var builder = WebApplication.CreateBuilder(args);

// ==============================================================================
// 1. CORREÇÃO DO ERRO DE LISTAGEM (JSON INFINITO)
// ==============================================================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta linha impede que a API trave ao tentar converter
        // relacionamentos circulares (Produto -> Fornecedor -> Produto...)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 2. Configuração do Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Configuração do AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 4. Configuração de Autenticação e JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSegura123!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 5. Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// 6. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==============================================================================
// 2. CORREÇÃO DO ERRO DE UPLOAD (CRIAR PASTA AUTOMATICAMENTE)
// ==============================================================================
// Isso garante que a pasta de imagens exista antes de qualquer upload
var uploadPath = Path.Combine(app.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "imagens");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
    Console.WriteLine($"[SISTEMA] Pasta de uploads criada com sucesso: {uploadPath}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita o uso de arquivos estáticos (para conseguir ver as imagens depois de salvas)
app.UseStaticFiles();

var imagensPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "imagens");
if (!Directory.Exists(imagensPath))
{
    Directory.CreateDirectory(imagensPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(imagensPath),
    RequestPath = "/imagens"
});

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();