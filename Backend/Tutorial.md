# SIGA-PET - Sistema de Gestão para Pet Shop

Sistema de gestão completo para pet shop desenvolvido em .NET 8 com Entity Framework Core, seguindo o padrão MVC e arquitetura em camadas.

## Estrutura do Projeto

### Controllers/
Contém os controladores da API que definem os endpoints HTTP. Cada controller é responsável por uma entidade específica e implementa operações CRUD (Create, Read, Update, Delete).

**Exemplo:** TutorController.cs
- Define rotas como GET /api/Tutor, POST /api/Tutor/{id}
- Recebe requisições HTTP e retorna respostas JSON
- Utiliza DTOs para entrada e saída de dados
- Implementa validação de modelo e tratamento de erros
- Injeta dependências como DbContext e AutoMapper

### Models/
Define as entidades do domínio que representam as tabelas do banco de dados. Cada modelo contém propriedades que correspondem às colunas da tabela e relacionamentos entre entidades.

**Funcionalidades:**
- Propriedades com tipos de dados específicos (int, string, DateTime)
- Data Annotations para validação (Required, StringLength, EmailAddress)
- Navigation Properties para relacionamentos (ICollection, ForeignKey)
- Configuração de chaves primárias e estrangeiras

**Entidades implementadas:**
- Tutor: donos dos pets com dados pessoais
- Animal: pets cadastrados vinculados aos tutores
- Funcionario: colaboradores do estabelecimento
- Servico: serviços oferecidos (consulta, banho, tosa)
- Agendamento: agendamentos de serviços para animais
- RegistroProntuario: histórico médico dos animais
- Produto: itens do estoque
- Fornecedor: fornecedores dos produtos
- Venda: vendas realizadas
- ItemVenda: detalhamento de itens por venda

### Data/
Contém o contexto do Entity Framework (AppDbContext) que gerencia a conexão com o banco de dados e define como as entidades se relacionam.

**AppDbContext.cs:**
- Herda de DbContext do Entity Framework Core
- Define DbSets para cada entidade (mapeamento objeto-relacional)
- Configura relacionamentos entre tabelas (OnModelCreating)
- Define comportamentos de exclusão (Cascade, SetNull, Restrict)
- Configura índices para otimização de consultas
- Mapeia nomes de tabelas personalizados (ToTable)

### DTOs/ (Data Transfer Objects)
Objetos de transferência de dados que definem a estrutura dos dados trafegados entre cliente e servidor. Servem para isolar o modelo interno da API dos dados expostos externamente.

**Tipos de DTOs:**
- CreateDto: dados necessários para criar um novo registro
- UpdateDto: dados permitidos para atualização
- ResponseDto: dados retornados ao cliente
- Não expõem propriedades internas como IDs de relacionamento
- Contêm validações específicas para entrada de dados
- Evitam over-posting e under-posting de dados

### Profiles/
Configurações do AutoMapper que definem como converter entre Models e DTOs automaticamente. O AutoMapper elimina código repetitivo de mapeamento manual.

**MappingProfile.cs:**
- Herda de Profile do AutoMapper
- Define mapeamentos bidirecionais (CreateMap)
- Configura transformações específicas (ForMember)
- Ignora propriedades que não devem ser mapeadas
- Define valores padrão para campos como DataCadastro

### Migrations/
Scripts gerados automaticamente pelo Entity Framework que criam e modificam a estrutura do banco de dados. Cada migration representa uma versão do schema.

**Funcionalidades:**
- Versionamento do banco de dados
- Criação de tabelas, colunas, índices e relacionamentos
- Permite rollback para versões anteriores
- Sincronização entre ambientes (desenvolvimento, teste, produção)
- Histórico de mudanças no schema

### Views/
Pasta preparada para futuras views do MVC caso seja implementado um front-end server-side com Razor Pages.

## Arquitetura e Padrões

### Injeção de Dependência
O projeto utiliza o container de DI nativo do .NET Core configurado no Program.cs:
- DbContext é injetado nos controllers
- AutoMapper é registrado como serviço
- Permite testabilidade e baixo acoplamento

### Repository Pattern (Implícito)
Embora não use repositories explícitos, o DbContext atua como Unit of Work e os DbSets como repositories, fornecendo métodos para consulta e persistência.

### API RESTful
Segue convenções REST para endpoints:
- GET para consultas
- POST para criação
- PUT para atualização completa
- DELETE para exclusão
- Códigos de status HTTP apropriados

### Validação em Camadas
- Data Annotations nos Models e DTOs
- ModelState validation nos Controllers
- Constraints no banco via Entity Framework

## Configuração e Execução

### Pré-requisitos
- .NET 8 SDK
- SQL Server LocalDB ou SQL Server
- Entity Framework CLI tools

### Comandos de Setup
```bash
dotnet restore                          # Restaura pacotes NuGet
dotnet ef migrations add InitialCreate  # Cria migration inicial
dotnet ef database update              # Aplica migrations ao banco
dotnet run                             # Executa a aplicação