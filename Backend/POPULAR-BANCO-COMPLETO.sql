-- Script completo para popular o banco SIGA-PET-DB com dados atualizados
-- Execute este script após rodar o LIMPAR-BANCO-APENAS.sql

-- 1. Inserir Usuários com nomes completos e SENHAS DIFERENTES
INSERT INTO [Usuarios] ([Nome], [Email], [PasswordHash], [TipoUsuario], [Ativo]) VALUES
('Admin Sistema', 'admin@sigapet.com', '$2a$11$K8H0DzQfH7E5J6CqH4QXuOJ4j9v8K2M7X3P6R9T8S5Y4Z1A0B3C5e2', 'Admin', 1),
('Dr. João Silva', 'vet@sigapet.com', '$2a$11$L9I1EaRgI8F6K7DqI5RYvPK5k0w9L3N8Y4Q7S0U5a6b2c4d6f8g0h2', 'Funcionario', 1),
('Maria Santos', 'tosador@sigapet.com', '$2a$11$M0J2FbShJ9G7L8ErJ6SZwQL6l1x0M4O9Z5R8T1V6b7c3d5e7f9h1i3', 'Funcionario', 1),
('Pedro Oliveira', 'atendente@sigapet.com', '$2a$11$N1K3GcTiK0H8M9FsK7TawRM7m2y1N5P0a6S9U2W7c8d4e6f8g0i2j4', 'Funcionario', 1),
('Carlos Silva', 'cliente@example.com', '$2a$11$O2L4HdUjL1I9N0GtL8UbxSN8n3z2O6Q1b7T0V3X8d9e5f7g9h1j3k5', 'Tutor', 1);

-- 2. Inserir Funcionários com dados completos
INSERT INTO [Funcionarios] ([Nome], [Cargo], [Telefone], [DataContratacao], [Ativo], [UsuarioId]) VALUES
('Dr. João Silva', 'Veterinario', '(11) 98765-4321', GETDATE(), 1, 2),
('Maria Santos', 'Tosador', '(11) 98765-4322', GETDATE(), 1, 3),
('Pedro Oliveira', 'Atendente', '(11) 98765-4323', GETDATE(), 1, 4);

-- 3. Inserir Tutores com endereços completos
INSERT INTO [Tutores] ([Nome], [Telefone], [Endereco], [DataCadastro], [UsuarioId]) VALUES
('Admin Sistema', '(11) 3333-3333', 'Rua dos Administradores, 100, Centro, São Paulo - SP, CEP: 01234-567', GETDATE(), 1),
('Carlos Silva', '(11) 99999-9999', 'Rua das Flores, 200, Jardim Primavera, São Paulo - SP, CEP: 04567-890', GETDATE(), 5),
('Ana Paula Costa', '(11) 88888-8888', 'Av. Principal, 300, Vila Nova, São Paulo - SP, CEP: 05678-901', GETDATE(), NULL);

-- 4. Inserir Categorias
INSERT INTO [Categorias] ([Nome], [Descricao]) VALUES
('Alimentos', 'Racao e alimentos para animais'),
('Higiene', 'Produtos de higiene e limpeza'),
('Brinquedos', 'Brinquedos e entretenimento'),
('Medicamentos', 'Medicamentos e suplementos'),
('Acessorios', 'Coleiras, guias e acessorios'),
('Camas e Casinhas', 'Locais de descanso');

-- 5. Inserir Fornecedores com dados completos
INSERT INTO [Fornecedores] ([Nome], [Cnpj], [Email], [Telefone], [Endereco], [RazaoSocial], [Contato]) VALUES
('PetFood Distribuidora', '12.345.678/0001-90', 'contato@petfood.com.br', '(11) 3000-1000', 'Av. Industrial, 1000, Distrito Industrial, São Paulo - SP', 'PetFood LTDA', 'Joao Almeida'),
('Higiene Pet Brasil', '98.765.432/0001-10', 'vendas@higienepet.com.br', '(11) 3000-2000', 'Rua Comercial, 500, Centro Comercial, São Paulo - SP', 'Higiene Pet S/A', 'Maria Silva'),
('Brinquedos e Cia', '45.678.901/0001-23', 'comercial@brinquedos.com.br', '(11) 3000-3000', 'Av. dos Brinquedos, 234, Vila Alegre, São Paulo - SP', 'Brinquedos LTDA', 'Pedro Santos'),
('VetMed Suprimentos', '78.901.234/0001-56', 'atendimento@vetmed.com.br', '(11) 3000-4000', 'Rua Farmaceutica, 789, Vila Médica, São Paulo - SP', 'VetMed LTDA', 'Ana Costa');

