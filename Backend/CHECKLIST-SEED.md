# Checklist de Execução - Sistema Seed

## ? Passo a Passo para Usar

### 1. Preparação do Banco
- [ ] Abrir SQL Server Management Studio
- [ ] Conectar ao banco de dados SIGA-PET
- [ ] Abrir o arquivo `Backend/RESET-BANCO-DADOS.sql`
- [ ] Executar o script completo (F5)
- [ ] Verificar mensagens de sucesso no output

**Resultado esperado:**
```
Banco de dados limpo com sucesso!
```

---

### 2. Iniciar o Backend
- [ ] Abrir terminal no diretório raiz do projeto
- [ ] Executar: `cd Backend`
- [ ] Executar: `dotnet run`
- [ ] Aguardar mensagem: `Now listening on: https://localhost:7000`

**Resultado esperado:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
```

---

### 3. Acessar Swagger
- [ ] Abrir navegador
- [ ] Acessar: `https://localhost:7000/swagger`
- [ ] Localizar a seção `Seed - Controller para gerenciar dados de teste`

**Resultado esperado:**
- Interface do Swagger carregada
- Seção Seed visível com todos os endpoints

---

### 4. Popular o Banco (Opção Rápida)
- [ ] Expandir endpoint: `POST /api/Seed/popular-completo`
- [ ] Clicar em `Try it out`
- [ ] Clicar em `Execute`
- [ ] Aguardar resposta (5-10 segundos)

**Resultado esperado:**
```json
{
  "success": true,
  "message": "Banco de dados populado com sucesso",
  "tempo_segundos": 6.23,
  "passos_executados": {
    "1_usuarios": "OK",
    "2_funcionarios": "OK",
    ...
  }
}
```

---

### 5. Verificar Status
- [ ] Expandir endpoint: `GET /api/Seed/status`
- [ ] Clicar em `Try it out`
- [ ] Clicar em `Execute`

**Resultado esperado:**
```json
{
  "banco": "SIGA-PET",
  "banco_vazio": false,
  "total_registros": 67,
  "tabelas": {
    "usuarios": 5,
    "funcionarios": 3,
    "tutores": 3,
    ...
  }
}
```

---

### 6. Testar Login
- [ ] Expandir endpoint: `POST /api/Auth/login`
- [ ] Clicar em `Try it out`
- [ ] Inserir no corpo da requisição:
```json
{
  "email": "admin@sigapet.com",
  "senha": "admin123"
}
```
- [ ] Clicar em `Execute`
- [ ] Copiar o token JWT retornado

**Resultado esperado:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "usuarioId": 1,
    "email": "admin@sigapet.com",
    "tipoUsuario": "Admin"
  }
}
```

---

### 7. Autorizar no Swagger
- [ ] Clicar no botão `Authorize` (cadeado) no topo do Swagger
- [ ] Inserir: `Bearer SEU_TOKEN_AQUI`
- [ ] Clicar em `Authorize`
- [ ] Clicar em `Close`

**Resultado esperado:**
- Cadeado fica fechado/verde
- Requests agora incluem token de autenticação

---

### 8. Testar Endpoints de Consulta
- [ ] `GET /api/Usuario` - Listar usuários
- [ ] `GET /api/Funcionario` - Listar funcionários
- [ ] `GET /api/Tutor` - Listar tutores
- [ ] `GET /api/Categoria` - Listar categorias
- [ ] `GET /api/Fornecedor` - Listar fornecedores
- [ ] `GET /api/Produto` - Listar produtos
- [ ] `GET /api/Servico` - Listar serviços
- [ ] `GET /api/Animal` - Listar animais

**Resultado esperado:**
- Todos retornam status 200
- Todos retornam arrays com dados

---

### 9. Verificar no Banco de Dados
- [ ] Abrir SQL Server Management Studio
- [ ] Executar consulta:
```sql
SELECT 
    (SELECT COUNT(*) FROM Usuarios) as Usuarios,
    (SELECT COUNT(*) FROM Funcionarios) as Funcionarios,
    (SELECT COUNT(*) FROM Tutores) as Tutores,
    (SELECT COUNT(*) FROM Categorias) as Categorias,
    (SELECT COUNT(*) FROM Fornecedores) as Fornecedores,
    (SELECT COUNT(*) FROM Produtos) as Produtos,
    (SELECT COUNT(*) FROM ProdutoImagens) as Imagens,
    (SELECT COUNT(*) FROM Servicos) as Servicos,
    (SELECT COUNT(*) FROM Animais) as Animais
```

**Resultado esperado:**
```
Usuarios: 5
Funcionarios: 3
Tutores: 3
Categorias: 6
Fornecedores: 4
Produtos: 15
Imagens: 9
Servicos: 8
Animais: 6
```

---

### 10. Testar Frontend (Opcional)
- [ ] Abrir novo terminal
- [ ] Executar: `cd Frontend`
- [ ] Executar: `npm start`
- [ ] Abrir navegador em: `http://localhost:4200`
- [ ] Fazer login com: `admin@sigapet.com` / `admin123`
- [ ] Navegar pelas páginas:
  - [ ] Dashboard
  - [ ] Produtos
  - [ ] Serviços
  - [ ] Agendamentos
  - [ ] Pets
  - [ ] Funcionários

---

## ?? Solução de Problemas

### Problema: Erro ao popular banco
**Solução:**
1. Execute: `DELETE /api/Seed/limpar`
2. Execute novamente: `POST /api/Seed/popular-completo`

### Problema: Erro "Banco ja possui dados"
**Solução:**
1. Execute o script SQL `RESET-BANCO-DADOS.sql`
2. OU execute: `DELETE /api/Seed/limpar`

### Problema: Token JWT inválido
**Solução:**
1. Faça login novamente
2. Copie o novo token
3. Clique em `Authorize` e cole o novo token

### Problema: Senha não funciona
**Solução:**
- Verifique se está usando: `admin123`
- Verifique se o email está correto
- Execute `DELETE /api/Seed/limpar` e `POST /api/Seed/popular-completo` novamente

---

## ?? Validação Final

Após executar todos os passos, você deve ter:

- ? 67 registros no banco de dados
- ? Login funcionando
- ? Todos os endpoints retornando dados
- ? Frontend acessível e funcional
- ? Carrinho de compras operacional
- ? Sistema de agendamentos operacional

---

## ?? Próximas Ações

Agora que o sistema está populado:

1. **Testar Funcionalidades:**
   - Criar novos produtos
   - Fazer compras (adicionar ao carrinho)
   - Criar agendamentos
   - Gerenciar pets

2. **Testar Fluxos Completos:**
   - Cadastro de novo cliente
   - Compra de produto
   - Agendamento de serviço
   - Gestão de estoque

3. **Validar Regras de Negócio:**
   - Estoque sendo decrementado em vendas
   - Validações de agendamento (horário, conflitos)
   - Permissões por tipo de usuário

---

## ?? Notas Finais

- Todos os dados criados são fictícios e para teste
- As senhas são simples (`admin123`) - em produção usar senhas fortes
- As imagens dos produtos são URLs placeholder
- Os dados podem ser resetados a qualquer momento

---

**Data de Criação:** 07/12/2024  
**Versão:** 1.0.0  
**Status:** Pronto para execução
