# ?? SIGA-PET - Sistema Integrado de Gestão para PetShop

Sistema completo de gestão para PetShop com Backend em ASP.NET Core e Frontend em Angular.

## ?? Pré-requisitos

- **.NET 8.0 SDK** ou superior
- **Node.js 20.15+** (recomendado 20.19+ ou 22.12+)
- **npm**
- **SQL Server** ou **LocalDB**
- **Angular CLI**: `npm install -g @angular/cli`

> ?? **Nota sobre Node.js**: O projeto funciona com Node.js v20.15.1, mas você verá warnings recomendando v20.19+. Estes warnings **podem ser ignorados** - a aplicação funciona normalmente.

## ?? Início Rápido

### Opção 1: Rodar Tudo de Uma Vez (Recomendado)

Execute um dos scripts na raiz do projeto:

**Windows (PowerShell):**
```powershell
.\start-dev.ps1
```

**Windows (CMD):**
```cmd
start-dev.bat
```

> ?? **Os scripts criam automaticamente a pasta `wwwroot` e verificam a estrutura do projeto**

### Opção 2: Rodar Manualmente

**Backend:**
```bash
cd Backend
dotnet restore
dotnet run --launch-profile https
```

**Frontend (em outro terminal):**
```bash
cd Frontend
npm install
npm start
```

## ?? URLs da Aplicação

- **Frontend**: http://localhost:4200
- **Backend API**: https://localhost:7000
- **Swagger (Documentação da API)**: https://localhost:7000/swagger

## ?? Parar os Servidores

**Windows (PowerShell):**
```powershell
.\stop-dev.ps1
```

**Windows (CMD):**
```cmd
stop-dev.bat
```

## ?? Estrutura do Projeto

```
PetShop-C/
??? Backend/                 # API ASP.NET Core
?   ??? Controllers/         # Endpoints da API
?   ??? Models/             # Entidades do banco
?   ??? DTOs/               # Objetos de transferência
?   ??? Data/               # Contexto do EF Core
?   ??? Profiles/           # Mapeamentos AutoMapper
?   ??? Migrations/         # Migrações do banco
?
??? Frontend/               # Aplicação Angular
?   ??? src/
?   ?   ??? app/
?   ?   ?   ??? model/      # Interfaces TypeScript
?   ?   ?   ??? service/    # Serviços HTTP
?   ?   ?   ??? pages/      # Componentes de páginas
?   ?   ??? environments/   # Configurações de ambiente
?
??? start-dev.ps1           # Script PowerShell para iniciar
??? start-dev.bat           # Script CMD para iniciar
??? stop-dev.ps1            # Script PowerShell para parar
??? stop-dev.bat            # Script CMD para parar
```

## ??? Banco de Dados

O projeto usa **SQL Server LocalDB** por padrão.

### Criar/Atualizar o Banco de Dados

```bash
cd Backend
dotnet ef database update
```

### Criar uma Nova Migration

```bash
cd Backend
dotnet ef migrations add NomeDaMigracao
dotnet ef database update
```

## ?? Endpoints da API

### Tutores
- `GET /api/Tutor` - Listar todos
- `GET /api/Tutor/{id}` - Buscar por ID
- `POST /api/Tutor` - Criar novo
- `PUT /api/Tutor/{id}` - Atualizar
- `DELETE /api/Tutor/{id}` - Deletar

### Animais (Pets)
- `GET /api/Animal` - Listar todos
- `GET /api/Animal/{id}` - Buscar por ID
- `GET /api/Animal/tutor/{tutorId}` - Buscar por tutor
- `POST /api/Animal` - Criar novo
- `PUT /api/Animal/{id}` - Atualizar
- `DELETE /api/Animal/{id}` - Deletar

### Produtos
- `GET /api/Produto` - Listar todos
- `GET /api/Produto/{id}` - Buscar por ID
- `GET /api/Produto/ativos` - Listar ativos
- `POST /api/Produto` - Criar novo
- `PUT /api/Produto/{id}` - Atualizar
- `DELETE /api/Produto/{id}` - Deletar

