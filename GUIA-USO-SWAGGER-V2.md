# ?? GUIA COMPLETO - NOVA VERSÃO COM SWAGGER MELHORADO

## ?? O QUE FOI FEITO

### ? 1. Controller de Banco de Dados (`DatabaseController`)

Novo controlador acessível via Swagger com 3 endpoints principais:

#### ?? POST `/api/Database/reset-e-popular`
- **Função**: Deleta o banco, recria todas as tabelas e popula com dados de exemplo
- **?? ATENÇÃO**: Deleta TODOS os dados existentes
- **Tempo**: 5-10 segundos
- **Resultado**: Banco completo pronto para uso

**Dados criados:**
- 7 Usuários (1 Admin + 3 Funcionários + 3 Clientes)
- 3 Funcionários (Veterinário, Tosador, Atendente)
- 4 Tutores (donos de pets)
- 5 Animais (pets)
- 6 Categorias de produtos
- 3 Fornecedores
- 8 Produtos com imagens
- 6 Serviços
- 5 Agendamentos
- 3 Vendas com itens

#### ?? GET `/api/Database/status`
- **Função**: Verifica status do banco e quantidade de registros
- **Uso**: Monitoramento e debugging

#### ??? DELETE `/api/Database/limpar-dados`
- **Função**: Remove todos os dados mas mantém as tabelas
- **Uso**: Quando quer começar do zero mas já tem estrutura

### ? 2. Swagger Melhorado

- **Documentação completa** com descrições detalhadas
- **Instruções de uso** direto na página inicial
- **Exemplos de requisições** em todos os endpoints
- **Tabelas de credenciais** para testes
- **CSS customizado** com cores do projeto
- **Organização melhorada** dos endpoints

### ? 3. Correções de Bugs

- ? Campo `UsuarioId` adicionado na tabela `Vendas`
- ? Endpoint `GET /api/Venda/usuario/{id}` funcionando
- ? Agendamentos mapeando corretamente objetos relacionados
- ? Compras funcionando para qualquer usuário
- ? "Minhas Compras" carregando corretamente

## ?? COMO USAR

### Passo 1: Iniciar o Sistema

```bash
# OPÇÃO 1: Script automático (Windows)
./start-dev.bat

# OPÇÃO 2: Script PowerShell
./start-dev.ps1

# OPÇÃO 3: Manual
# Terminal 1 - Backend
cd Backend
dotnet run

# Terminal 2 - Frontend
cd Frontend
npm start
```

### Passo 2: Acessar o Swagger

1. **Abra o navegador**: http://localhost:5000
2. Você verá a documentação completa do Swagger

### Passo 3: Resetar e Popular o Banco

1. **Localize o endpoint**: `POST /api/Database/reset-e-popular`
2. **Clique em "Try it out"**
3. **Clique em "Execute"**
4. **Aguarde** a resposta (5-10 segundos)
5. **Verifique** a resposta de sucesso:

```json
{
  "success": true,
  "message": "? Banco de dados resetado e populado com sucesso!",
  "resumo": {
    "usuarios": 7,
    "funcionarios": 3,
    "tutores": 4,
    ...
  },
  "credenciais": {
    "admin": {
      "email": "admin@sigapet.com",
      "senha": "senha123"
    },
    ...
  }
}
```

### Passo 4: Fazer Login e Autorizar

1. **Localize o endpoint**: `POST /api/Auth/login`
2. **Clique em "Try it out"**
3. **Cole o JSON**:

```json
{
  "email": "admin@sigapet.com",
  "senha": "senha123"
}
```

4. **Clique em "Execute"**
5. **Copie o token** da resposta (campo `token`)
6. **Clique no botão ?? Authorize** no topo da página
7. **Digite**: `Bearer {seu_token_aqui}` (substitua pelo token copiado)
8. **Clique em "Authorize"** e depois em "Close"

### Passo 5: Testar os Endpoints

Agora você pode testar qualquer endpoint protegido!

**Exemplos de testes:**

#### ?? Listar Produtos
```
GET /api/Produto
```

#### ?? Criar Venda (Compra)
```
POST /api/Venda
```
```json
{
  "usuarioId": 5,
  "tutorId": 1,
  "formaPagamento": "Cartão de Crédito",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 2
    }
  ]
}
```

#### ?? Listar Agendamentos
```
GET /api/Agendamento
```

#### ?? Minhas Compras
```
GET /api/Venda/usuario/5
```

## ?? CREDENCIAIS DE TESTE

Após popular o banco, use estas credenciais:

| Perfil | Email | Senha | Descrição |
|--------|-------|-------|-----------|
| ????? **Admin** | admin@sigapet.com | senha123 | Acesso total ao sistema |
| ????? **Veterinário** | carlos.vet@sigapet.com | senha123 | Dr. Carlos (atende animais) |
| ?? **Tosador** | ana.tosa@sigapet.com | senha123 | Ana (banho e tosa) |
| ?? **Atendente** | pedro.atend@sigapet.com | senha123 | Pedro (vendas) |
| ?? **Cliente 1** | maria.silva@email.com | senha123 | Maria (tem 2 pets) |
| ?? **Cliente 2** | joao.santos@email.com | senha123 | João (tem 1 pet) |
| ?? **Cliente 3** | paula.oli@email.com | senha123 | Paula (tem 2 pets) |

## ?? TESTAR NO FRONTEND

### 1. Login
```
http://localhost:4200/login
```
Use qualquer credencial acima.

### 2. Fazer uma Compra
1. Acesse: http://localhost:4200/produtos
2. Clique em um produto
3. Clique em "Comprar Agora"
4. Verifique se a compra foi criada

### 3. Ver Minhas Compras
```
http://localhost:4200/vendas/minhas
```
Deve aparecer a compra que você fez.

### 4. Ver Agendamentos
```
http://localhost:4200/agenda
```
Deve listar os agendamentos do tutor logado.

## ?? TROUBLESHOOTING

### Problema: "Banco de dados não encontrado"

**Solução:**
1. Use o endpoint `POST /api/Database/reset-e-popular` no Swagger
2. Aguarde a conclusão
3. Tente novamente

### Problema: "401 Unauthorized"

**Solução:**
1. Faça login em `POST /api/Auth/login`
2. Copie o token
3. Clique em ?? Authorize
4. Digite: `Bearer {token}`
5. Autorize e tente novamente

### Problema: "Minhas Compras em branco"

**Solução:**
1. Verifique se fez login
2. Faça uma compra primeiro
3. Recarregue a página
4. Se o problema persistir:
   - Abra o Console (F12)
   - Veja se há erros
   - Verifique se o endpoint `/api/Venda/usuario/{id}` está respondendo

### Problema: "Agendamentos não carregam"

**Solução:**
1. Verifique se o usuário é tutor (tem pets)
2. Se não for tutor, não terá agendamentos
3. Use o login: `maria.silva@email.com` / `senha123` (tem pets)

## ? CHECKLIST DE VERIFICAÇÃO

- [ ] Backend iniciado (http://localhost:5000)
- [ ] Frontend iniciado (http://localhost:4200)
- [ ] Swagger acessível (http://localhost:5000)
- [ ] Banco resetado e populado via Swagger
- [ ] Login realizado com sucesso
- [ ] Token JWT copiado e autorizado
- [ ] Produtos listando no frontend
- [ ] Compra realizada com sucesso
- [ ] "Minhas Compras" mostrando dados
- [ ] Agendamentos carregando corretamente

## ?? ESTRUTURA DO BANCO ATUALIZADA

### Tabela: Vendas

| Coluna | Tipo | Descrição | Novo? |
|--------|------|-----------|-------|
| VendaId | INT | ID da venda (PK) | Não |
| **UsuarioId** | INT | **ID do usuário comprador** | **? SIM** |
| TutorId | INT | ID do tutor (opcional) | Não |
| FuncionarioId | INT | ID do funcionário (opcional) | Não |
| DataVenda | DATETIME | Data da venda | Não |
| ValorTotal | DECIMAL | Valor total | Não |
| FormaPagamento | NVARCHAR | Forma de pagamento | Não |
| Observacoes | NVARCHAR | Observações | Não |

## ?? PRÓXIMOS PASSOS

1. ? **Execute o reset do banco** via Swagger
2. ? **Teste os endpoints** conforme os exemplos
3. ? **Valide as funcionalidades** no frontend
4. ? **Monitore os logs** para identificar problemas
5. ? **Reporte bugs** se encontrar algum

## ?? NOTAS IMPORTANTES

- **Sempre use o Swagger** para testar primeiro antes de usar o frontend
- **Resete o banco** sempre que precisar de dados frescos
- **Copie as credenciais** para facilitar os testes
- **Autorize o JWT** antes de testar endpoints protegidos
- **Monitore o console** do navegador para ver erros

## ?? SUPORTE

Se encontrar problemas:

1. **Verifique os logs** do backend (terminal)
2. **Veja o console** do navegador (F12)
3. **Teste no Swagger** antes do frontend
4. **Use o endpoint de status**: `GET /api/Database/status`
5. **Resete o banco** se necessário

---

**Data**: 08/12/2024  
**Versão**: 2.0 - Swagger Melhorado  
**Status**: ? Pronto para Uso
