# SIGA-PET - Sistema de Gestão para PetShop

O SIGA-PET é um sistema de gerenciamento para pet shops, construído com **ASP.NET Core 8.0** para o backend e **Angular 17** para o frontend.

## 🚀 Funcionalidades

- **Gestão de Clientes:** Cadastro e consulta de tutores de animais.
- **Gestão de Animais:** Registro de pets, associando-os aos seus tutores.
- **Controle de Estoque:** Cadastro e gerenciamento de produtos.
- **Serviços:** Definição de serviços oferecidos, como banho e tosa.
- **Agenda:** Agendamento de serviços para os pets com horários fixos.
- **Fornecedores:** Cadastro de fornecedores de produtos.
- **Painel Administrativo:** Dashboard com visão geral do negócio.

---

## 🛠 Tecnologias

| Categoria | Tecnologia |
|-----------|------------|
| Backend | ASP.NET Core 8.0, Entity Framework Core, AutoMapper |
| Frontend | Angular 17, TypeScript, RxJS, Bootstrap 5 |
| Banco de Dados | SQL Server (LocalDB) |
| Documentação API | Swagger / OpenAPI |

---

## 🏗 Arquitetura do Sistema

### Visão Geral da Estrutura

           FRONTEND (Angular 17+)
               http://localhost:4200
+-----------------------------------------------------+ | +----------+ +----------+ +----------+ | | | Models | | Services | | UI | | | | (Types) | | (HTTP) | | (Pages) | | | +----------+ +----------+ +----------+ | +-----------------------------------------------------+ | HTTP/REST API | JSON | +-----------------------------------------------------+ | BACKEND (ASP.NET Core 8.0) | | http://localhost:5000 | | | | +---------------+ +-----------------+ | | | Controllers | | DTOs | | | | (API Endp) | | (Data Transfer) | | | +---------------+ +-----------------+ | | | | | | +---------------+ +-----------------+ | | | AutoMapper | | Models | | | +---------------+ +-----------------+ | | | | | +--------------------+ | | | EF Core | | | | DbContext | | | +--------------------+ | +-----------------------------------------------------+ | ADO.NET | +------------------------+ | SQL Server LocalDB | | SIGAPetDb | +------------------------+


### Camadas do Backend

1.  **Controllers (`/Controllers`):**
    * Endpoints RESTful (GET, POST, PUT, DELETE).
    * Responsáveis por receber requisições, validar via `ModelState` e chamar o banco de dados.
    * Exemplo: `AgendamentoController`, `ProdutoController`.

2.  **Models (`/Models`):**
    * Representam as tabelas do banco de dados.
    * Usam *Data Annotations* (`[Required]`, `[StringLength]`) para validação e configuração do EF Core.
    * Entidades: `Tutor`, `Animal`, `Produto`, `Servico`, `Agendamento`, etc.

3.  **DTOs (`/DTOs`):**
    * Objetos de Transferência de Dados para desacoplar a API do banco.
    * Padrão usado: `CreateDto` (entrada), `UpdateDto` (atualização), `Dto` (leitura).

4.  **Data (`/Data`):**
    * `AppDbContext`: Gerencia a conexão com o SQL Server e as relações entre tabelas (Foreign Keys, One-to-Many).

5.  **Profiles (`/Profiles`):**
    * Configurações do **AutoMapper** para converter automaticamente entre Models e DTOs.

### Camadas do Frontend

1.  **Services (`/service`):**
    * Classes injetáveis (`@Injectable`) que comunicam com a API via `HttpClient`.
    * Gerenciam a lógica de chamadas HTTP (CRUD).

2.  **Pages/Components (`/pages`):**
    * Componentes visuais (HTML/SCSS/TS).
    * Organizados por funcionalidade: `agenda-form`, `produto-list`, etc.

3.  **Models (`/model`):**
    * Interfaces TypeScript que espelham os DTOs do backend para tipagem forte.

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20.x](https://nodejs.org/)
- SQL Server (ou LocalDB)

### Passos

1. **Backend (API):**
   ```bash
   cd Backend
   dotnet ef database update  # Cria o banco de dados
   dotnet run --launch-profile http
Acesse o Swagger em: http://localhost:5000/swagger

Frontend (Angular):

Bash

cd Frontend
npm install
npm start
Acesse a aplicação em: http://localhost:4200

Scripts de Atalho
Windows (PowerShell): Execute .\start-dev.ps1 na raiz.

Windows (CMD): Execute start-dev.bat na raiz.

---

## 🔐 Segurança e Padrões

JWT (JSON Web Tokens): Utilizado para autenticação de usuários.

CORS: Configurado para permitir requisições apenas da origem do frontend (http://localhost:4200).

Entity Framework: Uso de Migrations para versionamento do banco de dados.

Injeção de Dependência: Nativa do .NET Core e do Angular.

Versão: 1.0.0 Licença: MIT
