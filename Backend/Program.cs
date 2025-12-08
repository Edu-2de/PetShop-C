using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SIGA_PET.Data;
using SIGA_PET.Profiles;
using System.Text;
using System.Text.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ==============================================================================
// 1. CONFIGURAÇÃO DO JSON (EVITAR REFERÊNCIAS CÍCLICAS)
// ==============================================================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
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

// 6. Swagger com Documentação Completa e Melhorada
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Informações da API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "?? SIGA-PET API",
        Version = "v2.0.0",
        Description = @"
# API RESTful Completa para Gestão de Pet Shop

## ?? Funcionalidades

### ?? Autenticação e Usuários
- Login e Registro de usuários
- Controle de perfis (Admin, Funcionário, Tutor)
- Autenticação JWT

### ?? Gestão de Pessoas
- **Tutores**: Clientes/donos de pets
- **Funcionários**: Veterinários, Tosadores, Atendentes
- **Animais**: Cadastro completo de pets

### ?? E-commerce
- **Produtos**: Catálogo completo com imagens
- **Categorias**: Organização de produtos
- **Carrinho**: Gestão de compras
- **Vendas**: Histórico e relatórios

### ?? Agendamentos
- Serviços de banho, tosa e veterinária
- Controle de horários e profissionais
- Validação de conflitos

### ??? Banco de Dados
- **Resetar Banco**: Recria toda estrutura
- **Popular Banco**: Insere dados de exemplo
- **Status**: Verifica estado do banco

## ?? Começando

1. **Resetar e Popular o Banco**
   - Use o endpoint `POST /api/Database/reset-e-popular`
   - Isso cria a estrutura e insere dados de exemplo

2. **Fazer Login**
   - Use `POST /api/Auth/login` com as credenciais fornecidas
   - Copie o token JWT retornado

3. **Autorizar no Swagger**
   - Clique no botão ?? **Authorize** no topo
   - Digite: `Bearer {seu_token_aqui}`
   - Agora pode testar os endpoints protegidos!

## ?? Credenciais de Teste

Após popular o banco, use estas credenciais:

| Perfil | Email | Senha |
|--------|-------|-------|
| ????? Admin | admin@sigapet.com | senha123 |
| ????? Veterinário | carlos.vet@sigapet.com | senha123 |
| ?? Tosador | ana.tosa@sigapet.com | senha123 |
| ?? Cliente | maria.silva@email.com | senha123 |

## ?? Documentação Completa

Explore os endpoints abaixo para ver exemplos detalhados de requisições e respostas.
",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Suporte SIGA-PET",
            Email = "suporte@sigapet.com",
            Url = new Uri("https://github.com/Edu-2de/PetShop-C")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Segurança JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = @"
**Como usar a autenticação JWT:**

1. Faça login usando `POST /api/Auth/login`
2. Copie o token retornado no campo `token`
3. Clique no botão ?? **Authorize** acima
4. Digite: `Bearer {seu_token_aqui}` (substitua {seu_token_aqui} pelo token copiado)
5. Clique em **Authorize** e depois **Close**

Agora você pode testar os endpoints protegidos!

**Exemplo:**
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```
",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    // Tags personalizadas para organização
    options.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { controllerName ?? "Default" };
    });

    // Ordenar por nome do controller
    options.OrderActionsBy(api => $"{api.ActionDescriptor.RouteValues["controller"]}_{api.HttpMethod}");

    // XML Documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();

// ==============================================================================
// CONFIGURAÇÃO DE PASTAS DE UPLOAD
// ==============================================================================
var uploadPath = Path.Combine(app.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "imagens");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
    Console.WriteLine($"[SISTEMA] Pasta de uploads criada: {uploadPath}");
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = false;
    });
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SIGA-PET API v2.0");
        options.RoutePrefix = string.Empty; // Swagger em /
        options.DocumentTitle = "?? SIGA-PET API - Documentação Completa";
        
        // Configurações de UI
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        options.DisplayRequestDuration();
        options.ShowCommonExtensions();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.ShowExtensions();
        
        // Injetar CSS customizado
        options.InjectStylesheet("/swagger-ui/custom.css");
        
        // Tema escuro (opcional)
        // options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
        // {
        //     ["theme"] = "monokai"
        // };
    });
}

// Habilita arquivos estáticos
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

Console.WriteLine(@"
========================================
  ?? SIGA-PET API INICIADA COM SUCESSO
========================================

?? Documentação: http://localhost:5000
?? Swagger UI: http://localhost:5000/swagger
?? Frontend: http://localhost:4200

?? INÍCIO RÁPIDO:
1. Acesse http://localhost:5000
2. Use POST /api/Database/reset-e-popular para criar o banco
3. Use POST /api/Auth/login para autenticar
4. Copie o token e clique em ?? Authorize

? Sistema pronto para uso!
========================================
");

app.Run();