### Serviços
- `GET /api/Servico` - Listar todos
- `GET /api/Servico/{id}` - Buscar por ID
- `GET /api/Servico/ativos` - Listar ativos
- `POST /api/Servico` - Criar novo
- `PUT /api/Servico/{id}` - Atualizar
- `DELETE /api/Servico/{id}` - Deletar

### Agendamentos
- `GET /api/Agendamento` - Listar todos
- `GET /api/Agendamento/{id}` - Buscar por ID
- `GET /api/Agendamento/animal/{animalId}` - Buscar por animal
- `GET /api/Agendamento/data/{data}` - Buscar por data
- `POST /api/Agendamento` - Criar novo
- `PUT /api/Agendamento/{id}` - Atualizar
- `DELETE /api/Agendamento/{id}` - Deletar

### Fornecedores
- `GET /api/Fornecedor` - Listar todos
- `GET /api/Fornecedor/{id}` - Buscar por ID
- `POST /api/Fornecedor` - Criar novo
- `PUT /api/Fornecedor/{id}` - Atualizar
- `DELETE /api/Fornecedor/{id}` - Deletar

## ?? Configuração

### Backend - appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SIGAPetDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Frontend - environment.ts

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7000/api'
};
```

## ??? Tecnologias Utilizadas

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server / LocalDB
- AutoMapper
- Swagger/OpenAPI

### Frontend
- Angular 17+
- TypeScript
- RxJS
- HttpClient

## ?? Funcionalidades

- ? **Gestão de Tutores** - Cadastro de donos de pets
- ? **Gestão de Animais** - Registro de pets com informações detalhadas
- ? **Gestão de Produtos** - Controle de estoque
- ? **Gestão de Serviços** - Banho, tosa, consultas, etc.
- ? **Agendamentos** - Sistema de agendamento de serviços
- ? **Gestão de Fornecedores** - Cadastro de fornecedores

## ?? Avisos Esperados (Podem Ignorar)

### Warnings do Node.js
```
npm warn EBADENGINE Unsupported engine
```
**Status:** ?? Normal - Aplicação funciona perfeitamente

### Vulnerabilidades NPM
```
2 moderate severity vulnerabilities
```
**Status:** ?? Baixo risco em desenvolvimento (relacionadas ao Vite)

### Certificado SSL
```
NET::ERR_CERT_AUTHORITY_INVALID
```
**Status:** ?? Normal em desenvolvimento local - Aceite o aviso no navegador

## ?? Problemas Comuns

### Erro: "wwwroot não encontrado"
**Solução:** Os scripts criam automaticamente. Se erro persistir:
```powershell
mkdir Backend\wwwroot
```

### Erro: "Porta já em uso"
**Solução:**
```powershell
.\stop-dev.ps1
.\start-dev.ps1
```

### Frontend não carrega
**Solução:** Aguarde 10-30 segundos após iniciar - Angular precisa compilar

> ?? **Mais soluções**: Consulte [TROUBLESHOOTING.md](TROUBLESHOOTING.md) para lista completa de problemas e soluções

## ?? Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## ?? Licença

Este projeto está sob a licença MIT.

## ?? Autores

- Equipe Edu-2de

## ?? Documentação Adicional

- **[PRIMEIRO-USO.md](PRIMEIRO-USO.md)** - Guia passo a passo para iniciantes
- **[GUIA-RAPIDO.md](GUIA-RAPIDO.md)** - Referência rápida de comandos
- **[TROUBLESHOOTING.md](TROUBLESHOOTING.md)** - Soluções para problemas comuns
- **[CHECKLIST.md](CHECKLIST.md)** - Verificação de funcionalidades
- **[ARQUITETURA.md](ARQUITETURA.md)** - Arquitetura técnica do sistema
- **[CORRECOES.md](CORRECOES.md)** - Histórico de correções aplicadas

## ?? Suporte

Para suporte:
1. Consulte [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
2. Verifique a documentação acima
3. Abra uma issue no GitHub

---

**Status:** ? **100% Funcional**
**Última Atualização:** Hoje
**Versão:** 1.0.0
