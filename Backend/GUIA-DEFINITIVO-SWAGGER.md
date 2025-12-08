# ?? GUIA DEFINITIVO DO SWAGGER - SIGA PET

## ?? COMO USAR O SWAGGER COMPLETAMENTE NOVO

### **1. INICIAR O SISTEMA**
```bash
cd Backend
dotnet run
```

### **2. ACESSAR O SWAGGER**
```
?? http://localhost:5000/swagger
```

---

## ?? **SEÇÃO ESPECIAL: DADOS DE EXEMPLO**

**PRIMEIRA COISA A FAZER:** Procure pela seção **"?? Dados de Exemplo"** no Swagger!

### **?? Verificar Estado do Sistema**
**GET** `/api/Exemplos/status-sistema`
- ? Mostra quantos dados existem
- ? Indica se precisa criar dados de exemplo
- ? Dá recomendações do que fazer

### **?? Criar Ambiente Completo**
**POST** `/api/Exemplos/setup-completo`

**? UM CLIQUE = SISTEMA COMPLETO!**
- ? 8 usuários (1 admin + 4 funcionários + 3 clientes)
- ? 5 categorias de produtos
- ? 3 fornecedores
- ? 6 produtos diversos
- ? 5 serviços (com funcionários responsáveis)
- ? 4 pets de exemplo
- ? 5 agendamentos (incluindo conflitos para testar)

### **?? Criar Dados Básicos (Versão Rápida)**
**POST** `/api/Exemplos/dados-basicos`
- ? 1 admin + 2 funcionários + 1 cliente + 1 pet
- ? 2 serviços essenciais
- ? Pronto para testes rápidos

### **?? Reset Total (Cuidado!)**
**DELETE** `/api/Exemplos/reset-sistema`
- ? Remove TODOS os dados
- ? Use apenas quando quiser começar do zero

---

## ?? **PASSO A PASSO COMPLETO**

### **ETAPA 1: Criar Dados**
1. Execute **GET** `/api/Exemplos/status-sistema`
2. Se sistema vazio, execute **POST** `/api/Exemplos/setup-completo`
3. Aguarde a confirmação de sucesso

### **ETAPA 2: Fazer Login**
1. Vá para **POST** `/api/Auth/login`
2. Use estes dados:
```json
{
  "email": "admin@sigapet.com",
  "senha": "123456"
}
```
3. **COPIE O TOKEN** que aparece na resposta

### **ETAPA 3: Autorizar no Swagger**
1. Clique no botão **"?? Authorize"** no topo do Swagger
2. Cole o token no formato: `Bearer SEU_TOKEN_AQUI`
3. Clique em **"Authorize"**
4. Agora você pode testar TODOS os endpoints!

### **ETAPA 4: Explorar os Dados**
Execute estes GETs para ver os dados criados:
- **GET** `/api/Funcionario` - Ver funcionários
- **GET** `/api/Servico` - Ver serviços e responsáveis
- **GET** `/api/Animal` - Ver pets cadastrados
- **GET** `/api/Agendamento` - Ver agendamentos exemplo
- **GET** `/api/Produto` - Ver produtos em estoque

---

## ?? **TESTES ESPECIAIS DE REGRAS DE NEGÓCIO**

### **?? Teste 1: Funcionário Responsável Automático**
1. **POST** `/api/Agendamento`
```json
{
  "animalId": 1,
  "servicoId": 1,
  "dataHora": "2024-12-10T15:00:00",
  "status": "Pendente",
  "observacoes": "Teste de funcionário responsável"
}
```
? **Resultado esperado:** Sistema auto-seleciona Dr. João (funcionário responsável)

### **?? Teste 2: Conflito de Horário**
1. Primeiro, agende algo às 14h de amanhã
2. Tente agendar Dr. João no mesmo horário:
```json
{
  "animalId": 2,
  "servicoId": 4,
  "funcionarioId": 1,
  "dataHora": "2024-12-09T14:00:00",
  "status": "Pendente"
}
```
? **Resultado esperado:** Erro "funcionário já possui agendamento neste horário"

