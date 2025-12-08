# ?? GUIA COMPLETO DE TESTES - SWAGGER SIGA PET

## ?? ORDEM RECOMENDADA PARA TESTES COMPLETOS

### 1?? **CONFIGURAÇÃO INICIAL**
```bash
# 1. Inicie o backend
cd Backend
dotnet run

# 2. Acesse o Swagger
http://localhost:5000/swagger
```

---

## ?? **ETAPA 1: AUTENTICAÇÃO E USUÁRIOS**

### 1.1 Cadastrar Admin Principal
**POST /api/Auth/register**
```json
{
  "nome": "Admin Sistema",
  "email": "admin@sigapet.com",
  "senha": "123456",
  "telefone": "51999887766",
  "endereco": "Rua Principal, 123"
}
```
> ? **Resultado esperado:** Token JWT + dados do usuário criado

### 1.2 Login Admin
**POST /api/Auth/login**
```json
{
  "email": "admin@sigapet.com",
  "senha": "123456"
}
```
> ?? **IMPORTANTE:** Copie o token e clique em "Authorize" no Swagger!

---

## ?? **ETAPA 2: CADASTRAR FUNCIONÁRIOS**

### 2.1 Veterinário Dr. João
**POST /api/Funcionario**
```json
{
  "nome": "Dr. João Silva",
  "cargo": "Veterinário",
  "telefone": "51987654321",
  "email": "joao.vet@sigapet.com",
  "senha": "vet123",
  "dataContratacao": "2024-01-15T08:00:00"
}
```

### 2.2 Tosadora Maria
**POST /api/Funcionario**
```json
{
  "nome": "Maria Santos",
  "cargo": "Tosador",
  "telefone": "51976543210",
  "email": "maria.tosadora@sigapet.com",
  "senha": "tosa123",
  "dataContratacao": "2024-02-01T08:00:00"
}
```

### 2.3 Atendente Ana
**POST /api/Funcionario**
```json
{
  "nome": "Ana Costa",
  "cargo": "Atendente",
  "telefone": "51965432109",
  "email": "ana.atendente@sigapet.com",
  "senha": "atend123",
  "dataContratacao": "2024-01-20T08:00:00"
}
```

---

## ?? **ETAPA 3: ESTRUTURA DE DADOS**

### 3.1 Criar Categorias
**POST /api/Categoria** (repita para cada uma)
```json
{"nome": "Alimentação", "descricao": "Rações e petiscos"}
{"nome": "Higiene", "descricao": "Shampoos e produtos de limpeza"}
{"nome": "Medicamentos", "descricao": "Medicamentos veterinários"}
{"nome": "Brinquedos", "descricao": "Entretenimento para pets"}
{"nome": "Acessórios", "descricao": "Coleiras, guias e camas"}
```

### 3.2 Cadastrar Fornecedores
**POST /api/Fornecedor**
```json
{
  "nome": "Premier Pet Distribuidora",
  "cnpj": "12.345.678/0001-90",
  "telefone": "51333221100",
  "email": "vendas@premierpet.com.br",
  "endereco": "Av. das Indústrias, 500"
}
```

---

## ?? **ETAPA 4: SERVIÇOS COM FUNCIONÁRIOS RESPONSÁVEIS**

### 4.1 Consulta Veterinária (Dr. João responsável)
**POST /api/Servico**
```json
{
  "nome": "Consulta Veterinária Geral",
  "descricao": "Consulta clínica geral com exame físico completo",
  "preco": 80.00,
  "duracaoMinutos": 60,
  "ativo": true,
  "funcionarioResponsavelId": 1
}
```

### 4.2 Banho Simples (Maria responsável)
**POST /api/Servico**
```json
{
  "nome": "Banho Simples",
  "descricao": "Banho com shampoo neutro, secagem e perfume",
  "preco": 35.00,
  "duracaoMinutos": 45,
  "ativo": true,
  "funcionarioResponsavelId": 2
}
```

### 4.3 Tosa Completa (Maria responsável)
**POST /api/Servico**
```json
{
  "nome": "Tosa Completa",
  "descricao": "Banho, tosa, corte de unhas e limpeza de ouvido",
  "preco": 65.00,
  "duracaoMinutos": 90,
  "ativo": true,
  "funcionarioResponsavelId": 2
}
```

