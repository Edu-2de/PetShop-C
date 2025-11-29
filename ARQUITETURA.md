# ARQUITETURA DO SISTEMA - SIGA-PET

Documentação técnica detalhada da arquitetura do Sistema Integrado de Gestão para PetShop.

---

## Visão Geral

```
               FRONTEND (Angular 17+)
                   http://localhost:4200
+-----------------------------------------------------+
|  +----------+    +----------+    +----------+       |
|  |  Models  |    | Services |    |    UI    |       |
|  | (6 types)|    | (6 http) |    | (futuro) |       |
|  +----------+    +----------+    +----------+       |
+-----------------------------------------------------+
                       | HTTP/REST API
                       | JSON
                       | CORS Enabled
                       |
+-----------------------------------------------------+
|          BACKEND (ASP.NET Core 8.0)                 |
|              http://localhost:5000                  |
|                                                     |
|  +---------------+   +-----------------+            |
|  |  Controllers  |   |      DTOs       |            |
|  |   (6 APIs)    |   | (Create/Update/ |            |
|  |               |   |      Get)       |            |
|  +---------------+   +-----------------+            |
|         |                   |                       |
|  +---------------+   +-----------------+            |
|  |  AutoMapper   |   |     Models      |            |
|  +---------------+   +-----------------+            |
|         |                   |                       |
|         +-------------------+                       |
|                    |                                |
|         +--------------------+                      |
|         |      EF Core       |                      |
|         |      DbContext     |                      |
|         +--------------------+                      |
+-----------------------------------------------------+
                     | ADO.NET
                     |
         +------------------------+
         |   SQL Server LocalDB   |
         |       SIGAPetDb        |
         |     (10 tabelas)       |
         +------------------------+
```

---

## Estrutura de Camadas

### 1. Camada de Apresentação (Frontend)

#### Models (`Frontend/src/app/model/`)
Interfaces TypeScript que definem a estrutura dos dados:

```typescript
// tutor.model.ts
export interface Tutor {
  tutorId: number;
  nome: string;
  telefone: string;
  email: string;
  endereco: string;
  dataCadastro?: Date;
}
```

**Models Implementados:**
- `tutor.model.ts` - Dados do tutor (dono do pet)
- `pet.model.ts` - Dados do animal
- `produto.model.ts` - Dados de produtos
- `servico-pet.model.ts` - Serviços oferecidos
- `agenda.model.ts` - Agendamentos
- `fornecedor.model.ts` - Fornecedores

#### Services (`Frontend/src/app/service/`)
Serviços HTTP que fazem requisições à API:

```typescript
@Injectable({
  providedIn: 'root'
})
export class TutorService {
  private apiUrl = `${environment.apiUrl}/Tutor`;

  findAll(): Observable<Tutor[]> {
    return this.http.get<Tutor[]>(this.apiUrl);
  }

  findById(id: number): Observable<Tutor> {
    return this.http.get<Tutor>(`${this.apiUrl}/${id}`);
  }

  create(tutor: Omit<Tutor, 'tutorId'>): Observable<Tutor> {
    return this.http.post<Tutor>(this.apiUrl, tutor);
  }

  update(id: number, tutor: Omit<Tutor, 'tutorId'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, tutor);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

**Services Implementados:**
- `tutor.service.ts`
- `pet.service.ts`
- `produto.service.ts`
- `servico-pet.service.ts`
- `agenda.service.ts`
- `fornecedor.service.ts`

---

### 2. Camada de API (Controllers)

#### Controllers (`Backend/Controllers/`)
Endpoints REST que expõem a API:

```csharp
[ApiController]
[Route("api/[controller]")]
public class TutorController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    // GET: api/Tutor
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutores()
    {
        var tutores = await _context.Tutores.AsNoTracking().ToListAsync();
        var tutoresDto = _mapper.Map<IEnumerable<TutorDto>>(tutores);
        return Ok(tutoresDto);
    }

    // GET: api/Tutor/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TutorDto>> GetTutor(int id) { }

    // POST: api/Tutor
    [HttpPost]
    public async Task<ActionResult<TutorDto>> CreateTutor([FromBody] CreateTutorDto dto) { }

    // PUT: api/Tutor/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTutor(int id, [FromBody] UpdateTutorDto dto) { }

    // DELETE: api/Tutor/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTutor(int id) { }
}
```

**Controllers Implementados:**
1. `TutorController` - 5 endpoints
2. `AnimalController` - 6 endpoints (inclui busca por tutor)
3. `ProdutoController` - 6 endpoints (inclui busca por ativos)
4. `ServicoController` - 6 endpoints (inclui busca por ativos)
5. `AgendamentoController` - 7 endpoints (inclui busca por animal e data)
6. `FornecedorController` - 5 endpoints

**Total: 35 endpoints REST**

---

### 3. Camada de Transferência (DTOs)

#### Data Transfer Objects (`Backend/DTOs/`)

**Padrão utilizado:** 3 DTOs por entidade

1. **Get DTO** - Retorno de dados (inclui ID e campos calculados)
```csharp
public class TutorDto
{
    public int TutorId { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Endereco { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

2. **Create DTO** - Criação (sem ID)
```csharp
public class CreateTutorDto
{
    [Required]
    [StringLength(120)]
    public string Nome { get; set; }
    
    [StringLength(20)]
    public string Telefone { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string Endereco { get; set; }
}
```

3. **Update DTO** - Atualização (sem ID, com validações)
```csharp
public class UpdateTutorDto
{
    [Required]
    [StringLength(120)]
    public string Nome { get; set; }
    // ... mesmos campos do Create
}
```

**DTOs Implementados:** 18 (6 entidades × 3 tipos)

---

### 4. Camada de Mapeamento (AutoMapper)

#### Profiles (`Backend/Profiles/MappingProfile.cs`)

Configuração do AutoMapper para conversão entre Models e DTOs:

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Tutor Mappings
        CreateMap<Tutor, TutorDto>();
        CreateMap<CreateTutorDto, Tutor>();
        CreateMap<UpdateTutorDto, Tutor>();

        // Animal Mappings
        CreateMap<Animal, AnimalDto>();
        CreateMap<CreateAnimalDto, Animal>();
        CreateMap<UpdateAnimalDto, Animal>();

        // ... outros mapeamentos
    }
}
```

**Configuração no Program.cs:**
```csharp
builder.Services.AddAutoMapper(typeof(MappingProfile));
```

---

### 5. Camada de Domínio (Models)

#### Entidades (`Backend/Models/`)

**Entidades Principais:**

```csharp
// Tutor.cs
public class Tutor
{
    public int TutorId { get; set; }
    
    [Required]
    [StringLength(120)]
    public string Nome { get; set; }
    
    [StringLength(20)]
    public string Telefone { get; set; }
    
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; }
    
    [StringLength(250)]
    public string Endereco { get; set; }
    
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    
    // Navigation Property
    public virtual ICollection<Animal> Animais { get; set; } = new List<Animal>();
}
```

**Entidades Implementadas:**
1. `Tutor` - Donos de pets
2. `Animal` - Pets cadastrados
3. `Produto` - Produtos em estoque
4. `Servico` - Serviços oferecidos
5. `Agendamento` - Agendamentos de serviços
6. `Fornecedor` - Fornecedores
7. `Funcionario` - Funcionários (futuro)
8. `Venda` - Vendas (futuro)
9. `ItemVenda` - Itens de vendas (futuro)
10. `RegistroProntuario` - Prontuário médico (futuro)

---

### 6. Camada de Dados (Entity Framework)

#### DbContext (`Backend/Data/AppDbContext.cs`)

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItensVenda { get; set; }
    public DbSet<RegistroProntuario> RegistrosProntuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações de relacionamentos
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Tutor)
            .WithMany(t => t.Animais)
            .HasForeignKey(a => a.TutorId);

        // ... outras configurações
    }
}
```

