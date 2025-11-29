# SIGA-PET - Sistema de Gestão para PetShop

O SIGA-PET é um sistema de gerenciamento para pet shops, construído com **ASP.NET Core 8.0** para o backend e **Angular 17** para o frontend.

## ? Funcionalidades

- **Gestão de Clientes:** Cadastro e consulta de tutores de animais.
- **Gestão de Animais:** Registro de pets, associando-os aos seus tutores.
- **Controle de Estoque:** Cadastro e gerenciamento de produtos.
- **Serviços:** Definição de serviços oferecidos, como banho e tosa.
- **Agenda:** Agendamento de serviços para os pets.
- **Fornecedores:** Cadastro de fornecedores de produtos.

## Tecnologias

| Categoria | Tecnologia |
|-----------|------------|
| Backend | ASP.NET Core 8.0, Entity Framework Core, AutoMapper |
| Frontend | Angular 17, TypeScript, RxJS, Bootstrap 5 |
| Banco de Dados | SQL Server (LocalDB) |
| Documentação API | Swagger / OpenAPI |

## Começando

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20.x](https://nodejs.org/)
- SQL Server (ou LocalDB, que já vem com o Visual Studio)

### Passos para Instalação

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/Edu-2de/PetShop-C.git
   cd PetShop-C
   ```

2. **Configure o banco de dados:**
   O Entity Framework Core criará o banco `SIGAPetDb` automaticamente.
   ```bash
   cd Backend
   dotnet ef database update
   cd ..
   ```

3. **Inicie a aplicação (Backend e Frontend):**
   Use o script para sua plataforma.
   ```powershell
   # No PowerShell
   .\start-dev.ps1
   ```
   ```cmd
   # No CMD
   start-dev.bat
   ```

4. **Acesse as aplicações:**
   - **Frontend:** [http://localhost:4200](http://localhost:4200)
   - **API (Swagger):** [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Rotas do Frontend

A aplicação Angular possui as seguintes rotas principais:

| Rota | Descrição |
|------|-----------|
| `/` | Dashboard (página inicial) |
| `/tutores` | Lista de tutores |
| `/tutores/novo` | Formulário para criar novo tutor |
| `/tutores/editar/:id` | Formulário para editar um tutor |
| `/pets` | Lista de pets |
| `/pets/novo` | Formulário para criar novo pet |
| `/pets/editar/:id` | Formulário para editar um pet |
| `/produtos` | Lista de produtos |
| `/produtos/novo` | Formulário para criar novo produto |
| `/produtos/editar/:id` | Formulário para editar um produto |
| `/servicos` | Lista de serviços |
| `/servicos/novo` | Formulário para criar novo serviço |
| `/servicos/editar/:id` | Formulário para editar um serviço |
| `/fornecedores` | Lista de fornecedores |
| `/fornecedores/novo` | Formulário para criar novo fornecedor |
| `/fornecedores/editar/:id` | Formulário para editar um fornecedor |
| `/agenda` | Visualização da agenda |
| `/agenda/novo` | Formulário para criar novo agendamento |
| `/agenda/editar/:id` | Formulário para editar um agendamento |

## Endpoints da API

A API segue o padrão REST. A documentação completa está disponível via Swagger.

| Entidade | Endpoints |
|------------|-----------|
| **Tutor** | `GET`, `GET/{id}`, `GET/search?name={name}`, `POST`, `PUT/{id}`, `DELETE/{id}` |
| **Animal** | `GET`, `GET/{id}`, `GET/tutor/{tutorId}`, `GET/search?name={name}`, `POST`, `PUT/{id}`, `DELETE/{id}` |
| **Produto** | `GET`, `GET/{id}`, `GET/ativos`, `GET/search?name={name}`, `POST`, `PUT/{id}`, `DELETE/{id}` |
| **Serviço** | `GET`, `GET/{id}`, `GET/ativos`, `GET/search?name={name}`, `POST`, `PUT/{id}`, `DELETE/{id}` |
| **Fornecedor**| `GET`, `GET/{id}`, `GET/search?name={name}`, `POST`, `PUT/{id}`, `DELETE/{id}` |
| **Agendamento**| `GET`, `GET/{id}`, `GET/animal/{animalId}`, `GET/data/{data}`, `POST`, `PUT/{id}`, `DELETE/{id}` |

## Contribuição

Contribuições são bem-vindas! Siga os passos:
1. Faça um **Fork** do projeto.
2. Crie uma nova branch (`git checkout -b feature/sua-feature`).
3. Faça commit das suas alterações (`git commit -m 'Adiciona nova feature'`).
4. Envie para a sua branch (`git push origin feature/sua-feature`).
5. Abra um **Pull Request**.

## Licença

Este projeto está sob a licença MIT.