### 4.4 Vacinação (Dr. João responsável)
**POST /api/Servico**
```json
{
  "nome": "Vacinação V10",
  "descricao": "Aplicação de vacina V10 com carteirinha",
  "preco": 45.00,
  "duracaoMinutos": 30,
  "ativo": true,
  "funcionarioResponsavelId": 1
}
```

### 4.5 Serviço Geral (sem responsável específico)
**POST /api/Servico**
```json
{
  "nome": "Corte de Unhas",
  "descricao": "Corte simples de unhas",
  "preco": 15.00,
  "duracaoMinutos": 15,
  "ativo": true
}
```

---

## ??????????? **ETAPA 5: CADASTRAR CLIENTES**

### 5.1 Cliente João
**POST /api/Auth/register**
```json
{
  "nome": "João Carlos Silva",
  "email": "joao.cliente@gmail.com",
  "senha": "123456",
  "telefone": "51987123456",
  "endereco": "Rua das Flores, 456"
}
```

### 5.2 Cliente Maria
**POST /api/Auth/register**
```json
{
  "nome": "Maria Fernanda Costa",
  "email": "maria.cliente@gmail.com",
  "senha": "123456",
  "telefone": "51976234567",
  "endereco": "Av. Central, 789"
}
```

---

## ?? **ETAPA 6: CADASTRAR PETS**

> ?? **Login com cliente:** Faça logout do admin e login com cliente

### 6.1 Login João Cliente
**POST /api/Auth/login**
```json
{
  "email": "joao.cliente@gmail.com",
  "senha": "123456"
}
```

### 6.2 Pet Rex (do João)
**POST /api/Animal**
```json
{
  "tutorId": 2,
  "nome": "Rex",
  "especie": "Cão",
  "raca": "Labrador",
  "dataNascimento": "2020-03-15T00:00:00",
  "sexo": "Macho",
  "pelagem": "Dourada",
  "observacoes": "Muito dócil e brincalhão"
}
```

### 6.3 Login Maria Cliente
**POST /api/Auth/login**
```json
{
  "email": "maria.cliente@gmail.com",
  "senha": "123456"
}
```

### 6.4 Pet Luna (da Maria)
**POST /api/Animal**
```json
{
  "tutorId": 3,
  "nome": "Luna",
  "especie": "Gato",
  "raca": "Persa",
  "dataNascimento": "2021-08-20T00:00:00",
  "sexo": "Fêmea",
  "pelagem": "Branca",
  "observacoes": "Gata castrada, muito carinhosa"
}
```

---

## ?? **ETAPA 7: TESTES DE AGENDAMENTO**

> ?? **Use token do cliente ou admin para criar agendamentos**

### 7.1 ? **TESTE: Agendamento Normal**
**POST /api/Agendamento**
```json
{
  "animalId": 1,
  "servicoId": 1,
  "funcionarioId": 1,
  "dataHora": "2024-12-10T14:00:00",
  "status": "Confirmado",
  "observacoes": "Consulta de rotina - checkup anual"
}
```
> ? **Resultado esperado:** Agendamento criado com sucesso

### 7.2 ? **TESTE: Conflito de Funcionário**
**POST /api/Agendamento**
```json
{
  "animalId": 2,
  "servicoId": 4,
  "funcionarioId": 1,
  "dataHora": "2024-12-10T14:00:00",
  "status": "Pendente",
  "observacoes": "Vacina para Luna"
}
```
> ? **Resultado esperado:** Erro - "Este funcionário já possui um agendamento neste horário"

### 7.3 ? **TESTE: Mesmo Horário, Serviço Diferente**
**POST /api/Agendamento**
```json
{
  "animalId": 2,
  "servicoId": 5,
  "dataHora": "2024-12-10T14:00:00",
  "status": "Pendente",
  "observacoes": "Corte de unhas para Luna"
}
```
> ? **Resultado esperado:** Agendamento criado (serviço sem responsável específico)

### 7.4 ? **TESTE: Mesmo Serviço, Mesmo Horário**
**POST /api/Agendamento**
```json
{
  "animalId": 1,
  "servicoId": 1,
  "dataHora": "2024-12-10T14:00:00",
  "status": "Pendente",
  "observacoes": "Outra consulta"
}
```
> ? **Resultado esperado:** Erro - "O serviço 'Consulta Veterinária Geral' já está agendado para este horário"

### 7.5 ? **TESTE: Tosa com Funcionário Responsável**
**POST /api/Agendamento**
```json
{
  "animalId": 1,
  "servicoId": 3,
  "dataHora": "2024-12-12T09:00:00",
  "status": "Pendente",
  "observacoes": "Primeira tosa do Rex"
}
```
> ? **Resultado esperado:** Agendamento criado automaticamente com Maria (funcionarioId: 2)