---

## Modelo de Dados

### Relacionamentos

```
Tutor (1) ??????< (N) Animal
                        ?
                        ?
                        ??????> (N) Agendamento (N) ?????< (1) Servico
                        ?
                        ??????> (N) RegistroProntuario


Fornecedor (1) ??????< (N) Produto


Venda (1) ??????< (N) ItemVenda ????< (0..1) Produto
                                  ????< (0..1) Servico
```

### Tabelas Principais

#### 1. Tutores
```sql
CREATE TABLE Tutores (
    TutorId INT PRIMARY KEY IDENTITY,
    Nome NVARCHAR(120) NOT NULL,
    Telefone NVARCHAR(20),
    Email NVARCHAR(150),
    Endereco NVARCHAR(250),
    DataCadastro DATETIME2 NOT NULL DEFAULT GETDATE()
)
```

#### 2. Animais
```sql
CREATE TABLE Animais (
    AnimalId INT PRIMARY KEY IDENTITY,
    TutorId INT NOT NULL,
    Nome NVARCHAR(100) NOT NULL,
    Especie NVARCHAR(50),
    Raca NVARCHAR(100),
    DataNascimento DATE,
    Sexo NVARCHAR(20),
    Pelagem NVARCHAR(100),
    Observacoes NVARCHAR(500),
    FOREIGN KEY (TutorId) REFERENCES Tutores(TutorId)
)
```

#### 3. Produtos
```sql
CREATE TABLE Produtos (
    ProdutoId INT PRIMARY KEY IDENTITY,
    Nome NVARCHAR(150) NOT NULL,
    Descricao NVARCHAR(500),
    Preco DECIMAL(10,2) NOT NULL,
    QuantidadeEstoque INT NOT NULL,
    FornecedorId INT,
    Ativo BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (FornecedorId) REFERENCES Fornecedores(FornecedorId)
)
```

#### 4. Servicos
```sql
CREATE TABLE Servicos (
    ServicoId INT PRIMARY KEY IDENTITY,
    Nome NVARCHAR(120) NOT NULL,
    Descricao NVARCHAR(500),
    Preco DECIMAL(10,2) NOT NULL,
    DuracaoMinutos INT NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1
)
```

#### 5. Agendamentos
```sql
CREATE TABLE Agendamentos (
    AgendamentoId INT PRIMARY KEY IDENTITY,
    AnimalId INT NOT NULL,
    ServicoId INT NOT NULL,
    FuncionarioId INT,
    DataHora DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pendente',
    Observacoes NVARCHAR(500),
    FOREIGN KEY (AnimalId) REFERENCES Animais(AnimalId),
    FOREIGN KEY (ServicoId) REFERENCES Servicos(ServicoId),
    FOREIGN KEY (FuncionarioId) REFERENCES Funcionarios(FuncionarioId)
)
```

---

## Configurações Importantes

### CORS (`Backend/Program.cs`)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:4201")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ...

app.UseCors("AllowAngularApp");
```

### Swagger/OpenAPI

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ...

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SIGAPetDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

---

## Fluxo de Requisição

### Exemplo: Criar um Tutor

1. **Frontend** (tutor.service.ts)
```typescript
create(tutor: Omit<Tutor, 'tutorId'>): Observable<Tutor> {
  return this.http.post<Tutor>(`${apiUrl}/Tutor`, tutor);
}
```

2. **Requisição HTTP**
```http
POST http://localhost:5000/api/Tutor
Content-Type: application/json

