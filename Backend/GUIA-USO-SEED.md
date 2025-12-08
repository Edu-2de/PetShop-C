# Guia de Uso - Sistema de Seed de Dados

## Visão Geral

O sistema de seed foi completamente recriado de forma profissional e organizada. Use os endpoints do `SeedController` para popular e gerenciar dados de teste no banco de dados.

---

## Passo 1: Limpar o Banco de Dados

### SQL Server Management Studio

Execute o script SQL para limpar o banco:

```sql
-- Arquivo: Backend/RESET-BANCO-DADOS.sql
-- Execute no SQL Server Management Studio
```

O script irá:
- Deletar todos os dados de todas as tabelas
- Resetar os identity seeds para 1
- Exibir instruções de próximos passos

---

## Passo 2: Popular via Swagger

### Acesse o Swagger

```
https://localhost:7000/swagger
```

### Opção A: Popular Tudo de Uma Vez (RECOMENDADO)

1. Localize a seção `Seed` no Swagger
2. Expanda o endpoint `POST /api/Seed/popular-completo`
3. Clique em "Try it out"
4. Clique em "Execute"

**Resultado:**
- Cria 5 usuários
- Cria 3 funcionários
- Cria 3 tutores
- Cria 6 categorias
- Cria 4 fornecedores
- Cria 15 produtos
- Cria 9 imagens
- Cria 8 serviços
- Cria 13 vínculos serviço-funcionário
- Cria 6 animais/pets

**Tempo estimado:** 5-10 segundos

### Opção B: Popular Passo a Passo

Execute os endpoints na ordem:

1. `POST /api/Seed/usuarios` - Criar usuários
2. `POST /api/Seed/funcionarios` - Criar funcionários
3. `POST /api/Seed/tutores` - Criar tutores
4. `POST /api/Seed/categorias` - Criar categorias
5. `POST /api/Seed/fornecedores` - Criar fornecedores
6. `POST /api/Seed/produtos` - Criar produtos
7. `POST /api/Seed/imagens` - Criar imagens de produtos
8. `POST /api/Seed/servicos` - Criar serviços
9. `POST /api/Seed/vinculos` - Vincular serviços aos funcionários
10. `POST /api/Seed/animais` - Criar animais/pets

---

## Passo 3: Verificar Status

### Via Swagger

```
GET /api/Seed/status
```

Retorna:
- Quantidade de registros em cada tabela
- Total de registros no banco
- Se o banco está vazio ou populado
- Recomendação de ação

---

## Credenciais Criadas

### Usuário Admin
- **Email:** admin@sigapet.com
- **Senha:** admin123
- **Tipo:** Admin

### Veterinário
- **Email:** vet@sigapet.com
- **Senha:** admin123
- **Tipo:** Funcionario

### Tosador
- **Email:** tosador@sigapet.com
- **Senha:** admin123
- **Tipo:** Funcionario

### Atendente
- **Email:** atendente@sigapet.com
- **Senha:** admin123
- **Tipo:** Funcionario

### Cliente
- **Email:** cliente@example.com
- **Senha:** admin123
- **Tipo:** Tutor

---

## Dados Criados

### Usuários (5)
- 1 Admin
- 3 Funcionários (Veterinário, Tosador, Atendente)
- 1 Cliente/Tutor

### Funcionários (3)
- Dr. João Silva (Veterinário)
- Maria Santos (Tosador)
- Pedro Oliveira (Atendente)

### Tutores (3)
- Admin User (com login)
- Carlos Silva (com login)
- Ana Paula Costa (sem login - para testes de agendamento)

### Categorias (6)
- Alimentos
- Higiene
- Brinquedos
- Medicamentos
- Acessórios
- Camas e Casinhas

### Fornecedores (4)
- PetFood Distribuidora
- Higiene Pet Brasil
- Brinquedos e Cia
- VetMed Suprimentos

### Produtos (15)
1. Ração Premium Cães 15kg - R$ 189,90
2. Ração Premium Gatos 5kg - R$ 95,90
3. Ração Filhotes 3kg - R$ 78,50
4. Shampoo Neutro 500ml - R$ 42,90
5. Condicionador Pelos Longos 500ml - R$ 48,90
6. Kit Escova + Pente - R$ 35,90
7. Bola de Borracha Resistente - R$ 29,90
8. Corda para Morder 3 Nós - R$ 24,90
9. Arranhador para Gatos 60cm - R$ 149,90
10. Antipulgas e Carrapatos - R$ 68,90
11. Vermífugo Comprimido - R$ 45,90
12. Suplemento Vitamínico - R$ 89,90
13. Coleira Ajustável Nylon - R$ 38,90
14. Guia Retrátil 5m - R$ 79,90
15. Cama Ortopédica Grande - R$ 259,90