### 7.6 ? **TESTE: Tentar Tosa com Funcionário Errado**
**POST /api/Agendamento**
```json
{
  "animalId": 2,
  "servicoId": 3,
  "funcionarioId": 3,
  "dataHora": "2024-12-13T10:00:00",
  "status": "Pendente",
  "observacoes": "Tosa com atendente"
}
```
> ? **Resultado esperado:** Erro - "Este serviço deve ser realizado pelo funcionário responsável: Maria Santos"

---

## ?? **ETAPA 8: TESTES DE VERIFICAÇÃO**

### 8.1 Verificar Disponibilidade
**GET /api/Agendamento/disponibilidade?servicoId=1&dataHora=2024-12-10T14:00:00**
> ? **Resultado:** `{ "disponivel": false, "conflitos": ["Funcionário já possui agendamento neste horário"], ... }`

### 8.2 Verificar Horário Livre
**GET /api/Agendamento/disponibilidade?servicoId=1&dataHora=2024-12-10T15:00:00**
> ? **Resultado:** `{ "disponivel": true, "conflitos": [], ... }`

### 8.3 Listar Agendamentos
**GET /api/Agendamento**
> ? **Resultado:** Lista todos os agendamentos com dados completos

### 8.4 Agendamentos por Tutor
**GET /api/Agendamento/tutor/2**
> ? **Resultado:** Lista agendamentos do João

---

## ??? **ETAPA 9: TESTES DE PRODUTOS E VENDAS**

### 9.1 Cadastrar Produtos
**POST /api/Produto**
```json
{
  "nome": "Ração Premier Golden Adulto",
  "descricao": "Ração super premium para cães adultos",
  "preco": 89.90,
  "categoriaId": 1,
  "fornecedorId": 1,
  "codigoBarras": "7898934567890",
  "estoque": 50,
  "estoqueMinimo": 10,
  "ativo": true
}
```

### 9.2 Criar Venda Mista
**POST /api/Venda**
```json
{
  "tutorId": 2,
  "funcionarioId": 3,
  "formaPagamento": "PIX",
  "observacoes": "Consulta + ração",
  "itens": [
    {
      "servicoId": 1,
      "quantidade": 1,
      "precoUnitario": 80.00
    },
    {
      "produtoId": 1,
      "quantidade": 1,
      "precoUnitario": 89.90
    }
  ]
}
```

---

## ?? **CENÁRIOS DE VALIDAÇÃO ESPECÍFICOS**

### ? **REGRAS DE NEGÓCIO IMPLEMENTADAS:**

1. **? Funcionário Responsável Automático:**
   - Serviços com funcionário responsável auto-atribuem o profissional
   - Não permite agendamento com funcionário diferente do responsável

2. **? Conflitos de Horário:**
   - Mesmo funcionário não pode ter 2 agendamentos no mesmo horário
   - Mesmo serviço (com responsável) não pode ter 2 agendamentos simultâneos
   - Mesmo animal não pode ter 2 agendamentos no mesmo horário

3. **? Flexibilidade:**
   - Serviços sem responsável podem ser executados por qualquer funcionário
   - Diferentes serviços podem acontecer simultaneamente (ex: consulta + corte de unhas)

4. **? Cadastro Automático:**
   - Registro de usuário cria automaticamente o tutor vinculado
   - Login retorna token JWT válido

---

## ?? **ENDPOINTS PARA MONITORAMENTO**

- **GET /api/Funcionario** - Listar funcionários
- **GET /api/Servico** - Listar todos os serviços
- **GET /api/Servico/ativos** - Apenas serviços ativos
- **GET /api/Animal** - Listar todos os pets
- **GET /api/Tutor** - Listar tutores
- **GET /api/Agendamento** - Todos os agendamentos
- **GET /api/Agendamento/data/2024-12-10** - Agendamentos por data

---

## ?? **VALIDAÇÃO FINAL**

Após executar todos os testes:

1. ? Sistema deve ter dados completos de funcionários
2. ? Serviços com e sem funcionários responsáveis
3. ? Agendamentos respeitando regras de conflito
4. ? Tutores e pets cadastrados corretamente
5. ? Vendas mistas funcionando
6. ? Todas as validações de negócio operando

**?? SISTEMA TOTALMENTE FUNCIONAL E TESTADO!**

