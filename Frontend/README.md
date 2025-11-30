# SIGA-PET - Sistema de Gestão para PetShop

SIGA-PET é uma solução completa para gestão de pet shops, com arquitetura moderna e separação entre Backend (API) e Frontend (SPA). O sistema foi desenvolvido para ser escalável, modular e fácil de manter, utilizando tecnologias atuais do mercado.

---

## Funcionalidades

- Cadastro, edição e consulta de tutores (clientes)
- Gerenciamento de animais vinculados aos tutores, com histórico
- Controle de estoque: produtos, quantidades, preços e imagens
- Catálogo de serviços: banho, tosa, consultas, entre outros
- Agenda inteligente: agendamentos em intervalos fixos de 30 minutos, sem conflitos
- Gestão de fornecedores: cadastro, histórico de suprimentos
- Dashboard administrativo: visão geral das principais funções
- Autenticação e controle de acesso (admin/usuário)

---

## Tecnologias Utilizadas

### Backend
- ASP.NET Core 8.0
- C#
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger / OpenAPI
- Autenticação JWT (estrutura pronta)

### Frontend
- Angular 17
- TypeScript
- Bootstrap 5
- RxJS
- Angular HttpClient

---

## Estrutura de Pastas

### Backend
- `/Controllers` - Endpoints da API
- `/DTOs` - Objetos de transferência de dados
- `/Models` - Entidades e regras de negócio
- `/Data` - AppDbContext e conexão com o banco
- `/Profiles` - Configurações do AutoMapper

### Frontend
- `/services` - Comunicação com o backend (GET, POST, PUT, DELETE)
- `/pages` - Telas e componentes principais
- `/models` - Interfaces TypeScript para tipagem

---

## Endpoints da API

### Clientes
- GET /api/clientes
- GET /api/clientes/{id}
- POST /api/clientes
- PUT /api/clientes/{id}
- DELETE /api/clientes/{id}

### Animais
- GET /api/animais
- GET /api/animais/cliente/{clienteId}
- POST /api/animais
- PUT /api/animais/{id}
- DELETE /api/animais/{id}

### Produtos (Estoque)
- GET /api/produtos
- POST /api/produtos
- PUT /api/produtos/{id}
- DELETE /api/produtos/{id}

### Agendamentos
- GET /api/agendamentos
- GET /api/agendamentos/data/{data}
- POST /api/agendamentos
- PUT /api/agendamentos/{id}
- DELETE /api/agendamentos/{id}

### Fornecedores e Serviços
- GET /api/fornecedores
- GET /api/servicos

---

## Regras de Negócio

### Agendamentos com intervalos fixos de 30 minutos
- O frontend utiliza inputs com `step="1800"` para garantir intervalos de 30 minutos
- Horários inseridos manualmente são ajustados para o padrão antes do envio
- A API valida o horário e impede conflitos na agenda

---

## Instalação e Execução

### Pré-requisitos
- .NET SDK 8.0
- Node.js 20+
- SQL Server (LocalDB ou instância)
- NPM

### 1. Configuração do banco

```bash
cd Backend
dotnet ef database update
```

### 2. Execução separada

#### Backend
```bash
cd Backend
dotnet run --launch-profile http
```
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

#### Frontend
```bash
cd Frontend
npm install
npm start
```
- App: http://localhost:4200

### 3. Execução simultânea (Windows)
Crie um arquivo `start-dev.bat` na raiz do projeto:

```bat
@echo off
echo Iniciando o SIGA-PET...

start "Backend API" cmd /k "cd Backend && dotnet run --launch-profile http"
timeout /t 5

start "Frontend Angular" cmd /k "cd Frontend && npm start"

echo Sistema iniciando. Acesse http://localhost:4200
```

Para executar:
```bash
./start-dev.bat
```

---

## Observações

- O SIGA-PET foi desenvolvido com foco em modularidade, clareza, escalabilidade e boas práticas tanto no frontend quanto no backend.
- O sistema é preparado para autenticação JWT, podendo ser integrado facilmente com provedores externos.
- O frontend é responsivo e utiliza Bootstrap 5 para garantir boa experiência em diferentes dispositivos.
- O backend segue arquitetura em camadas, facilitando manutenção e evolução do projeto.

---

## Licença

Este projeto é distribuído sob a licença MIT. Consulte o arquivo LICENSE para mais detalhes.

---

## Contato

Para dúvidas, sugestões ou contribuições, utilize o sistema de Issues ou Pull Requests no GitHub.






