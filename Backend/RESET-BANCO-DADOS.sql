-- =====================================================
-- ?? SCRIPT PARA LIMPAR O BANCO DE DADOS
-- =====================================================
-- Execute este script no SQL Server Management Studio
-- para resetar completamente o banco de dados
--
-- ?? ATENÇÃO: Este script irá DELETAR TODOS OS DADOS!
-- =====================================================

PRINT '?? Iniciando limpeza do banco de dados...'
PRINT ''

-- 1. DELETAR TODAS AS TABELAS (respeitando chaves estrangeiras)
PRINT '?? Deletando dados das tabelas...'

DELETE FROM [ItensVenda];
PRINT '  ? ItensVenda'

DELETE FROM [Vendas];
PRINT '  ? Vendas'

DELETE FROM [Agendamentos];
PRINT '  ? Agendamentos'

DELETE FROM [ServicoFuncionarios];
PRINT '  ? ServicoFuncionarios'

DELETE FROM [RegistroProntuario];
PRINT '  ? RegistroProntuario'

DELETE FROM [Servicos];
PRINT '  ? Servicos'

DELETE FROM [Animais];
PRINT '  ? Animais'

DELETE FROM [ProdutoImagens];
PRINT '  ? ProdutoImagens'

DELETE FROM [Produtos];
PRINT '  ? Produtos'

DELETE FROM [Categorias];
PRINT '  ? Categorias'

DELETE FROM [Fornecedores];
PRINT '  ? Fornecedores'

DELETE FROM [Funcionarios];
PRINT '  ? Funcionarios'

DELETE FROM [Tutores];
PRINT '  ? Tutores'

DELETE FROM [Usuarios];
PRINT '  ? Usuarios'

PRINT ''
PRINT '?? Resetando identidades das tabelas...'

-- 2. RESEED para voltar ao 1
DBCC CHECKIDENT ('ItensVenda', RESEED, 0);
DBCC CHECKIDENT ('Vendas', RESEED, 0);
DBCC CHECKIDENT ('Agendamentos', RESEED, 0);
DBCC CHECKIDENT ('RegistroProntuario', RESEED, 0);
DBCC CHECKIDENT ('Servicos', RESEED, 0);
DBCC CHECKIDENT ('Animais', RESEED, 0);
DBCC CHECKIDENT ('ProdutoImagens', RESEED, 0);
DBCC CHECKIDENT ('Produtos', RESEED, 0);
DBCC CHECKIDENT ('Categorias', RESEED, 0);
DBCC CHECKIDENT ('Fornecedores', RESEED, 0);
DBCC CHECKIDENT ('Funcionarios', RESEED, 0);
DBCC CHECKIDENT ('Tutores', RESEED, 0);
DBCC CHECKIDENT ('Usuarios', RESEED, 0);

PRINT ''
PRINT '? Banco de dados limpo com sucesso!'
PRINT ''
PRINT '???????????????????????????????????????????????????????'
PRINT '?? PRÓXIMOS PASSOS:'
PRINT '???????????????????????????????????????????????????????'
PRINT ''
PRINT '1??  Acesse o Swagger em: https://localhost:7000/swagger'
PRINT ''
PRINT '2??  Expanda a seção: Seed - Controller para popular banco'
PRINT ''
PRINT '3??  Execute um dos endpoints:'
PRINT ''
PRINT '    ?? OPÇÃO RÁPIDA (Recomendado):'
PRINT '       POST /api/Seed/popular-completo'
PRINT '       ? Popula todo o banco em um único clique!'
PRINT ''
PRINT '    ?? OPÇÃO PASSO A PASSO:'
PRINT '       Execute os endpoints na ordem:'
PRINT '       1. POST /api/Seed/1-criar-usuarios'
PRINT '       2. POST /api/Seed/2-criar-funcionarios'
PRINT '       3. POST /api/Seed/3-criar-tutores'
PRINT '       4. POST /api/Seed/4-criar-categorias'
PRINT '       5. POST /api/Seed/5-criar-fornecedores'
PRINT '       6. POST /api/Seed/6-criar-produtos'
PRINT '       7. POST /api/Seed/7-criar-imagens-produtos'
PRINT '       8. POST /api/Seed/8-criar-servicos'
PRINT '       9. POST /api/Seed/9-vincular-servicos-funcionarios'
PRINT '       10. POST /api/Seed/10-criar-animais'
PRINT ''
PRINT '4??  Verifique o status:'
PRINT '       GET /api/Seed/status'
PRINT ''
PRINT '???????????????????????????????????????????????????????'
PRINT '?? CREDENCIAIS DE TESTE (após popular):'
PRINT '???????????????????????????????????????????????????????'
PRINT ''
PRINT '   ?? Admin:'
PRINT '      Email: admin@sigapet.com'
PRINT '      Senha: admin123'
PRINT ''
PRINT '   ????? Veterinário:'
PRINT '      Email: vet@sigapet.com'
PRINT '      Senha: admin123'
PRINT ''
PRINT '   ?? Tosador:'
PRINT '      Email: tosador@sigapet.com'
PRINT '      Senha: admin123'
PRINT ''
PRINT '   ????? Atendente:'
PRINT '      Email: atendente@sigapet.com'
PRINT '      Senha: admin123'
PRINT ''
PRINT '   ????? Cliente:'
PRINT '      Email: cliente@example.com'
PRINT '      Senha: admin123'
PRINT ''
PRINT '???????????????????????????????????????????????????????'
PRINT '?? DADOS QUE SERÃO CRIADOS:'
PRINT '???????????????????????????????????????????????????????'
PRINT ''
PRINT '   • 5 Usuários (Login)'
PRINT '   • 3 Funcionários'
PRINT '   • 3 Tutores'
PRINT '   • 6 Categorias de Produtos'
PRINT '   • 4 Fornecedores'
PRINT '   • 15 Produtos variados'
PRINT '   • 9 Imagens de produtos'
PRINT '   • 8 Serviços (consultas, banho, tosa, etc)'
PRINT '   • 14 Vínculos Serviço-Funcionário'
PRINT '   • 6 Animais/Pets'
PRINT ''
PRINT '???????????????????????????????????????????????????????'
PRINT ''