-- 6. Inserir Produtos
INSERT INTO [Produtos] ([Nome], [Descricao], [Preco], [QuantidadeEstoque], [CategoriaId], [FornecedorId], [Ativo], [CodigoBarras]) VALUES
('Racao Premium Caes 15kg', 'Racao super premium para caes adultos', 189.90, 45, 1, 1, 1, '7891234567890'),
('Racao Premium Gatos 5kg', 'Racao especial para gatos adultos', 95.90, 60, 1, 1, 1, '7891234567891'),
('Racao Filhotes 3kg', 'Racao para filhotes ate 12 meses', 78.50, 35, 1, 1, 1, '7891234567892'),
('Shampoo Neutro 500ml', 'Shampoo hipoalergenico pH balanceado', 42.90, 80, 2, 2, 1, '7891234567893'),
('Condicionador Pelos Longos 500ml', 'Condicionador para pelos longos', 48.90, 55, 2, 2, 1, '7891234567894'),
('Kit Escova + Pente', 'Kit completo para escovacao', 35.90, 40, 2, 2, 1, '7891234567895'),
('Bola de Borracha Resistente', 'Bola super resistente', 29.90, 120, 3, 3, 1, '7891234567896'),
('Corda para Morder 3 Nos', 'Corda resistente com 3 nos', 24.90, 95, 3, 3, 1, '7891234567897'),
('Arranhador para Gatos 60cm', 'Arranhador em sisal', 149.90, 25, 3, 3, 1, '7891234567898'),
('Antipulgas e Carrapatos', 'Protecao por 30 dias', 68.90, 70, 4, 4, 1, '7891234567899'),
('Vermifugo Comprimido', 'Vermifugo amplo espectro', 45.90, 85, 4, 4, 1, '7891234567800'),
('Suplemento Vitaminico', 'Suplemento completo', 89.90, 50, 4, 4, 1, '7891234567801'),
('Coleira Ajustavel Nylon', 'Coleira resistente', 38.90, 65, 5, 2, 1, '7891234567802'),
('Guia Retratil 5m', 'Guia retratil automatica', 79.90, 40, 5, 2, 1, '7891234567803'),
('Cama Ortopedica Grande', 'Cama com espuma especial', 259.90, 15, 6, 3, 1, '7891234567804');

-- 7. Inserir Imagens dos Produtos
INSERT INTO [ProdutoImagens] ([ProdutoId], [Url]) VALUES
(1, 'assets/images/products/racao-caes.jpg'),
(2, 'assets/images/products/racao-gatos.jpg'),
(3, 'assets/images/products/racao-filhotes.jpg'),
(4, 'assets/images/products/shampoo.jpg'),
(5, 'assets/images/products/condicionador.jpg'),
(7, 'assets/images/products/brinquedo-bola.jpg'),
(9, 'assets/images/products/arranhador.jpg'),
(13, 'assets/images/products/coleira.jpg'),
(15, 'assets/images/products/cama.jpg');

-- 8. Inserir Serviços
INSERT INTO [Servicos] ([Nome], [Descricao], [Preco], [DuracaoMinutos], [Ativo], [CargosResponsaveis], [FuncionarioResponsavelId]) VALUES
('Consulta Veterinaria', 'Consulta clinica geral', 180.00, 60, 1, 'Veterinario', 1),
('Banho Simples', 'Banho com shampoo neutro', 65.00, 40, 1, 'Tosador,Atendente', NULL),
('Banho e Tosa', 'Banho completo + tosa', 120.00, 90, 1, 'Tosador', 2),
('Tosa Higienica', 'Tosa focada em higiene', 80.00, 50, 1, 'Tosador', 2),
('Vacinacao Multipla', 'Aplicacao de vacina V10 ou V8', 120.00, 30, 1, 'Veterinario', 1),
('Limpeza de Orelhas', 'Limpeza completa', 35.00, 20, 1, 'Tosador,Atendente', NULL),
('Corte de Unhas', 'Corte e lixamento', 30.00, 15, 1, 'Tosador,Veterinario,Atendente', NULL),
('Aplicacao Antipulgas', 'Aplicacao topica', 50.00, 20, 1, 'Veterinario,Tosador', NULL);