### Serviços (8)
1. Consulta Veterinária - R$ 180,00 (60 min)
2. Banho Simples - R$ 65,00 (40 min)
3. Banho e Tosa - R$ 120,00 (90 min)
4. Tosa Higiênica - R$ 80,00 (50 min)
5. Vacinação Múltipla - R$ 120,00 (30 min)
6. Limpeza de Orelhas - R$ 35,00 (20 min)
7. Corte de Unhas - R$ 30,00 (15 min)
8. Aplicação Antipulgas - R$ 50,00 (20 min)

### Animais/Pets (6)
- **Rex** - Cão Labrador (Tutor: Carlos Silva)
- **Mimi** - Gato Siamês (Tutor: Carlos Silva)
- **Thor** - Cão Rottweiler (Tutor: Carlos Silva)
- **Luna** - Gato Persa (Tutor: Ana Paula Costa)
- **Spike** - Cão Pug (Tutor: Ana Paula Costa)
- **Mel** - Gato SRD (Tutor: Ana Paula Costa)

---

## Fluxo Completo de Uso

### 1. Reset Completo

```bash
# Execute o script SQL
Backend/RESET-BANCO-DADOS.sql
```

### 2. Popular Banco

```bash
# Via Swagger
POST /api/Seed/popular-completo
```

### 3. Verificar Status

```bash
GET /api/Seed/status
```

### 4. Fazer Login

```bash
POST /api/Auth/login
{
  "email": "admin@sigapet.com",
  "senha": "admin123"
}
```

### 5. Testar Endpoints

Use o token JWT retornado para testar os demais endpoints.

---

## Limpar Banco via API

### Swagger

```
DELETE /api/Seed/limpar
```

**ATENÇÃO:** Esta operação remove TODOS os dados do banco!

---

## Tratamento de Erros

### Erro: "Usuarios ja existem"

**Solução:** Execute `DELETE /api/Seed/limpar` antes de popular novamente.

### Erro: "Banco ja possui dados"

**Solução:** Execute `DELETE /api/Seed/limpar` ou o script SQL `RESET-BANCO-DADOS.sql`.

### Erro ao deletar com foreign keys

O sistema foi projetado para:
1. Desabilitar constraints temporariamente
2. Deletar dados na ordem correta
3. Reabilitar constraints
4. Resetar identity seeds

Se ainda assim houver erro, execute o script SQL manualmente.

---

## Endpoints Disponíveis

### Limpeza
- `DELETE /api/Seed/limpar` - Limpar todo o banco

### Criação Individual
- `POST /api/Seed/usuarios` - Criar usuários (Passo 1)
- `POST /api/Seed/funcionarios` - Criar funcionários (Passo 2)
- `POST /api/Seed/tutores` - Criar tutores (Passo 3)
- `POST /api/Seed/categorias` - Criar categorias (Passo 4)
- `POST /api/Seed/fornecedores` - Criar fornecedores (Passo 5)
- `POST /api/Seed/produtos` - Criar produtos (Passo 6)
- `POST /api/Seed/imagens` - Criar imagens (Passo 7)
- `POST /api/Seed/servicos` - Criar serviços (Passo 8)
- `POST /api/Seed/vinculos` - Criar vínculos (Passo 9)
- `POST /api/Seed/animais` - Criar animais (Passo 10)

### Popular Completo
- `POST /api/Seed/popular-completo` - Executar todos os passos

### Status
- `GET /api/Seed/status` - Verificar status do banco

---

## Notas Importantes

1. **Ordem de Execução:** Os passos devem ser executados na ordem correta devido às dependências de foreign keys.

2. **Validações:** Cada endpoint valida se os dados já existem antes de criar novos.

3. **Transações:** Todas as operações usam transações para garantir consistência.

4. **Senha Padrão:** Todas as senhas criadas são `admin123` (hash BCrypt).

5. **Dados Realistas:** Os dados criados são realistas e úteis para testes completos do sistema.

---

## Troubleshooting

### Problema: Não consigo limpar o banco

**Solução:** Execute o script SQL `RESET-BANCO-DADOS.sql` no SQL Server Management Studio.

### Problema: Erro de foreign key ao deletar

**Solução:** O sistema desabilita constraints temporariamente. Se persistir, verifique se há dados manualmente inseridos que não seguem a estrutura.

### Problema: Token JWT expirado

**Solução:** Faça login novamente em `POST /api/Auth/login` para obter novo token.

### Problema: Produtos sem imagens no frontend

**Solução:** As imagens são URLs placeholder. Para imagens reais, adicione arquivos em `Frontend/src/assets/images/products/`.

---

## Próximos Passos

Após popular o banco:

1. ? Fazer login no sistema
2. ? Testar criação de agendamentos
3. ? Testar carrinho de compras
4. ? Testar gestão de produtos
5. ? Testar gestão de serviços
6. ? Testar relatórios e dashboards

---

## Suporte

Para problemas ou dúvidas:
- Verifique os logs do backend
- Consulte a documentação no Swagger
- Revise este guia

**Data de Criação:** 07/12/2024  
**Versão:** 2.0.0  
**Status:** Operacional
