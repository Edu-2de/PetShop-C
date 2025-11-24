# ?? SIGA-PET - Sistema Integrado de Gestão para PetShop

Sistema completo de gestão para PetShop desenvolvido com **ASP.NET Core 8.0** (Backend) e **Angular 17+** (Frontend).

---

## ?? Tecnologias

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core** - ORM
- **SQL Server / LocalDB** - Banco de dados
- **AutoMapper** - Mapeamento objeto-objeto
- **Swagger / OpenAPI** - Documentação da API

### Frontend
- **Angular 17+** - Framework SPA
- **TypeScript** - Linguagem
- **RxJS** - Programação reativa
- **Bootstrap 5** - UI Framework

---

## ?? Início Rápido

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20.15+](https://nodejs.org/)
- SQL Server ou LocalDB

### 1. Clonar o Repositório
```bash
git clone https://github.com/Edu-2de/PetShop-C.git
cd PetShop-C
```

### 2. Configurar Banco de Dados
```bash
cd Backend
dotnet ef database update
cd ..
```

### 3. Iniciar Aplicação
```powershell
# PowerShell
.\start-dev.ps1
```

```cmd
# CMD
start-dev.bat
```

### 4. Acessar
- **Backend (Swagger)**: http://localhost:5000/swagger
- **Frontend**: http://localhost:4200

---

## ?? Estrutura do Projeto

```
PetShop-C/
??? Backend/                    # API ASP.NET Core
?   ??? Controllers/            # 6 Controllers (Tutor, Animal, Produto, etc)
?   ??? Models/                 # 10 Entidades do banco
?   ??? DTOs/                   # 18 Data Transfer Objects
?   ??? Data/                   # DbContext EF Core
?   ??? Profiles/               # Configuração AutoMapper
?   ??? Migrations/             # Migrations do banco
?
??? Frontend/                   # Aplicação Angular
?   ??? src/app/
?   ?   ??? model/              # 6 Interfaces TypeScript
?   ?   ??? service/            # 6 Services HTTP
?   ?   ??? environments/       # Configuração de ambiente
?   ??? ...
?
??? start-dev.ps1              # Script inicialização (PowerShell)
??? start-dev.bat              # Script inicialização (CMD)
??? stop-dev.ps1               # Script parar servidores
??? stop-dev.bat               # Script parar servidores
??? README.md                  # Este arquivo
??? ARQUITETURA.md             # Documentação técnica detalhada
```

---

## ??? Banco de Dados

### Entidades (10 tabelas)
1. **Tutor** - Donos de pets
2. **Animal** - Pets cadastrados
3. **Produto** - Produtos do estoque
4. **Servico** - Serviços oferecidos (banho, tosa, etc)
5. **Agendamento** - Agendamentos de serviços
6. **Fornecedor** - Fornecedores
7. **Funcionario** - Funcionários (futuro)
8. **Venda** - Vendas (futuro)
9. **ItemVenda** - Itens de venda (futuro)
10. **RegistroProntuario** - Prontuário médico (futuro)

### Comandos Úteis
```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigracao

# Atualizar banco
dotnet ef database update

# Reverter migration
dotnet ef database update PreviousMigrationName

# Limpar e recriar banco
dotnet ef database drop -f
dotnet ef database update
```

---

## ?? API Endpoints

### Tutores (`/api/Tutor`)
- `GET /api/Tutor` - Listar todos
- `GET /api/Tutor/{id}` - Buscar por ID
- `POST /api/Tutor` - Criar
- `PUT /api/Tutor/{id}` - Atualizar
- `DELETE /api/Tutor/{id}` - Deletar

### Animais (`/api/Animal`)
- `GET /api/Animal` - Listar todos
- `GET /api/Animal/{id}` - Buscar por ID
- `GET /api/Animal/tutor/{tutorId}` - Buscar por tutor
- `POST /api/Animal` - Criar
- `PUT /api/Animal/{id}` - Atualizar
- `DELETE /api/Animal/{id}` - Deletar

### Produtos (`/api/Produto`)
- `GET /api/Produto` - Listar todos
- `GET /api/Produto/{id}` - Buscar por ID
- `GET /api/Produto/ativos` - Listar apenas ativos
- `POST /api/Produto` - Criar
- `PUT /api/Produto/{id}` - Atualizar
- `DELETE /api/Produto/{id}` - Deletar

### Serviços (`/api/Servico`)
- `GET /api/Servico` - Listar todos
- `GET /api/Servico/{id}` - Buscar por ID
- `GET /api/Servico/ativos` - Listar apenas ativos
- `POST /api/Servico` - Criar
- `PUT /api/Servico/{id}` - Atualizar
- `DELETE /api/Servico/{id}` - Deletar

### Agendamentos (`/api/Agendamento`)
- `GET /api/Agendamento` - Listar todos
- `GET /api/Agendamento/{id}` - Buscar por ID
- `GET /api/Agendamento/animal/{animalId}` - Buscar por animal
- `GET /api/Agendamento/data/{data}` - Buscar por data
- `POST /api/Agendamento` - Criar
- `PUT /api/Agendamento/{id}` - Atualizar
- `DELETE /api/Agendamento/{id}` - Deletar

### Fornecedores (`/api/Fornecedor`)
- `GET /api/Fornecedor` - Listar todos
- `GET /api/Fornecedor/{id}` - Buscar por ID
- `POST /api/Fornecedor` - Criar
- `PUT /api/Fornecedor/{id}` - Atualizar
- `DELETE /api/Fornecedor/{id}` - Deletar

---

## ?? Configuração

### Backend - `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SIGAPetDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Frontend - `environment.ts`
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

---

## ?? Desenvolvimento

### Executar Backend Manualmente
```bash
cd Backend
dotnet run --launch-profile http
```

### Executar Frontend Manualmente
```bash
cd Frontend
npm install
npm start
```

### Build de Produção

**Backend:**
```bash
cd Backend
dotnet publish -c Release -o ./publish
```

**Frontend:**
```bash
cd Frontend
npm run build
# Arquivos gerados em: dist/
```

---

## ?? Testes

### Testar API com Swagger
1. Acesse http://localhost:5000/swagger
2. Clique em "Try it out" em qualquer endpoint
3. Execute e veja o resultado

### Exemplo de POST (Criar Tutor)
```json
{
  "nome": "João Silva",
  "telefone": "(11) 98765-4321",
  "email": "joao@exemplo.com",
  "endereco": "Rua Exemplo, 123"
}
```

---

## ?? Problemas Comuns

### Erro: "Unable to configure HTTPS endpoint"
**Solução:**
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Erro: Porta já em uso
**Solução:**
```powershell
.\stop-dev.ps1
.\start-dev.ps1
```

### Frontend não compila
**Solução:**
```bash
cd Frontend
rm -rf node_modules package-lock.json
npm install
```

---

## ?? Documentação Adicional

- **[ARQUITETURA.md](ARQUITETURA.md)** - Documentação técnica detalhada da arquitetura do sistema

---

## ?? Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## ?? Funcionalidades Implementadas

- ? CRUD completo de Tutores
- ? CRUD completo de Animais
- ? CRUD completo de Produtos
- ? CRUD completo de Serviços
- ? CRUD completo de Agendamentos
- ? CRUD completo de Fornecedores
- ? Relacionamento entre entidades
- ? Validações de dados
- ? Tratamento de erros
- ? Documentação Swagger
- ? CORS configurado
- ? Models e Services do Frontend

### ?? Em Desenvolvimento
- ? Componentes do Frontend (UI)
- ? Autenticação e Autorização
- ? Relatórios
- ? Dashboard

---

## ?? Licença

Este projeto está sob a licença MIT.

---

## ?? Autores

**Equipe Edu-2de**
- GitHub: [@Edu-2de](https://github.com/Edu-2de)
- Repositório: [PetShop-C](https://github.com/Edu-2de/PetShop-C)

---

## ?? Suporte

Para dúvidas ou problemas:
1. Consulte a documentação em [ARQUITETURA.md](ARQUITETURA.md)
2. Abra uma issue no GitHub
3. Entre em contato com a equipe

---

**Versão:** 1.0.0  
**Status:** ? Backend funcional | ? Frontend em desenvolvimento  
**Última Atualização:** Novembro 2024
