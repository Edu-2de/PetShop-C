# SIGA-PET - Sistema de Gestão para PetShop

SIGA-PET é uma solução completa para gestão de pet shops, com arquitetura moderna, separação entre Backend (API RESTful) e Frontend (SPA), e foco em escalabilidade, segurança e facilidade de manutenção.

---

## Sumário
- [Visão Geral](#visão-geral)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Dependências do Projeto](#dependências-do-projeto)
- [Arquitetura e Estrutura de Pastas](#arquitetura-e-estrutura-de-pastas)
- [Endpoints da API](#endpoints-da-api)
- [Regras de Negócio](#regras-de-negócio)
- [Instalação e Execução](#instalação-e-execução)
- [Configuração de Ambiente](#configuração-de-ambiente)
- [Testes](#testes)
- [Boas Práticas e Observações](#boas-práticas-e-observações)
- [Troubleshooting](#troubleshooting)
- [Licença](#licença)
- [Contato](#contato)

---

## Visão Geral

O SIGA-PET é um sistema web full-stack desenvolvido para automatizar e facilitar a gestão de pet shops. Com interface responsiva e API robusta, o sistema permite controle completo de tutores, pets, produtos, serviços, agenda e fornecedores.

### Características Principais
- Arquitetura moderna separada (Backend e Frontend)
- API RESTful com documentação Swagger
- Interface responsiva e intuitiva
- Sistema de autenticação e autorização
- Controle de permissões (Admin e Usuário)
- Dashboard administrativo completo

---

## Funcionalidades

### Gestão de Clientes
- Cadastro, edição e consulta de tutores (clientes)
- Gerenciamento de animais vinculados aos tutores, com histórico
- Relacionamento entre tutores e seus pets

### Controle de Estoque
- Controle de estoque: produtos, quantidades, preços e imagens
- Upload de múltiplas imagens por produto
- Categorização de produtos
- Gestão de fornecedores: cadastro, histórico de suprimentos

### Serviços e Agenda
- Catálogo de serviços: banho, tosa, consultas, entre outros
- Agenda inteligente: agendamentos em intervalos fixos de 30 minutos, sem conflitos
- Validação automática de horários
- Prevenção de conflitos na agenda

### Administração
- Dashboard administrativo: visão geral das principais funções
- Autenticação e controle de acesso (admin/usuário)
- Interface responsiva e intuitiva
- Relatórios e filtros de pesquisa
- Integração entre módulos (clientes, pets, produtos, serviços, agenda)
- API documentada via Swagger/OpenAPI

---

## Tecnologias Utilizadas

### Backend (API)
- **ASP.NET Core 8.0** - Framework web moderno e de alto desempenho
- **C#** - Linguagem de programação
- **Entity Framework Core (ORM)** - Mapeamento objeto-relacional
- **SQL Server** - Banco de dados (LocalDB ou instância)
- **AutoMapper** - Mapeamento automático de objetos
- **Swagger / OpenAPI** - Documentação interativa da API
- **JWT (JSON Web Tokens)** - Autenticação e autorização
- **CORS** - Configuração de Cross-Origin Resource Sharing

### Frontend (SPA)
- **Angular 20.2** - Framework para aplicações web
- **TypeScript 5.9** - Superset tipado do JavaScript
- **Bootstrap 5.3** - Framework CSS para design responsivo
- **Bootstrap Icons 1.13** - Biblioteca de ícones
- **RxJS 7.8** - Programação reativa
- **Angular HttpClient** - Comunicação com a API
- **SCSS** - Pré-processador CSS para estilos customizados
- **Guards de rota** - Proteção de rotas por autenticação/autorização
- **Interceptors HTTP** - Manipulação de requisições e respostas

---

## Dependências do Projeto

### Frontend (package.json)

#### Dependências de Produção
```json
{
  "@angular/animations": "^20.2.0",
  "@angular/common": "^20.2.0",
  "@angular/compiler": "^20.2.0",
  "@angular/core": "^20.2.0",
  "@angular/forms": "^20.2.0",
  "@angular/platform-browser": "^20.2.0",
  "@angular/router": "^20.2.0",
  "bootstrap": "^5.3.8",
  "bootstrap-icons": "^1.13.1",
  "rxjs": "~7.8.0",
  "tslib": "^2.3.0",
  "zone.js": "~0.15.0"
}
```

#### Dependências de Desenvolvimento
```json
{
  "@angular/build": "^20.2.1",
  "@angular/cli": "^20.2.1",
  "@angular/compiler-cli": "^20.2.0",
  "@types/jasmine": "~5.1.0",
  "jasmine-core": "~5.9.0",
  "json-server": "^1.0.0-beta.3",
  "karma": "~6.4.0",
  "karma-chrome-launcher": "~3.2.0",
  "karma-coverage": "~2.2.0",
  "karma-jasmine": "~5.1.0",
  "karma-jasmine-html-reporter": "~2.1.0",
  "typescript": "~5.9.2"
}
```

### Backend (.csproj)

#### Pacotes NuGet Necessários
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.0" />
```

### Instalação de Dependências

#### Frontend
```sh
cd Frontend
npm install
```

#### Backend
```sh
cd Backend
dotnet restore
```

---

## Arquitetura e Estrutura de Pastas

### Backend
```
Backend/
├── Controllers/          - Endpoints da API (REST)
├── DTOs/                 - Data Transfer Objects
├── Models/               - Entidades do domínio
├── Data/                 - DbContext e configurações do banco
├── Profiles/             - Configurações do AutoMapper
├── Enums/                - Enumerações (Status, Tipos)
├── Properties/           - Configurações de inicialização
└── appsettings.json      - Configurações da aplicação
```

### Frontend
```
Frontend/
├── src/
│   ├── app/
│   │   ├── pages/            - Componentes de páginas
│   │   ├── services/         - Serviços de comunicação com API
│   │   ├── models/           - Interfaces TypeScript
│   │   ├── guards/           - Guards de proteção de rotas
│   │   ├── interceptors/     - Interceptores HTTP
│   │   ├── app.routes.ts     - Configuração de rotas
│   │   ├── app.config.ts     - Configuração da aplicação
│   │   └── app.scss          - Estilos globais
│   ├── assets/               - Recursos estáticos
│   ├── environments/         - Configurações por ambiente
│   └── styles.scss           - Estilos globais Bootstrap
└── angular.json              - Configuração do Angular CLI
```

---

## Endpoints da API

### Autenticação
- POST /api/auth/login - Login de usuário
- POST /api/auth/register - Registro de novo usuário

### Tutores (Clientes)
- GET /api/tutores - Lista todos os tutores
- GET /api/tutores/{id} - Busca tutor por ID
- POST /api/tutores - Cria novo tutor
- PUT /api/tutores/{id} - Atualiza tutor
- DELETE /api/tutores/{id} - Remove tutor

### Animais (Pets)
- GET /api/animais - Lista todos os animais
- GET /api/animais/cliente/{clienteId} - Busca animais por tutor
- POST /api/animais - Cadastra novo animal
- PUT /api/animais/{id} - Atualiza animal
- DELETE /api/animais/{id} - Remove animal

### Produtos
- GET /api/produtos - Lista todos os produtos
- GET /api/produtos/{id} - Busca produto por ID
- POST /api/produtos - Cria novo produto
- PUT /api/produtos/{id} - Atualiza produto
- DELETE /api/produtos/{id} - Remove produto

### Imagens de Produtos
- POST /api/produtoimagem - Upload de imagem
- DELETE /api/produtoimagem/{id} - Remove imagem

### Categorias
- GET /api/categorias - Lista todas as categorias
- POST /api/categorias - Cria nova categoria
- PUT /api/categorias/{id} - Atualiza categoria
- DELETE /api/categorias/{id} - Remove categoria

### Agendamentos
- GET /api/agendamentos - Lista todos os agendamentos
- GET /api/agendamentos/data/{data} - Busca por data
- POST /api/agendamentos - Cria novo agendamento
- PUT /api/agendamentos/{id} - Atualiza agendamento
- DELETE /api/agendamentos/{id} - Cancela agendamento

### Serviços
- GET /api/servicos - Lista todos os serviços
- POST /api/servicos - Cria novo serviço
- PUT /api/servicos/{id} - Atualiza serviço
- DELETE /api/servicos/{id} - Remove serviço

### Fornecedores
- GET /api/fornecedores - Lista todos os fornecedores
- POST /api/fornecedores - Cria novo fornecedor
- PUT /api/fornecedores/{id} - Atualiza fornecedor
- DELETE /api/fornecedores/{id} - Remove fornecedor

### Funcionários
- GET /api/funcionarios - Lista todos os funcionários
- POST /api/funcionarios - Cria novo funcionário
- PUT /api/funcionarios/{id} - Atualiza funcionário
- DELETE /api/funcionarios/{id} - Remove funcionário

### Vendas
- GET /api/vendas - Lista todas as vendas
- POST /api/vendas - Registra nova venda

---

## Regras de Negócio

### Agendamentos
- Intervalos fixos de 30 minutos
- O frontend utiliza inputs com `step="1800"` (30 minutos em segundos)
- Horários inseridos manualmente são ajustados automaticamente
- A API valida o horário e impede conflitos na agenda
- Não é permitido agendar no mesmo horário para o mesmo funcionário

### Produtos
- Controle de estoque com validação de quantidade e preço
- Produtos podem ter múltiplas imagens
- Produtos inativos não aparecem para usuários comuns
- Validação de preço mínimo e quantidade

### Tutores e Pets
- Relacionamento obrigatório entre tutores e pets
- Um tutor pode ter múltiplos pets
- Cada pet pertence a apenas um tutor

### Autenticação e Autorização
- Autenticação JWT para proteger rotas sensíveis
- Permissões diferenciadas para admin e usuário comum
- Tokens com expiração configurável
- Rotas administrativas protegidas por guard

### Validações
- Cadastro de produtos e serviços com imagens
- Filtros e pesquisa em todas as listas
- Validação de dados obrigatórios em todos os formulários
- Proteção contra duplicidade de registros
- Validação de CPF/CNPJ
- Validação de e-mail

---

## Instalação e Execução

### Pré-requisitos
- **.NET SDK 8.0** ou superior
- **Node.js 20+** e NPM
- **SQL Server** (LocalDB ou instância completa)
- **Git** (para clonar o repositório)

### 1. Clonar o Repositório
```sh
git clone https://github.com/Edu-2de/PetShop-C.git
cd PetShop-C
```

### 2. Configuração do Banco de Dados

#### Atualizar a string de conexão (opcional)
Edite o arquivo `Backend/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SigaPetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

#### Aplicar migrations
```sh
cd Backend
dotnet ef database update
```

Se o comando acima falhar, instale a ferramenta EF:
```sh
dotnet tool install --global dotnet-ef
```

### 3. Instalação de Dependências

#### Backend
```sh
cd Backend
dotnet restore
dotnet build
```

#### Frontend
```sh
cd Frontend
npm install
```

### 4. Execução Separada

#### Backend
```sh
cd Backend
dotnet run --launch-profile http
```
- **API:** http://localhost:5000
- **Swagger:** http://localhost:5000/swagger

#### Frontend
```sh
cd Frontend
npm start
```
- **Aplicação:** http://localhost:4200

### 5. Execução Simultânea (Recomendado)

#### Windows (PowerShell)
```powershell
.\start-dev.ps1
```

#### Windows (CMD)
```cmd
.\start-dev.bat
```

#### Linux/Mac
Crie um arquivo `start-dev.sh`:
```sh
#!/bin/bash
echo "Iniciando o SIGA-PET..."

cd Backend && dotnet run --launch-profile http &
sleep 5

cd ../Frontend && npm start &

echo "Sistema iniciando. Acesse http://localhost:4200"
wait
```

Execute:
```sh
chmod +x start-dev.sh
./start-dev.sh
```

### 6. Parar a Aplicação

#### Windows (PowerShell)
```powershell
.\stop-dev.ps1
```

#### Windows (CMD)
```cmd
.\stop-dev.bat
```

---

## Configuração de Ambiente

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Sua string de conexão aqui"
  },
  "Jwt": {
    "Key": "SuaChaveSecretaAqui",
    "Issuer": "SigaPetAPI",
    "Audience": "SigaPetClient",
    "ExpireMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Frontend (environment.ts)
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

### Variáveis de Ambiente
- **Desenvolvimento:** Use `environment.ts`
- **Produção:** Use `environment.prod.ts`
- Mantenha variáveis sensíveis fora do versionamento

---

## Testes

### Backend
- Testes via **Swagger UI:** http://localhost:5000/swagger
- Testes via **Postman** ou **Insomnia**
- Endpoints protegidos requerem token JWT no header:
  ```
  Authorization: Bearer {seu-token-aqui}
  ```

### Frontend
```sh
cd Frontend
npm test
```

### Testes Recomendados
- Cadastro de tutores e pets
- Upload de imagens de produtos
- Criação de agendamentos
- Autenticação e autorização
- Fluxo completo de venda

---

## Boas Práticas e Observações

### Desenvolvimento
- O SIGA-PET foi desenvolvido com foco em modularidade, clareza, escalabilidade e boas práticas
- O código segue padrões de nomenclatura, organização e separação de responsabilidades
- Commits seguem convenções (veja GUIA-COMMIT.md)

### Segurança
- Autenticação JWT implementada
- Rotas administrativas protegidas
- Validação de dados em frontend e backend
- Proteção contra SQL Injection (EF Core)
- CORS configurado adequadamente

### Performance
- Lazy loading de módulos no Angular
- Paginação de listas grandes
- Otimização de consultas no banco
- Cache de imagens

### Deploy
- O projeto está pronto para deploy em ambiente cloud ou local
- Recomendado usar variáveis de ambiente para produção
- Configurar SSL/TLS para produção
- Usar banco de dados SQL Server em produção

---

## Troubleshooting

### Erro: "dotnet ef não reconhecido"
```sh
dotnet tool install --global dotnet-ef
```

### Erro: "Cannot connect to database"
- Verifique se o SQL Server está rodando
- Confirme a string de conexão no `appsettings.json`
- Execute `dotnet ef database update`

### Erro: "Port 5000 already in use"
- Altere a porta em `Properties/launchSettings.json`
- Ou pare o processo que está usando a porta

### Erro: "npm install failed"
- Limpe o cache: `npm cache clean --force`
- Delete `node_modules` e `package-lock.json`
- Execute `npm install` novamente

### Frontend não conecta com Backend
- Verifique se a API está rodando
- Confirme a URL em `environment.ts`
- Verifique o CORS no backend

Para mais detalhes, consulte `TROUBLESHOOTING.md`

---

## Licença

Este projeto é distribuído sob a licença MIT. Consulte o arquivo LICENSE para mais detalhes.

---

## Contato

Para dúvidas, sugestões ou contribuições:
- Abra uma **Issue** no GitHub
- Envie um **Pull Request**
- Entre em contato via e-mail

---

## Créditos e Referências

- [Documentação oficial do ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- [Documentação oficial do Angular](https://angular.io/docs)
- [Documentação do Bootstrap](https://getbootstrap.com/docs/5.3/getting-started/introduction/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [JWT Authentication](https://jwt.io/)
- Exemplos de arquitetura em camadas e boas práticas de desenvolvimento

---

## Histórico de Versões

### v1.0 (Inicial)
- Estrutura inicial do projeto
- Cadastro de clientes, pets, produtos, serviços
- Agenda e fornecedores
- CRUD completo de todas as entidades

### v1.1 (Autenticação)
- Implementação de autenticação JWT
- Dashboard administrativo
- Melhorias de UI/UX
- Guards de proteção de rotas

### v1.2 (Melhorias)
- Adição de testes automatizados
- Documentação Swagger completa
- Ajustes de segurança
- Upload de múltiplas imagens

### v1.3 (Atual)
- Angular 20 atualizado
- Bootstrap 5.3
- Melhorias de performance
- Documentação completa

---

## Observações Finais

O SIGA-PET foi projetado para ser modular, escalável e simples de manter, seguindo boas práticas de desenvolvimento tanto no backend quanto no frontend. O sistema pode ser expandido para novas funcionalidades conforme a necessidade do negócio.

### Próximas Funcionalidades (Roadmap)
- Relatórios avançados em PDF
- Integração com pagamento online
- Notificações por e-mail/SMS
- Sistema de fidelidade para clientes
- Aplicativo mobile (React Native)
- Dashboard com gráficos e estatísticas
- Prontuário eletrônico veterinário
- Integração com NFe

### Contribuindo
Contribuições são bem-vindas! Por favor:
1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

**Desenvolvido com dedicação para facilitar a gestão de pet shops.**






