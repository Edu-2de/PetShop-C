# SIGA-PET - Sistema de Gestão para PetShop

SIGA-PET é uma solução completa para gestão de pet shops, com arquitetura moderna, separação entre Backend (API RESTful) e Frontend (SPA), e foco em escalabilidade, segurança e facilidade de manutenção.

---

## Sumário
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura e Estrutura de Pastas](#arquitetura-e-estrutura-de-pastas)
- [Endpoints da API](#endpoints-da-api)
- [Regras de Negócio](#regras-de-negócio)
- [Instalação e Execução](#instalação-e-execução)
- [Configuração de Ambiente](#configuração-de-ambiente)
- [Testes](#testes)
- [Boas Práticas e Observações](#boas-práticas-e-observações)
- [Licença](#licença)
- [Contato](#contato)

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
- Interface responsiva e intuitiva
- Relatórios e filtros de pesquisa
- Integração entre módulos (clientes, pets, produtos, serviços, agenda)
- API documentada via Swagger/OpenAPI

---

## Tecnologias Utilizadas

### Backend (API)
- ASP.NET Core 8.0
- C#
- Entity Framework Core (ORM)
- SQL Server (LocalDB ou instância)
- AutoMapper
- Swagger / OpenAPI
- Autenticação JWT (estrutura pronta)
- Arquitetura em camadas (Controllers, DTOs, Models, Data, Profiles)
- Validação de dados e regras de negócio

### Frontend (SPA)
- Angular 17
- TypeScript
- Bootstrap 5
- RxJS
- Angular HttpClient
- SCSS para estilos customizados
- Estrutura modular de componentes e serviços
- Guards de rota para autenticação/autorização
- Interceptors para requisições HTTP

---

## Arquitetura e Estrutura de Pastas

### Backend
```
/Controllers      - Endpoints da API
/DTOs             - Objetos de transferência de dados
/Models           - Entidades e regras de negócio
/Data             - AppDbContext e conexão com o banco
/Profiles         - Configurações do AutoMapper
/Enums            - Enumerações de status e tipos
/Properties       - Configurações de inicialização
```

### Frontend
```
/src/app/pages        - Telas e componentes principais
/src/app/services     - Comunicação com o backend (GET, POST, PUT, DELETE)
/src/app/models       - Interfaces TypeScript para tipagem
/src/app/guards       - Proteção de rotas (auth, admin)
/src/app/assets       - Imagens, ícones e arquivos estáticos
/src/app/interceptors - Interceptação de requisições HTTP
/src/app/app.routes.ts- Configuração de rotas
/src/app/app.scss     - Estilos globais
```

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

### Funcionários e Categorias (se implementado)
- GET /api/funcionarios
- GET /api/categorias

---

## Regras de Negócio

- Agendamentos em intervalos fixos de 30 minutos
  - O frontend utiliza inputs com `step="1800"` para garantir intervalos de 30 minutos
  - Horários inseridos manualmente são ajustados para o padrão antes do envio
  - A API valida o horário e impede conflitos na agenda
- Controle de estoque com validação de quantidade e preço
- Relacionamento entre tutores e pets
- Autenticação JWT para proteger rotas sensíveis
- Permissões diferenciadas para admin e usuário comum
- Cadastro de produtos e serviços com imagens
- Filtros e pesquisa em todas as listas
- Validação de dados obrigatórios em todos os formulários
- Proteção contra duplicidade de registros

---

## Instalação e Execução

### Pré-requisitos
- .NET SDK 8.0
- Node.js 20+
- SQL Server (LocalDB ou instância)
- NPM

### 1. Configuração do banco
```sh
cd Backend
dotnet ef database update
```

### 2. Execução separada
#### Backend
```sh
cd Backend
dotnet run --launch-profile http
```
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

#### Frontend
```sh
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
```sh
./start-dev.bat
```

---

## Configuração de Ambiente

- O arquivo `appsettings.json` do backend permite configurar a string de conexão do banco de dados, portas e outras opções.
- O frontend pode ser configurado em `src/environments/environment.ts` para apontar para a URL da API.
- Variáveis sensíveis devem ser mantidas fora do versionamento (use `.env` ou segredos do sistema operacional).

---

## Testes

- O backend pode ser testado via Swagger ou ferramentas como Postman.
- O frontend possui suporte a testes unitários com Jasmine/Karma.
- Recomenda-se criar cenários de teste para cada funcionalidade crítica (cadastro, edição, exclusão, autenticação, agendamento).

---

## Boas Práticas e Observações

- O SIGA-PET foi desenvolvido com foco em modularidade, clareza, escalabilidade e boas práticas tanto no frontend quanto no backend.
- O sistema é preparado para autenticação JWT, podendo ser integrado facilmente com provedores externos.
- O frontend é responsivo e utiliza Bootstrap 5 para garantir boa experiência em diferentes dispositivos.
- O backend segue arquitetura em camadas, facilitando manutenção e evolução do projeto.
- O código segue padrões de nomenclatura, organização e separação de responsabilidades.
- O projeto está pronto para deploy em ambiente cloud ou local.

---

## Licença

Este projeto é distribuído sob a licença MIT. Consulte o arquivo LICENSE para mais detalhes.

---

## Contato

Para dúvidas, sugestões ou contribuições, utilize o sistema de Issues ou Pull Requests no GitHub.

---

## Créditos e Referências

- Documentação oficial do [ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- Documentação oficial do [Angular](https://angular.io/docs)
- Documentação do [Bootstrap](https://getbootstrap.com/docs/5.0/getting-started/introduction/)
- Exemplos de arquitetura em camadas e boas práticas de desenvolvimento

---

## Histórico de Versões

- v1.0 - Estrutura inicial, cadastro de clientes, pets, produtos, serviços, agenda e fornecedores
- v1.1 - Implementação de autenticação JWT, dashboard administrativo, melhorias de UI/UX
- v1.2 - Adição de testes automatizados, documentação Swagger, ajustes de segurança

---

## Observações Finais

O SIGA-PET foi projetado para ser modular, escalável e simples de manter, seguindo boas práticas de desenvolvimento tanto no backend quanto no frontend. O sistema pode ser expandido para novas funcionalidades conforme a necessidade do negócio.