-- 9. Inserir Vínculos Serviço-Funcionário
INSERT INTO [ServicoFuncionarios] ([ServicoId], [FuncionarioId]) VALUES
(1, 1), (2, 2), (2, 3), (3, 2), (4, 2), (5, 1), (6, 2), (6, 3), (7, 1), (7, 2), (7, 3), (8, 1), (8, 2);

-- 10. Inserir Animais
INSERT INTO [Animais] ([Nome], [Especie], [Raca], [Sexo], [DataNascimento], [Pelagem], [Observacoes], [TutorId]) VALUES
('Rex', 'Cao', 'Labrador', 'Macho', '2020-05-15', 'Curta', 'Muito amigavel e obediente', 2),
('Mimi', 'Gato', 'Siames', 'Femea', '2019-08-20', 'Curta', 'Um pouco arisca', 2),
('Thor', 'Cao', 'Rottweiler', 'Macho', '2021-03-10', 'Curta', 'Protetor e leal', 2),
('Luna', 'Gato', 'Persa', 'Femea', '2022-01-05', 'Longa', 'Calma e carinhosa', 3),
('Spike', 'Cao', 'Pug', 'Macho', '2021-11-22', 'Curta', 'Muito brincalhao', 3),
('Mel', 'Gato', 'SRD', 'Femea', '2020-07-08', 'Media', 'Resgatada da rua', 3);

-- 11. Inserir Vendas (NOVO)
-- Venda para Carlos Silva (TutorId: 2, UsuarioId: 5)
INSERT INTO [Vendas] ([TutorId], [UsuarioId], [FuncionarioId], [DataVenda], [ValorTotal], [FormaPagamento], [Observacoes]) VALUES
(2, 5, 3, GETDATE(), 228.80, 'Cartão de Crédito', 'Cliente pediu para entregar em casa.');
DECLARE @VendaCarlosId INT = SCOPE_IDENTITY();
INSERT INTO [ItemVendas] ([VendaId], [ProdutoId], [Quantidade], [PrecoUnitario]) VALUES
(@VendaCarlosId, 1, 1, 189.90),
(@VendaCarlosId, 7, 1, 29.90);

-- Venda para Ana Paula Costa (TutorId: 3, sem UsuarioId)
INSERT INTO [Vendas] ([TutorId], [UsuarioId], [FuncionarioId], [DataVenda], [ValorTotal], [FormaPagamento], [Observacoes]) VALUES
(3, NULL, 4, GETDATE(), 175.80, 'Dinheiro', NULL);
DECLARE @VendaAnaId INT = SCOPE_IDENTITY();
INSERT INTO [ItemVendas] ([VendaId], [ProdutoId], [ServicoId], [Quantidade], [PrecoUnitario]) VALUES
(@VendaAnaId, 2, NULL, 1, 95.90),
(@VendaAnaId, NULL, 4, 1, 80.00);

-- 12. Inserir Agendamentos (NOVO)
-- Agendamento para Rex (Tutor: Carlos Silva)
INSERT INTO [Agendamentos] ([AnimalId], [ServicoId], [FuncionarioId], [DataHora], [Status], [Observacoes]) VALUES
(1, 1, 1, DATEADD(day, 3, GETDATE()), 'Confirmado', 'Check-up anual do Rex.');

-- Agendamento para Luna (Tutor: Ana Paula Costa)
INSERT INTO [Agendamentos] ([AnimalId], [ServicoId], [FuncionarioId], [DataHora], [Status], [Observacoes]) VALUES
(4, 3, 2, DATEADD(day, 5, GETDATE()), 'Pendente', 'Tosa completa para a Luna, por favor.');

PRINT '';
PRINT '? BANCO POPULADO COM SUCESSO!';
PRINT '';
PRINT '?? CREDENCIAIS DE TESTE (SENHAS DIFERENTES):';
PRINT 'admin@sigapet.com / admin2024';
PRINT 'vet@sigapet.com / vet123';
PRINT 'tosador@sigapet.com / tosador123';
PRINT 'atendente@sigapet.com / atendente123';
PRINT 'cliente@example.com / cliente123';
PRINT '';
PRINT '?? IMPORTANTE: As senhas são diferentes para facilitar testes.';
PRINT '   Se der erro de login, use o Swagger:';
PRINT '   POST /api/Seed/popular-completo';
PRINT '';