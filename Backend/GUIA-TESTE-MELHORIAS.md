# ?? GUIA DE TESTE - MELHORIAS SIGA-PET

## ?? SETUP INICIAL

### 1. Resetar e Popular Banco
```sql
-- Execute no SQL Server Management Studio
-- Arquivo: Backend/RESET-BANCO-DADOS.sql
-- OU use o script completo fornecido
```

### 2. Popular via Swagger
```
1. Acesse: https://localhost:7000/swagger
2. Execute: POST /api/Seed/popular-completo  
3. Verifique retorno de sucesso
```

---

## ? TESTE 1: CREDENCIAIS ATUALIZADAS

### Login com Novas Credenciais
```
? Admin: admin@sigapet.com / admin123
? Veterinário: vet@sigapet.com / admin123
? Tosador: tosador@sigapet.com / admin123  
? Atendente: atendente@sigapet.com / admin123
? Cliente: cliente@sigapet.com / admin123
```

### Verificação:
- [ ] Todos os logins funcionam
- [ ] Redirecionamentos corretos (admin ? /admin, cliente ? /)
- [ ] Dados do usuário exibidos corretamente na navbar

---

## ? TESTE 2: COMPRA SEM SER TUTOR

### Cenário A: Admin Comprando (sem tutorId)
```
1. Login: admin@sigapet.com / admin123
2. Ir para /produtos  
3. Adicionar produtos ao carrinho
4. Finalizar compra no dropdown do carrinho
5. Verificar sucesso da compra
```

### Verificação Backend:
```sql
-- Verificar se tutor foi criado automaticamente
SELECT * FROM Tutores WHERE Nome LIKE '%Admin%' AND UsuarioId IS NULL;

-- Verificar venda criada
SELECT * FROM Vendas WHERE TutorId = (SELECT TutorId FROM Tutores WHERE Nome LIKE '%Admin%');
```

### Cenário B: Cliente sem TutorId (caso exista)
- Mesmo fluxo do Cenário A
- Verificar criação automática de tutor

---

## ? TESTE 3: AGENDAMENTO COMPLETO (CRIAR TUTOR + PET)

### Cenário: Cliente Novo Agendando
```
1. Login: cliente@sigapet.com / admin123
2. Ir para /agenda/novo
3. Selecionar serviço (ex: Consulta Veterinária)
4. Escolher data/hora futura válida
5. Marcar "Cadastrar novo pet" 
6. Preencher dados do pet:
   - Nome: "Buddy"
   - Espécie: "Cão"
   - Raça: "Golden"
   - Sexo: "Macho"
7. Salvar agendamento
```

### Verificação Backend:
```sql
-- Verificar tutor criado automaticamente
SELECT * FROM Tutores WHERE Nome LIKE '%Carlos%'; -- Nome do cliente@sigapet.com

-- Verificar animal criado  
SELECT * FROM Animais WHERE Nome = 'Buddy';

-- Verificar agendamento criado
SELECT a.*, an.Nome as PetNome, s.Nome as ServicoNome 
FROM Agendamentos a 
JOIN Animais an ON a.AnimalId = an.AnimalId
JOIN Servicos s ON a.ServicoId = s.ServicoId
WHERE an.Nome = 'Buddy';
```

---

## ?? VERIFICAÇÕES DE INTEGRIDADE

### 1. Endpoint Popular-Completo Inalterado
```
POST /api/Seed/popular-completo
- [ ] Retorna mesmos dados de antes
- [ ] Cria 5 usuários, 3 funcionários, 3 tutores, etc.
- [ ] Não há regressões
```

### 2. Fluxos Existentes Funcionais  
```
- [ ] Admin pode gerenciar produtos/serviços
- [ ] Funcionários podem acessar suas funcionalidades
- [ ] Tutores existentes podem fazer agendamentos normalmente
- [ ] Vendas tradicionais (com tutorId) funcionam
```

### 3. Validações Mantidas
```
- [ ] Não pode agendar no passado
- [ ] Não pode agendar domingos
- [ ] Horário 8h-18h respeitado
- [ ] Conflitos de funcionário detectados
- [ ] Estoque de produtos respeitado
```

---

## ?? NOVOS ENDPOINTS PARA TESTAR

### 1. Venda com Auto-criação de Tutor
```http
POST /api/Venda
Content-Type: application/json

{
  "nomeCliente": "João Teste",
  "emailCliente": "joao.teste@email.com",
  "telefoneCliente": "(11) 99999-8888",
  "enderecoCliente": "Rua Teste, 123",
  "formaPagamento": "Dinheiro",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 2
    }
  ]
}
```

### 2. Agendamento Completo
```http
POST /api/Agendamento/completo  
Content-Type: application/json

{
  "servicoId": 1,
  "dataHora": "2024-12-25T14:30:00",
  "nomeTutor": "Maria Teste",
  "emailTutor": "maria.teste@email.com", 
  "telefoneTutor": "(11) 98765-4321",
  "enderecoTutor": "Av. Teste, 456",
  "nomeAnimal": "Luna",
  "especieAnimal": "Gato", 
  "racaAnimal": "Siamês",
  "sexoAnimal": "Fêmea",
  "pelagemAnimal": "Curta"
}
```

---

## ?? PROBLEMAS CONHECIDOS E SOLUÇÕES

### 1. "Usuário não identificado como tutor"
**Causa**: Frontend ainda esperando tutorId obrigatório
**Solução**: Implementada lógica para criar tutor automaticamente

### 2. "Venda deve ter tutorId"  
**Causa**: Backend antigo exigia tutorId
**Solução**: TutorId agora é opcional no CreateVendaDto

### 3. Conflito de horários
**Causa**: Validação de funcionário ocupado 
**Solução**: Implementada distribuição automática de funcionários

---

## ?? RESULTADOS ESPERADOS

### Após os Testes:
- [x] ? 1. Credenciais atualizadas funcionando
- [x] ? 2. Usuários logados podem comprar sem ser tutor
- [x] ? 3. Agendamento cria tutor+pet automaticamente  
- [x] ? Endpoint popular-completo inalterado
- [x] ? Zero regressões em funcionalidades existentes

### Benefícios Alcançados:
- **UX Melhorada**: Menos barreiras para primeira compra/agendamento
- **Conversão**: Cliente pode comprar/agendar imediatamente após login
- **Flexibilidade**: Suporta clientes avulsos e cadastrados
- **Integridade**: Relacionamentos corretos mantidos no banco