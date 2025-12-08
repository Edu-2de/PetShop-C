# ?? GUIA COMPLETO DE DADOS PARA SWAGGER - SIGA PET

Este documento contém todos os dados necessários para testar completamente a API via Swagger.

---

## ?? 1. CADASTRO E AUTENTICAÇÃO

### 1.1 Cadastrar Admin Principal
**POST /api/Auth/register**
```json
{
  "nome": "Administrador Sistema",
  "email": "admin@sigapet.com",
  "senha": "123456",
  "telefone": "51999887766",
  "endereco": "Rua Principal, 123 - Centro"
}
```

### 1.2 Login do Admin
**POST /api/Auth/login**
```json
{
  "email": "admin@sigapet.com",
  "senha": "123456"
}
```
> **Importante:** Copie o `token` retornado e use no botão "Authorize" do Swagger!

---

## ?? 2. CADASTRO DE FUNCIONÁRIOS

### 2.1 Veterinário Principal
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

### 2.2 Tosador Especialista
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

### 2.3 Atendente Recepção
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

### 2.4 Auxiliar Veterinário
**POST /api/Funcionario**
```json
{
  "nome": "Pedro Lima",
  "cargo": "Auxiliar Veterinário",
  "telefone": "51954321098",
  "email": "pedro.auxiliar@sigapet.com",
  "senha": "aux123",
  "dataContratacao": "2024-03-01T08:00:00"
}
```

---

## ?? 3. CADASTRO DE CATEGORIAS

### 3.1 Categoria Alimentação
**POST /api/Categoria**
```json
{
  "nome": "Alimentação",
  "descricao": "Rações, petiscos e suplementos para pets"
}
```

### 3.2 Categoria Higiene
**POST /api/Categoria**
```json
{
  "nome": "Higiene e Beleza",
  "descricao": "Shampoos, condicionadores e produtos de limpeza"
}
```

### 3.3 Categoria Medicamentos
**POST /api/Categoria**
```json
{
  "nome": "Medicamentos",
  "descricao": "Medicamentos veterinários e suplementos"
}
```

### 3.4 Categoria Brinquedos
**POST /api/Categoria**
```json
{
  "nome": "Brinquedos",
  "descricao": "Brinquedos e acessórios para entretenimento"
}
```

### 3.5 Categoria Acessórios
**POST /api/Categoria**
```json
{
  "nome": "Acessórios",
  "descricao": "Coleiras, guias, camas e outros acessórios"
}
```

---

## ?? 4. CADASTRO DE FORNECEDORES

### 4.1 Fornecedor Ração Premier
**POST /api/Fornecedor**
```json
{
  "nome": "Premier Pet Distribuidora",
  "cnpj": "12.345.678/0001-90",
  "telefone": "51333221100",
  "email": "vendas@premierpet.com.br",
  "endereco": "Av. das Indústrias, 500 - Distrito Industrial"
}
```

### 4.2 Fornecedor Medicamentos Vetnil
**POST /api/Fornecedor**
```json
{
  "nome": "Vetnil Indústria Veterinária",
  "cnpj": "98.765.432/0001-12",
  "telefone": "1140001234",
  "email": "comercial@vetnil.com.br",
  "endereco": "Rua Industrial, 1000 - São Paulo - SP"
}
```

### 4.3 Fornecedor Brinquedos PetLove
**POST /api/Fornecedor**
```json
{
  "nome": "PetLove Acessórios",
  "cnpj": "45.678.901/0001-34",
  "telefone": "51377889900",
  "email": "vendas@petlove.com.br",
  "endereco": "Rua dos Pets, 250 - Zona Sul"
}
```

---

## ??? 5. CADASTRO DE PRODUTOS

### 5.1 Ração Premium Cães
**POST /api/Produto**
```json
{
  "nome": "Ração Premier Golden Adulto",
  "descricao": "Ração super premium para cães adultos de todas as raças",
  "preco": 89.90,
  "categoria": 1,
  "fornecedorId": 1,
  "codigoBarras": "7898934567890",
  "estoque": 50,
  "estoqueMinimo": 10,
  "ativo": true
}
```

### 5.2 Shampoo Neutro
**POST /api/Produto**
```json
{
  "nome": "Shampoo Neutro Pet Clean",
  "descricao": "Shampoo neutro para todos os tipos de pelagem",
  "preco": 24.90,
  "categoria": 2,
  "fornecedorId": 3,
  "codigoBarras": "7898934567891",
  "estoque": 30,
  "estoqueMinimo": 5,
  "ativo": true
}
```