### **?? Teste 3: Verificar Disponibilidade**
**GET** `/api/Agendamento/disponibilidade?servicoId=1&dataHora=2024-12-09T14:00:00`
- ? Mostra se horário está livre
- ? Lista conflitos específicos
- ? Informa funcionário responsável

### **?? Teste 4: Serviço Flexível**
```json
{
  "animalId": 1,
  "servicoId": 5,
  "dataHora": "2024-12-09T14:00:00"
}
```
? **Resultado esperado:** Aceita qualquer funcionário (Corte de Unhas não tem responsável)

---

## ?? **DIFERENTES TIPOS DE USUÁRIO**

### **?? Admin (Completo Acesso)**
- **Login:** admin@sigapet.com / 123456
- ? Pode tudo: criar funcionários, serviços, ver todos agendamentos

### **????? Veterinário**
- **Login:** joao.vet@sigapet.com / 123456
- ? Especialista em consultas e vacinação

### **?? Tosadora**
- **Login:** maria.tosadora@sigapet.com / 123456
- ? Responsável por banho e tosa

### **?? Cliente**
- **Login:** joao.cliente@gmail.com / 123456
- ? Pode agendar serviços para seus pets

---

## ??? **ENDPOINTS POR CATEGORIA NO SWAGGER**

### **?? Auth**
- Login, registro, refresh token

### **?? Funcionários**
- CRUD completo de funcionários
- Busca por cargo, status

### **?? Serviços**
- CRUD com funcionário responsável
- Busca por nome, ativos

### **?? Agendamentos**
- CRUD completo
- Verificação de disponibilidade
- Filtros por data, tutor, animal

### **?? Animais/Pets**
- CRUD completo
- Busca por tutor, espécie

### **?? Produtos**
- CRUD completo
- Gestão de estoque
- Categorias e fornecedores

### **?? Vendas**
- Vendas mistas (produtos + serviços)
- Relatórios por período

### **?? Dados de Exemplo**
- **? SEÇÃO ESPECIAL** para criar dados rapidamente

---

## ?? **FLUXO COMPLETO DE TESTE**

### **1. Setup (1 minuto)**
1. Status-sistema ? Setup-completo ? Login admin ? Authorize

### **2. Testar Básico (2 minutos)**
1. Listar funcionários, serviços, pets
2. Criar um agendamento simples

### **3. Testar Regras Avançadas (3 minutos)**
1. Testar conflitos de horário
2. Testar funcionário responsável
3. Testar disponibilidade

### **4. Testar Como Cliente (2 minutos)**
1. Login como cliente
2. Agendar para seus pets
3. Ver limitações de acesso

---

## ?? **RESOLUÇÃO DE PROBLEMAS**

### **Erro 401 Unauthorized**
- ? Faça login e use o token no Authorize

### **Erro 400 Sistema já possui dados**
- ? Use DELETE reset-sistema primeiro

### **Erro 404 Not Found**
- ? Verifique se criou os dados de exemplo

### **Conflitos de Agendamento**
- ? É esperado! Teste as regras de negócio

---

## ?? **PRONTO PARA PRODUÇÃO!**

Com este Swagger completo, você pode:
- ? **Demonstrar** o sistema para clientes
- ? **Testar** todas as funcionalidades
- ? **Desenvolver** novas features
- ? **Documentar** a API
- ? **Treinar** usuários

**O sistema está 100% funcional e profissional! ??**

---

**?? Documentação Adicional:**
- `SWAGGER-DADOS-EXEMPLO.md` - Dados detalhados para copiar/colar
- `GUIA-TESTES-SWAGGER.md` - Cenários específicos de teste
- `IMPLEMENTACAO-COMPLETA-FINAL.md` - Resumo de todas as implementações