{
  "nome": "João Silva",
  "telefone": "(11) 98765-4321",
  "email": "joao@exemplo.com",
  "endereco": "Rua Exemplo, 123"
}
```

3. **Controller** (TutorController.cs)
```csharp
[HttpPost]
public async Task<ActionResult<TutorDto>> CreateTutor([FromBody] CreateTutorDto createTutorDto)
{
    // Validação automática via DataAnnotations
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // Mapeamento DTO -> Model
    var tutor = _mapper.Map<Tutor>(createTutorDto);

    // Persistência no banco
    _context.Tutores.Add(tutor);
    await _context.SaveChangesAsync();

    // Mapeamento Model -> DTO de retorno
    var tutorDto = _mapper.Map<TutorDto>(tutor);
    
    // Retorno HTTP 201 Created
    return CreatedAtAction(nameof(GetTutor), new { id = tutor.TutorId }, tutorDto);
}
```

4. **Resposta HTTP**
```http
HTTP/1.1 201 Created
Location: http://localhost:5000/api/Tutor/1
Content-Type: application/json

{
  "tutorId": 1,
  "nome": "João Silva",
  "telefone": "(11) 98765-4321",
  "email": "joao@exemplo.com",
  "endereco": "Rua Exemplo, 123",
  "dataCadastro": "2024-11-24T12:30:00"
}
```

---

## Padrões e Boas Práticas

### 1. Repository Pattern
- Implementado via **Entity Framework Core DbContext**
- Métodos assíncronos (`async/await`)
- `AsNoTracking()` para queries read-only

### 2. DTO Pattern
- Separação de camadas
- Validação via Data Annotations
- Evita over-posting e under-posting

### 3. Dependency Injection
- Controllers recebem dependências via construtor
- Configurado no `Program.cs`

### 4. REST API Best Practices
- Verbos HTTP corretos (GET, POST, PUT, DELETE)
- Códigos de status apropriados (200, 201, 404, 500)
- Rotas padronizadas (`/api/{controller}/{id}`)

### 5. Tratamento de Erros
```csharp
try
{
    // Lógica de negócio
}
catch (DbUpdateConcurrencyException)
{
    return Conflict("Erro de concorrência...");
}
catch (Exception ex)
{
    return StatusCode(500, $"Erro interno: {ex.Message}");
}
```

---

## Testando a API

### Swagger UI
1. Acesse: http://localhost:5000/swagger
2. Expanda um endpoint
3. Clique em "Try it out"
4. Preencha os dados
5. Clique em "Execute"
6. Veja a resposta

### cURL
```bash
# GET - Listar todos
curl -X GET "http://localhost:5000/api/Tutor" -H "accept: application/json"

# GET - Buscar por ID
curl -X GET "http://localhost:5000/api/Tutor/1" -H "accept: application/json"

# POST - Criar
curl -X POST "http://localhost:5000/api/Tutor" \
  -H "Content-Type: application/json" \
  -d '{"nome":"João Silva","telefone":"(11)98765-4321","email":"joao@exemplo.com"}'

# PUT - Atualizar
curl -X PUT "http://localhost:5000/api/Tutor/1" \
  -H "Content-Type: application/json" \
  -d '{"nome":"João Silva Updated","telefone":"(11)98765-4321"}'

# DELETE - Deletar
curl -X DELETE "http://localhost:5000/api/Tutor/1"
```

---

## Segurança

### Recomendações para Produção

1. **HTTPS**: Habilitar e forçar HTTPS
```csharp
app.UseHttpsRedirection();
```

2. **Autenticação JWT**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* config */ });
```

3. **Autorização**
```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTutor(int id) { }
```

4. **Validação de Inputs**
- Data Annotations já implementadas
- Adicionar validações customizadas se necessário

5. **SQL Injection Protection**
- Entity Framework já protege automaticamente
- Usar sempre parametrização

---

## Suporte e Documentação

- **README.md** - Documentação principal
- **Swagger** - http://localhost:5000/swagger
- **Issues** - GitHub Issues
- **Email** - contato@sigapet.com.br (exemplo)

---

**Versão da Documentação:** 1.0.0  
**Última Atualização:** Novembro 2024  
**Autor:** Equipe Edu-2de