### 5.3 Antipulgas
**POST /api/Produto**
```json
{
  "nome": "Antipulgas Advantage Max",
  "descricao": "Tratamento completo contra pulgas e carrapatos",
  "preco": 45.90,
  "categoria": 3,
  "fornecedorId": 2,
  "codigoBarras": "7898934567892",
  "estoque": 25,
  "estoqueMinimo": 8,
  "ativo": true
}
```

### 5.4 Bola de Borracha
**POST /api/Produto**
```json
{
  "nome": "Bola Maciça Colorida",
  "descricao": "Bola de borracha resistente para cães brincarem",
  "preco": 12.90,
  "categoria": 4,
  "fornecedorId": 3,
  "codigoBarras": "7898934567893",
  "estoque": 100,
  "estoqueMinimo": 20,
  "ativo": true
}
```

### 5.5 Coleira Ajustável
**POST /api/Produto**
```json
{
  "nome": "Coleira Ajustável Premium",
  "descricao": "Coleira de nylon ajustável com fivela de segurança",
  "preco": 28.90,
  "categoria": 5,
  "fornecedorId": 3,
  "codigoBarras": "7898934567894",
  "estoque": 40,
  "estoqueMinimo": 10,
  "ativo": true
}
```

### 5.6 Ração Gatos Premium
**POST /api/Produto**
```json
{
  "nome": "Ração Whiskas Gatos Adultos",
  "descricao": "Alimento completo para gatos adultos com sabor salmão",
  "preco": 34.90,
  "categoria": 1,
  "fornecedorId": 1,
  "codigoBarras": "7898934567895",
  "estoque": 35,
  "estoqueMinimo": 8,
  "ativo": true
}
```

---

## ?? 6. CADASTRO DE SERVIÇOS

### 6.1 Consulta Veterinária
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

### 6.2 Banho Simples
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

### 6.3 Tosa Completa
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

### 6.4 Vacinação
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

### 6.5 Cirurgia Castração
**POST /api/Servico**
```json
{
  "nome": "Castração (Porte Médio)",
  "descricao": "Procedimento cirúrgico de castração para cães de porte médio",
  "preco": 350.00,
  "duracaoMinutos": 120,
  "ativo": true,
  "funcionarioResponsavelId": 1
}
```

### 6.6 Limpeza Dental
**POST /api/Servico**
```json
{
  "nome": "Limpeza Dental",
  "descricao": "Profilaxia dental com anestesia",
  "preco": 180.00,
  "duracaoMinutos": 90,
  "ativo": true,
  "funcionarioResponsavelId": 1
}
```

---

## ??????????? 7. CADASTRO DE TUTORES/CLIENTES

### 7.1 Cliente João
**POST /api/Auth/register**
```json
{
  "nome": "João Carlos Silva",
  "email": "joao.cliente@gmail.com",
  "senha": "123456",
  "telefone": "51987123456",
  "endereco": "Rua das Flores, 456 - Bairro Alegre"
}
```

### 7.2 Cliente Maria
**POST /api/Auth/register**
```json
{
  "nome": "Maria Fernanda Costa",
  "email": "maria.cliente@gmail.com",
  "senha": "123456",
  "telefone": "51976234567",
  "endereco": "Av. Central, 789 - Centro"
}
```

### 7.3 Cliente Pedro
**POST /api/Auth/register**
```json
{
  "nome": "Pedro Henrique Santos",
  "email": "pedro.cliente@gmail.com",
  "senha": "123456",
  "telefone": "51965345678",
  "endereco": "Rua do Parque, 321 - Vila Nova"
}
```

---

## ?? 8. CADASTRO DE ANIMAIS/PETS

> **Nota:** Use o token de login dos tutores para cadastrar os pets deles.

### 8.1 Pet do João - Rex
**POST /api/Animal**
```json
{
  "tutorId": 1,
  "nome": "Rex",
  "especie": "Cão",
  "raca": "Labrador",
  "dataNascimento": "2020-03-15T00:00:00",
  "sexo": "Macho",
  "pelagem": "Dourada",
  "observacoes": "Muito dócil e brincalhão. Vacinado em dia."
}
```

### 8.2 Pet da Maria - Luna
**POST /api/Animal**
```json
{
  "tutorId": 2,
  "nome": "Luna",
  "especie": "Gato",
  "raca": "Persa",
  "dataNascimento": "2021-08-20T00:00:00",
  "sexo": "Fêmea",
  "pelagem": "Branca",
  "observacoes": "Gata castrada, muito carinhosa. Alérgica a frutos do mar."
}
```

