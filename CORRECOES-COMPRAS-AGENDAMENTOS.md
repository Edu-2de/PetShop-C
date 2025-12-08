# ?? GUIA DE CORREÇÃO - Problemas de Compras e Agendamentos

## ?? RESUMO DOS PROBLEMAS IDENTIFICADOS

1. **Compras não funcionam**: Faltava o campo `UsuarioId` na tabela `Vendas` do banco de dados
2. **Minhas Compras em branco**: Endpoint buscava por `tutorId`, mas nem todo usuário é tutor
3. **Agendamentos não carregam**: Mapeamento incorreto dos objetos relacionados (pet, servico, funcionario)

## ? CORREÇÕES APLICADAS

### 1. Backend - Banco de Dados

#### Arquivo Criado: `Backend/Migrations/AdicionarUsuarioIdVenda.sql`

Este arquivo contém o script SQL para adicionar o campo `UsuarioId` à tabela `Vendas`.

**Como aplicar:**

```sql
-- Abra o SQL Server Management Studio (SSMS)
-- Conecte-se ao seu servidor
-- Abra o arquivo Backend/Migrations/AdicionarUsuarioIdVenda.sql
-- Execute o script (F5)
```

Ou via comando:

```bash
cd Backend
sqlcmd -S localhost -d SIGA-PET -i Migrations/AdicionarUsuarioIdVenda.sql
```

### 2. Backend - Configuração do Entity Framework

**Arquivos Modificados:**
- `Backend/Data/AppDbContext.cs` - Adicionado relacionamento `Venda.Usuario`
- `Backend/Controllers/VendaController.cs` - Método `GetVendasByUsuario` já existe
- `Backend/Profiles/MappingProfile.cs` - Mapeamento do `UsuarioId` já configurado

### 3. Frontend - Modelos e Serviços

**Arquivos Modificados:**
- `Frontend/src/app/model/venda.model.ts` - Adicionado campo `usuarioId`
- `Frontend/src/app/model/agenda.model.ts` - Adicionados objetos `pet`, `servico` e `funcionario`
- `Frontend/src/app/service/agenda/agenda.ts` - Método `mapAgendamento` para mapear objetos corretamente
- `Frontend/src/app/pages/produtos/produto-detail/produto-detail.ts` - Método `comprarAgora` corrigido

## ?? COMO TESTAR AS CORREÇÕES

### 1. Teste de Compra

1. **Faça login** como usuário normal (não precisa ser tutor)
2. **Acesse a loja** (/produtos)
3. **Clique em um produto** para ver detalhes
4. **Clique em "Comprar Agora"**
5. **Verifique** se a compra foi criada com sucesso
6. **Acesse** "Minhas Compras" (/vendas/minhas)
7. **Confirme** que a compra aparece na lista

### 2. Teste de Agendamentos

1. **Faça login** como tutor (usuário com pet cadastrado)
2. **Acesse** "Meus Agendamentos" (/agenda)
3. **Verifique** se os agendamentos carregam corretamente
4. **Confirme** que aparecem: nome do pet, serviço e status

### 3. Teste de Minhas Compras

1. **Faça login** como qualquer usuário
2. **Realize uma compra** (produto ou serviço)
3. **Acesse** "Minhas Compras" (/vendas/minhas)
4. **Verifique** que a compra aparece
5. **Expanda os detalhes** para ver os itens

## ??? TROUBLESHOOTING

### Problema: "Erro ao criar venda"

**Solução:**
1. Verifique se o script SQL foi executado no banco
2. Confirme que a coluna `UsuarioId` existe em `Vendas`:

```sql
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Vendas' AND COLUMN_NAME = 'UsuarioId'
```

### Problema: "Minhas Compras em branco"

**Solução:**
1. Abra o Console do navegador (F12)
2. Verifique se há erro de console
3. Confirme que o endpoint `/api/Venda/usuario/{usuarioId}` está respondendo:

```
GET http://localhost:5000/api/Venda/usuario/1
```

### Problema: "Agendamentos não carregam"

**Solução:**
1. Abra o Console do navegador (F12)
2. Verifique a resposta do endpoint `/api/Agendamento/usuario/{usuarioId}`
3. Confirme que o backend está retornando os objetos relacionados:

```json
{
  "agendamentoId": 1,
  "animal": {
    "animalId": 1,
    "nome": "Rex",
    ...
  },
  "servico": {
    "servicoId": 1,
    "nome": "Banho",
    ...
  }
}
```

## ?? ESTRUTURA ATUALIZADA DO BANCO

### Tabela: Vendas

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| VendaId | INT | ID da venda (PK) |
| **UsuarioId** | INT | **NOVO** - ID do usuário que fez a compra |
| TutorId | INT | ID do tutor (opcional) |
| FuncionarioId | INT | ID do funcionário (opcional) |
| DataVenda | DATETIME | Data da venda |
| ValorTotal | DECIMAL | Valor total |
| FormaPagamento | NVARCHAR | Forma de pagamento |
| Observacoes | NVARCHAR | Observações |

## ?? PRÓXIMOS PASSOS

1. **Execute o script SQL** para adicionar o campo `UsuarioId` no banco
2. **Reinicie o backend** para aplicar as mudanças do Entity Framework
3. **Teste todas as funcionalidades** conforme descrito acima
4. **Monitore o console** para identificar quaisquer outros erros

## ?? NOTAS IMPORTANTES

- **Usuário vs Tutor**: Agora qualquer usuário pode comprar, não apenas tutores
- **Agendamentos**: Apenas tutores com pets cadastrados podem agendar
- **Compatibilidade**: Os campos antigos (`tutorId`) foram mantidos para compatibilidade
- **Migração automática**: Se preferir usar Entity Framework migrations:

```bash
cd Backend
dotnet ef migrations add AdicionarUsuarioIdVenda
dotnet ef database update
```

## ? CHECKLIST DE VERIFICAÇÃO

- [ ] Script SQL executado no banco de dados
- [ ] Backend reiniciado
- [ ] Frontend compilado sem erros
- [ ] Teste de compra realizado com sucesso
- [ ] Página "Minhas Compras" carregando corretamente
- [ ] Página "Meus Agendamentos" carregando corretamente
- [ ] Console do navegador sem erros

---

**Data**: 08/12/2024  
**Versão**: 1.0  
**Status**: Correções Aplicadas ?