### 8.3 Pet do Pedro - Thor
**POST /api/Animal**
```json
{
  "tutorId": 3,
  "nome": "Thor",
  "especie": "Cão",
  "raca": "Pastor Alemão",
  "dataNascimento": "2019-12-10T00:00:00",
  "sexo": "Macho",
  "pelagem": "Preta e Marrom",
  "observacoes": "Cão de guarda, bem treinado. Já teve cirurgia no joelho."
}
```

### 8.4 Pet da Maria - Mimi
**POST /api/Animal**
```json
{
  "tutorId": 2,
  "nome": "Mimi",
  "especie": "Gato",
  "raca": "SRD",
  "dataNascimento": "2022-05-05T00:00:00",
  "sexo": "Fêmea",
  "pelagem": "Rajada",
  "observacoes": "Gata resgatada, muito esperta e independente."
}
```

---

## ?? 9. CADASTRO DE AGENDAMENTOS

> **Nota:** Use tokens dos tutores para criar agendamentos dos próprios pets.

### 9.1 Consulta para Rex
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

### 9.2 Tosa para Rex
**POST /api/Agendamento**
```json
{
  "animalId": 1,
  "servicoId": 3,
  "funcionarioId": 2,
  "dataHora": "2024-12-12T09:00:00",
  "status": "Pendente",
  "observacoes": "Primeira tosa do Rex, ele é bem dócil"
}
```

### 9.3 Consulta para Luna
**POST /api/Agendamento**
```json
{
  "animalId": 2,
  "servicoId": 1,
  "funcionarioId": 1,
  "dataHora": "2024-12-11T15:30:00",
  "status": "Confirmado",
  "observacoes": "Gata com coceira excessiva, investigar possível alergia"
}
```

### 9.4 Vacinação para Thor
**POST /api/Agendamento**
```json
{
  "animalId": 3,
  "servicoId": 4,
  "funcionarioId": 1,
  "dataHora": "2024-12-13T10:00:00",
  "status": "Pendente",
  "observacoes": "Reforço da vacina V10"
}
```

---

## ?? 10. EXEMPLOS DE VENDAS

> **Nota:** Use token de funcionário/admin para registrar vendas.

### 10.1 Venda Produtos para João
**POST /api/Venda**
```json
{
  "tutorId": 1,
  "funcionarioId": 3,
  "formaPagamento": "Cartão de Crédito",
  "observacoes": "Cliente levou ração e antipulgas",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 1,
      "precoUnitario": 89.90
    },
    {
      "produtoId": 3,
      "quantidade": 1,
      "precoUnitario": 45.90
    }
  ]
}
```

### 10.2 Venda Mista (Produto + Serviço) para Maria
**POST /api/Venda**
```json
{
  "tutorId": 2,
  "funcionarioId": 3,
  "formaPagamento": "PIX",
  "observacoes": "Banho da Luna + shampoo especial",
  "itens": [
    {
      "servicoId": 2,
      "quantidade": 1,
      "precoUnitario": 35.00
    },
    {
      "produtoId": 2,
      "quantidade": 1,
      "precoUnitario": 24.90
    }
  ]
}
```

---

## ?? 11. SEQUÊNCIA RECOMENDADA PARA TESTES

1. **Cadastrar Admin e fazer login** (seção 1)
2. **Cadastrar todos os funcionários** (seção 2)
3. **Criar todas as categorias** (seção 3)
4. **Cadastrar fornecedores** (seção 4)
5. **Cadastrar produtos** (seção 5)
6. **Criar serviços com funcionários responsáveis** (seção 6)
7. **Cadastrar clientes/tutores** (seção 7)
8. **Login com clientes e cadastrar pets** (seção 8)
9. **Criar agendamentos** (seção 9)
10. **Registrar vendas** (seção 10)

---

## ?? OBSERVAÇÕES IMPORTANTES

- **Tokens JWT:** Sempre use o token do usuário logado no botão "Authorize"
- **IDs sequenciais:** Os IDs começam em 1 e são incrementais
- **Funcionários responsáveis:** Cada serviço pode ter um funcionário responsável específico
- **Conflitos de horário:** O sistema valida conflitos baseado em funcionário + serviço + horário
- **Status válidos:** Pendente, Confirmado, EmAndamento, Concluido, Cancelado

---

**?? Com estes dados, você terá um sistema completo para testar todas as funcionalidades!**

