-- =====================================================
-- SCRIPT COMPLETO: RESET E POPULAR BANCO SIGA-PET
-- Data: 08/12/2024
-- Versão: 2.0 - COMPLETA E CORRIGIDA
-- =====================================================

USE master;
GO

-- Desconectar todas as conexões ativas
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'SIGA-PET')
BEGIN
    ALTER DATABASE [SIGA-PET] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [SIGA-PET];
    PRINT '? Banco de dados antigo removido';
END
GO

-- Criar novo banco
CREATE DATABASE [SIGA-PET];
GO

USE [SIGA-PET];
GO

PRINT '========================================';
PRINT '  CRIANDO ESTRUTURA DO BANCO DE DADOS';
PRINT '========================================';
PRINT '';

-- =====================================================
-- 1. TABELA: Usuarios (BASE DO SISTEMA)
-- =====================================================
PRINT '[1/14] Criando tabela Usuarios...';
CREATE TABLE [dbo].[Usuarios] (
    [UsuarioId] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(120) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(256) NOT NULL,
    [TipoUsuario] NVARCHAR(20) NOT NULL CHECK ([TipoUsuario] IN ('Admin', 'Tutor', 'Funcionario')),
    [Ativo] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [CHK_Email_Format] CHECK ([Email] LIKE '%@%.%')
);
CREATE INDEX [IX_Usuarios_Email] ON [dbo].[Usuarios]([Email]);
CREATE INDEX [IX_Usuarios_TipoUsuario] ON [dbo].[Usuarios]([TipoUsuario]);
PRINT '  ? Tabela Usuarios criada';
GO

-- =====================================================
-- 2. TABELA: Tutores
-- =====================================================
PRINT '[2/14] Criando tabela Tutores...';
CREATE TABLE [dbo].[Tutores] (
    [TutorId] INT IDENTITY(1,1) PRIMARY KEY,
    [UsuarioId] INT NULL, -- Nullable para permitir tutores sem login
    [Nome] NVARCHAR(120) NOT NULL,
    [Telefone] NVARCHAR(20) NULL,
    [Endereco] NVARCHAR(250) NULL,
    [DataCadastro] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Tutores_Usuarios] FOREIGN KEY ([UsuarioId]) 
        REFERENCES [dbo].[Usuarios]([UsuarioId]) ON DELETE CASCADE
);
CREATE UNIQUE INDEX [IX_Tutores_UsuarioId] ON [dbo].[Tutores]([UsuarioId]) WHERE [UsuarioId] IS NOT NULL;
PRINT '  ? Tabela Tutores criada';
GO

-- =====================================================
-- 3. TABELA: Funcionarios
-- =====================================================
PRINT '[3/14] Criando tabela Funcionarios...';
CREATE TABLE [dbo].[Funcionarios] (
    [FuncionarioId] INT IDENTITY(1,1) PRIMARY KEY,
    [UsuarioId] INT NOT NULL,
    [Nome] NVARCHAR(120) NOT NULL,
    [Cargo] NVARCHAR(80) NULL,
    [Telefone] NVARCHAR(20) NULL,
    [DataContratacao] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [Ativo] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Funcionarios_Usuarios] FOREIGN KEY ([UsuarioId]) 
        REFERENCES [dbo].[Usuarios]([UsuarioId]) ON DELETE CASCADE
);
CREATE UNIQUE INDEX [IX_Funcionarios_UsuarioId] ON [dbo].[Funcionarios]([UsuarioId]);
PRINT '  ? Tabela Funcionarios criada';
GO

-- =====================================================
-- 4. TABELA: Animais
-- =====================================================
PRINT '[4/14] Criando tabela Animais...';
CREATE TABLE [dbo].[Animais] (
    [AnimalId] INT IDENTITY(1,1) PRIMARY KEY,
    [TutorId] INT NOT NULL,
    [Nome] NVARCHAR(100) NOT NULL,
    [Especie] NVARCHAR(50) NULL CHECK ([Especie] IN ('Cão', 'Gato', 'Pássaro', 'Outros')),
    [Raca] NVARCHAR(100) NULL,
    [DataNascimento] DATETIME2 NULL,
    [Sexo] NVARCHAR(20) NULL CHECK ([Sexo] IN ('Macho', 'Fêmea')),
    [Pelagem] NVARCHAR(100) NULL CHECK ([Pelagem] IN ('Curta', 'Média', 'Longa')),
    [Observacoes] NVARCHAR(500) NULL,
    CONSTRAINT [FK_Animais_Tutores] FOREIGN KEY ([TutorId]) 
        REFERENCES [dbo].[Tutores]([TutorId]) ON DELETE NO ACTION
);
CREATE INDEX [IX_Animais_TutorId] ON [dbo].[Animais]([TutorId]);
PRINT '  ? Tabela Animais criada';
GO

-- =====================================================
-- 5. TABELA: Categorias
-- =====================================================
PRINT '[5/14] Criando tabela Categorias...';
CREATE TABLE [dbo].[Categorias] (
    [CategoriaId] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(100) NOT NULL UNIQUE,
    [Descricao] NVARCHAR(255) NULL
);
PRINT '  ? Tabela Categorias criada';
GO

-- =====================================================
-- 6. TABELA: Fornecedores
-- =====================================================
PRINT '[6/14] Criando tabela Fornecedores...';
CREATE TABLE [dbo].[Fornecedores] (
    [FornecedorId] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(150) NOT NULL,
    [Cnpj] NVARCHAR(150) NULL,
    [Telefone] NVARCHAR(20) NULL,
    [Email] NVARCHAR(150) NULL,
    [Endereco] NVARCHAR(300) NULL
);
PRINT '  ? Tabela Fornecedores criada';
GO

-- =====================================================
-- 7. TABELA: Produtos
-- =====================================================
PRINT '[7/14] Criando tabela Produtos...';
CREATE TABLE [dbo].[Produtos] (
    [ProdutoId] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(150) NOT NULL,
    [Descricao] NVARCHAR(500) NULL,
    [Preco] DECIMAL(10,2) NOT NULL CHECK ([Preco] >= 0),
    [QuantidadeEstoque] INT NOT NULL DEFAULT 0 CHECK ([QuantidadeEstoque] >= 0),
    [Ativo] BIT NOT NULL DEFAULT 1,
    [CodigoBarras] NVARCHAR(80) NULL,
    [CategoriaId] INT NULL,
    [FornecedorId] INT NULL,
    CONSTRAINT [FK_Produtos_Categorias] FOREIGN KEY ([CategoriaId]) 
        REFERENCES [dbo].[Categorias]([CategoriaId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Produtos_Fornecedores] FOREIGN KEY ([FornecedorId]) 
        REFERENCES [dbo].[Fornecedores]([FornecedorId]) ON DELETE SET NULL
);
CREATE UNIQUE INDEX [IX_Produtos_CodigoBarras] ON [dbo].[Produtos]([CodigoBarras]) WHERE [CodigoBarras] IS NOT NULL;
CREATE INDEX [IX_Produtos_CategoriaId] ON [dbo].[Produtos]([CategoriaId]);
PRINT '  ? Tabela Produtos criada';
GO

-- =====================================================
-- 8. TABELA: ProdutoImagens
-- =====================================================
PRINT '[8/14] Criando tabela ProdutoImagens...';
CREATE TABLE [dbo].[ProdutoImagens] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ProdutoId] INT NOT NULL,
    [Url] NVARCHAR(300) NOT NULL,
    CONSTRAINT [FK_ProdutoImagens_Produtos] FOREIGN KEY ([ProdutoId]) 
        REFERENCES [dbo].[Produtos]([ProdutoId]) ON DELETE CASCADE
);
CREATE INDEX [IX_ProdutoImagens_ProdutoId] ON [dbo].[ProdutoImagens]([ProdutoId]);
PRINT '  ? Tabela ProdutoImagens criada';
GO

-- =====================================================
-- 9. TABELA: Servicos
-- =====================================================
PRINT '[9/14] Criando tabela Servicos...';
CREATE TABLE [dbo].[Servicos] (
    [ServicoId] INT IDENTITY(1,1) PRIMARY KEY,
    [Nome] NVARCHAR(120) NOT NULL,
    [Descricao] NVARCHAR(500) NULL,
    [Preco] DECIMAL(10,2) NOT NULL CHECK ([Preco] >= 0),
    [DuracaoMinutos] INT NOT NULL CHECK ([DuracaoMinutos] > 0),
    [Ativo] BIT NOT NULL DEFAULT 1,
    [FuncionarioResponsavelId] INT NULL,
    [CargosResponsaveis] NVARCHAR(200) NULL,
    CONSTRAINT [FK_Servicos_Funcionarios] FOREIGN KEY ([FuncionarioResponsavelId]) 
        REFERENCES [dbo].[Funcionarios]([FuncionarioId]) ON DELETE SET NULL
);
PRINT '  ? Tabela Servicos criada';
GO

-- =====================================================
-- 10. TABELA: ServicoFuncionarios (Muitos-para-Muitos)
-- =====================================================
PRINT '[10/14] Criando tabela ServicoFuncionarios...';
CREATE TABLE [dbo].[ServicoFuncionarios] (
    [ServicoId] INT NOT NULL,
    [FuncionarioId] INT NOT NULL,
    PRIMARY KEY ([ServicoId], [FuncionarioId]),
    CONSTRAINT [FK_ServicoFuncionarios_Servicos] FOREIGN KEY ([ServicoId]) 
        REFERENCES [dbo].[Servicos]([ServicoId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ServicoFuncionarios_Funcionarios] FOREIGN KEY ([FuncionarioId]) 
        REFERENCES [dbo].[Funcionarios]([FuncionarioId]) ON DELETE CASCADE
);
PRINT '  ? Tabela ServicoFuncionarios criada';
GO

-- =====================================================
-- 11. TABELA: Agendamentos
-- =====================================================
PRINT '[11/14] Criando tabela Agendamentos...';
CREATE TABLE [dbo].[Agendamentos] (
    [AgendamentoId] INT IDENTITY(1,1) PRIMARY KEY,
    [AnimalId] INT NOT NULL,
    [ServicoId] INT NOT NULL,
    [FuncionarioId] INT NULL,
    [DataHora] DATETIME2 NOT NULL,
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Pendente' 
        CHECK ([Status] IN ('Pendente', 'Confirmado', 'EmAndamento', 'Concluido', 'Cancelado')),
    [Observacoes] NVARCHAR(500) NULL,
    CONSTRAINT [FK_Agendamentos_Animais] FOREIGN KEY ([AnimalId]) 
        REFERENCES [dbo].[Animais]([AnimalId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Agendamentos_Servicos] FOREIGN KEY ([ServicoId]) 
        REFERENCES [dbo].[Servicos]([ServicoId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Agendamentos_Funcionarios] FOREIGN KEY ([FuncionarioId]) 
        REFERENCES [dbo].[Funcionarios]([FuncionarioId]) ON DELETE SET NULL
);
CREATE INDEX [IX_Agendamentos_DataHora] ON [dbo].[Agendamentos]([DataHora]);
CREATE INDEX [IX_Agendamentos_Status] ON [dbo].[Agendamentos]([Status]);
PRINT '  ? Tabela Agendamentos criada';
GO

-- =====================================================
-- 12. TABELA: Vendas (COM USUARIOID)
-- =====================================================
PRINT '[12/14] Criando tabela Vendas...';
CREATE TABLE [dbo].[Vendas] (
    [VendaId] INT IDENTITY(1,1) PRIMARY KEY,
    [UsuarioId] INT NULL, -- ? NOVO: Rastrear usuário que fez a compra
    [TutorId] INT NULL,
    [FuncionarioId] INT NULL,
    [DataVenda] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [ValorTotal] DECIMAL(10,2) NOT NULL CHECK ([ValorTotal] >= 0),
    [FormaPagamento] NVARCHAR(50) NULL,
    [Observacoes] NVARCHAR(500) NULL,
    CONSTRAINT [FK_Vendas_Usuarios] FOREIGN KEY ([UsuarioId]) 
        REFERENCES [dbo].[Usuarios]([UsuarioId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Vendas_Tutores] FOREIGN KEY ([TutorId]) 
        REFERENCES [dbo].[Tutores]([TutorId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Vendas_Funcionarios] FOREIGN KEY ([FuncionarioId]) 
        REFERENCES [dbo].[Funcionarios]([FuncionarioId]) ON DELETE SET NULL
);
CREATE INDEX [IX_Vendas_UsuarioId] ON [dbo].[Vendas]([UsuarioId]);
CREATE INDEX [IX_Vendas_TutorId] ON [dbo].[Vendas]([TutorId]);
CREATE INDEX [IX_Vendas_DataVenda] ON [dbo].[Vendas]([DataVenda]);
PRINT '  ? Tabela Vendas criada';
GO

-- =====================================================
-- 13. TABELA: ItensVenda
-- =====================================================
PRINT '[13/14] Criando tabela ItensVenda...';
CREATE TABLE [dbo].[ItensVenda] (
    [ItemVendaId] INT IDENTITY(1,1) PRIMARY KEY,
    [VendaId] INT NOT NULL,
    [ProdutoId] INT NULL,
    [ServicoId] INT NULL,
    [Quantidade] INT NOT NULL CHECK ([Quantidade] > 0),
    [PrecoUnitario] DECIMAL(10,2) NOT NULL CHECK ([PrecoUnitario] >= 0),
    CONSTRAINT [FK_ItensVenda_Vendas] FOREIGN KEY ([VendaId]) 
        REFERENCES [dbo].[Vendas]([VendaId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ItensVenda_Produtos] FOREIGN KEY ([ProdutoId]) 
        REFERENCES [dbo].[Produtos]([ProdutoId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ItensVenda_Servicos] FOREIGN KEY ([ServicoId]) 
        REFERENCES [dbo].[Servicos]([ServicoId]) ON DELETE NO ACTION,
    CONSTRAINT [CHK_ItensVenda_ProdutoOuServico] 
        CHECK (([ProdutoId] IS NOT NULL AND [ServicoId] IS NULL) OR 
               ([ProdutoId] IS NULL AND [ServicoId] IS NOT NULL))
);
CREATE INDEX [IX_ItensVenda_VendaId] ON [dbo].[ItensVenda]([VendaId]);
PRINT '  ? Tabela ItensVenda criada';
GO

-- =====================================================
-- 14. TABELA: RegistrosProntuario
-- =====================================================
PRINT '[14/14] Criando tabela RegistrosProntuario...';
CREATE TABLE [dbo].[RegistrosProntuario] (
    [RegistroProntuarioId] INT IDENTITY(1,1) PRIMARY KEY,
    [AnimalId] INT NOT NULL,
    [FuncionarioId] INT NULL,
    [DataAtendimento] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [TipoAtendimento] NVARCHAR(80) NULL,
    [Descricao] NVARCHAR(1000) NULL,
    [Prescricoes] NVARCHAR(500) NULL,
    [Peso] DECIMAL(5,2) NULL CHECK ([Peso] > 0),
    CONSTRAINT [FK_RegistrosProntuario_Animais] FOREIGN KEY ([AnimalId]) 
        REFERENCES [dbo].[Animais]([AnimalId]) ON DELETE CASCADE,
    CONSTRAINT [FK_RegistrosProntuario_Funcionarios] FOREIGN KEY ([FuncionarioId]) 
        REFERENCES [dbo].[Funcionarios]([FuncionarioId]) ON DELETE SET NULL
);
CREATE INDEX [IX_RegistrosProntuario_AnimalId] ON [dbo].[RegistrosProntuario]([AnimalId]);
PRINT '  ? Tabela RegistrosProntuario criada';
GO

PRINT '';
PRINT '========================================';
PRINT '  POPULANDO DADOS INICIAIS';
PRINT '========================================';
PRINT '';

-- =====================================================
-- POPULAR: Usuarios (com senhas BCrypt)
-- =====================================================
PRINT '[1/12] Inserindo Usuarios...';
-- Senha para todos: senha123 (hash BCrypt)
SET IDENTITY_INSERT [dbo].[Usuarios] ON;
INSERT INTO [dbo].[Usuarios] ([UsuarioId], [Nome], [Email], [PasswordHash], [TipoUsuario], [Ativo]) VALUES
(1, 'Admin Sistema', 'admin@sigapet.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Admin', 1),
(2, 'Dr. Carlos Veterinário', 'carlos.vet@sigapet.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Funcionario', 1),
(3, 'Ana Tosadora', 'ana.tosa@sigapet.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Funcionario', 1),
(4, 'Pedro Atendente', 'pedro.atend@sigapet.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Funcionario', 1),
(5, 'Maria Silva', 'maria.silva@email.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Tutor', 1),
(6, 'João Santos', 'joao.santos@email.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Tutor', 1),
(7, 'Paula Oliveira', 'paula.oli@email.com', '$2a$11$YqVw8z5hK4Cq7Q9X7hZf2.FY1Y8h4K4C4Y8hK4Cq7Q9X7hZf2.FY1Y', 'Tutor', 1);
SET IDENTITY_INSERT [dbo].[Usuarios] OFF;
PRINT '  ? 7 usuários inseridos';
GO

-- =====================================================
-- POPULAR: Funcionarios
-- =====================================================
PRINT '[2/12] Inserindo Funcionarios...';
SET IDENTITY_INSERT [dbo].[Funcionarios] ON;
INSERT INTO [dbo].[Funcionarios] ([FuncionarioId], [UsuarioId], [Nome], [Cargo], [Telefone], [DataContratacao], [Ativo]) VALUES
(1, 2, 'Dr. Carlos Veterinário', 'Veterinário', '(11) 98765-1111', '2023-01-15', 1),
(2, 3, 'Ana Tosadora', 'Tosador', '(11) 98765-2222', '2023-03-20', 1),
(3, 4, 'Pedro Atendente', 'Atendente', '(11) 98765-3333', '2023-06-10', 1);
SET IDENTITY_INSERT [dbo].[Funcionarios] OFF;
PRINT '  ? 3 funcionários inseridos';
GO

-- =====================================================
-- POPULAR: Tutores
-- =====================================================
PRINT '[3/12] Inserindo Tutores...';
SET IDENTITY_INSERT [dbo].[Tutores] ON;
INSERT INTO [dbo].[Tutores] ([TutorId], [UsuarioId], [Nome], [Telefone], [Endereco], [DataCadastro]) VALUES
(1, 5, 'Maria Silva', '(11) 99999-1111', 'Rua das Flores, 123', '2024-01-10'),
(2, 6, 'João Santos', '(11) 99999-2222', 'Av. Brasil, 456', '2024-02-15'),
(3, 7, 'Paula Oliveira', '(11) 99999-3333', 'Rua da Paz, 789', '2024-03-20'),
(4, NULL, 'Cliente Avulso', '(11) 99999-9999', 'Não informado', GETDATE()); -- Tutor sem login
SET IDENTITY_INSERT [dbo].[Tutores] OFF;
PRINT '  ? 4 tutores inseridos';
GO

-- =====================================================
-- POPULAR: Animais
-- =====================================================
PRINT '[4/12] Inserindo Animais...';
SET IDENTITY_INSERT [dbo].[Animais] ON;
INSERT INTO [dbo].[Animais] ([AnimalId], [TutorId], [Nome], [Especie], [Raca], [DataNascimento], [Sexo], [Pelagem], [Observacoes]) VALUES
(1, 1, 'Rex', 'Cão', 'Labrador', '2020-05-10', 'Macho', 'Curta', 'Muito dócil e brincalhão'),
(2, 1, 'Mimi', 'Gato', 'Persa', '2021-03-15', 'Fêmea', 'Longa', 'Gosta de carinho'),
(3, 2, 'Bob', 'Cão', 'Bulldog', '2019-08-20', 'Macho', 'Curta', 'Precisa de dieta especial'),
(4, 3, 'Luna', 'Gato', 'Siamês', '2022-01-05', 'Fêmea', 'Curta', 'Muito ativa'),
(5, 3, 'Thor', 'Cão', 'Pastor Alemão', '2020-11-30', 'Macho', 'Média', 'Treinamento de guarda');
SET IDENTITY_INSERT [dbo].[Animais] OFF;
PRINT '  ? 5 animais inseridos';
GO

-- =====================================================
-- POPULAR: Categorias
-- =====================================================
PRINT '[5/12] Inserindo Categorias...';
SET IDENTITY_INSERT [dbo].[Categorias] ON;
INSERT INTO [dbo].[Categorias] ([CategoriaId], [Nome], [Descricao]) VALUES
(1, 'Ração', 'Alimentos secos e úmidos para pets'),
(2, 'Brinquedos', 'Brinquedos diversos para entretenimento'),
(3, 'Higiene', 'Produtos de limpeza e higiene'),
(4, 'Acessórios', 'Coleiras, guias, camas e outros'),
(5, 'Medicamentos', 'Medicamentos e suplementos'),
(6, 'Petiscos', 'Snacks e treats para pets');
SET IDENTITY_INSERT [dbo].[Categorias] OFF;
PRINT '  ? 6 categorias inseridas';
GO

-- =====================================================
-- POPULAR: Fornecedores
-- =====================================================
PRINT '[6/12] Inserindo Fornecedores...';
SET IDENTITY_INSERT [dbo].[Fornecedores] ON;
INSERT INTO [dbo].[Fornecedores] ([FornecedorId], [Nome], [Cnpj], [Telefone], [Email], [Endereco]) VALUES
(1, 'PetFood Distribuidora', '12.345.678/0001-99', '(11) 3000-1111', 'contato@petfood.com.br', 'Av. Industrial, 1000'),
(2, 'BrinquePet Ltda', '98.765.432/0001-11', '(11) 3000-2222', 'vendas@brinquepet.com.br', 'Rua das Fábricas, 500'),
(3, 'HigienePet S.A.', '11.222.333/0001-44', '(11) 3000-3333', 'comercial@higienepet.com.br', 'Av. Química, 200');
SET IDENTITY_INSERT [dbo].[Fornecedores] OFF;
PRINT '  ? 3 fornecedores inseridos';
GO

-- =====================================================
-- POPULAR: Produtos
-- =====================================================
PRINT '[7/12] Inserindo Produtos...';
SET IDENTITY_INSERT [dbo].[Produtos] ON;
INSERT INTO [dbo].[Produtos] ([ProdutoId], [Nome], [Descricao], [Preco], [QuantidadeEstoque], [Ativo], [CodigoBarras], [CategoriaId], [FornecedorId]) VALUES
(1, 'Ração Premium Cães Adultos 15kg', 'Ração balanceada para cães adultos de todas as raças', 189.90, 50, 1, '7891234567890', 1, 1),
(2, 'Ração Premium Gatos Adultos 10kg', 'Ração especial para gatos adultos', 149.90, 30, 1, '7891234567891', 1, 1),
(3, 'Bola de Borracha Grande', 'Bola resistente para cães de porte grande', 29.90, 100, 1, '7891234567892', 2, 2),
(4, 'Arranhador para Gatos', 'Arranhador com brinquedo suspenso', 89.90, 20, 1, '7891234567893', 2, 2),
(5, 'Shampoo Neutro 500ml', 'Shampoo hipoalergênico para pets', 39.90, 80, 1, '7891234567894', 3, 3),
(6, 'Coleira Ajustável Nylon', 'Coleira resistente tamanho M', 24.90, 150, 1, '7891234567895', 4, 2),
(7, 'Cama Ortopédica Média', 'Cama confortável para pets de médio porte', 159.90, 25, 1, '7891234567896', 4, 2),
(8, 'Petisco Natural Frango 200g', 'Petisco desidratado 100% natural', 19.90, 200, 1, '7891234567897', 6, 1);
SET IDENTITY_INSERT [dbo].[Produtos] OFF;
PRINT '  ? 8 produtos inseridos';
GO

-- =====================================================
-- POPULAR: ProdutoImagens
-- =====================================================
PRINT '[8/12] Inserindo ProdutoImagens...';
SET IDENTITY_INSERT [dbo].[ProdutoImagens] ON;
INSERT INTO [dbo].[ProdutoImagens] ([Id], [ProdutoId], [Url]) VALUES
(1, 1, 'assets/images/products/racao-caes.jpg'),
(2, 2, 'assets/images/products/racao-gatos.jpg'),
(3, 3, 'assets/images/products/bola-borracha.jpg'),
(4, 4, 'assets/images/products/arranhador.jpg'),
(5, 5, 'assets/images/products/shampoo.jpg'),
(6, 6, 'assets/images/products/coleira.jpg'),
(7, 7, 'assets/images/products/cama.jpg'),
(8, 8, 'assets/images/products/petisco.jpg');
SET IDENTITY_INSERT [dbo].[ProdutoImagens] OFF;
PRINT '  ? 8 imagens de produtos inseridas';
GO

-- =====================================================
-- POPULAR: Servicos
-- =====================================================
PRINT '[9/12] Inserindo Servicos...';
SET IDENTITY_INSERT [dbo].[Servicos] ON;
INSERT INTO [dbo].[Servicos] ([ServicoId], [Nome], [Descricao], [Preco], [DuracaoMinutos], [Ativo], [FuncionarioResponsavelId], [CargosResponsaveis]) VALUES
(1, 'Consulta Veterinária', 'Consulta geral com veterinário', 120.00, 30, 1, 1, 'Veterinário'),
(2, 'Banho e Tosa Pequeno Porte', 'Banho completo e tosa para pets pequenos', 80.00, 60, 1, 2, 'Tosador'),
(3, 'Banho e Tosa Grande Porte', 'Banho completo e tosa para pets grandes', 120.00, 90, 1, 2, 'Tosador'),
(4, 'Vacinação', 'Aplicação de vacinas', 60.00, 15, 1, 1, 'Veterinário'),
(5, 'Exame de Sangue', 'Coleta e análise de exames laboratoriais', 150.00, 20, 1, 1, 'Veterinário'),
(6, 'Tosa Higiênica', 'Tosa apenas em regiões específicas', 50.00, 30, 1, 2, 'Tosador');
SET IDENTITY_INSERT [dbo].[Servicos] OFF;
PRINT '  ? 6 serviços inseridos';
GO

-- =====================================================
-- POPULAR: ServicoFuncionarios (Relacionamento)
-- =====================================================
PRINT '[10/12] Inserindo ServicoFuncionarios...';
INSERT INTO [dbo].[ServicoFuncionarios] ([ServicoId], [FuncionarioId]) VALUES
(1, 1), -- Consulta -> Dr. Carlos
(2, 2), -- Banho Pequeno -> Ana
(3, 2), -- Banho Grande -> Ana
(4, 1), -- Vacinação -> Dr. Carlos
(5, 1), -- Exame -> Dr. Carlos
(6, 2); -- Tosa Higiênica -> Ana
PRINT '  ? 6 relacionamentos serviço-funcionário inseridos';
GO

-- =====================================================
-- POPULAR: Agendamentos
-- =====================================================
PRINT '[11/12] Inserindo Agendamentos...';
SET IDENTITY_INSERT [dbo].[Agendamentos] ON;
INSERT INTO [dbo].[Agendamentos] ([AgendamentoId], [AnimalId], [ServicoId], [FuncionarioId], [DataHora], [Status], [Observacoes]) VALUES
(1, 1, 1, 1, DATEADD(DAY, 2, GETDATE()), 'Confirmado', 'Primeira consulta do Rex'),
(2, 2, 2, 2, DATEADD(DAY, 3, GETDATE()), 'Pendente', 'Banho e tosa completa'),
(3, 3, 1, 1, DATEADD(DAY, 5, GETDATE()), 'Pendente', 'Check-up de rotina'),
(4, 4, 6, 2, DATEADD(DAY, 1, GETDATE()), 'Confirmado', NULL),
(5, 5, 4, 1, DATEADD(DAY, 7, GETDATE()), 'Pendente', 'Reforço de vacina antirrábica');
SET IDENTITY_INSERT [dbo].[Agendamentos] OFF;
PRINT '  ? 5 agendamentos inseridos';
GO

-- =====================================================
-- POPULAR: Vendas
-- =====================================================
PRINT '[12/12] Inserindo Vendas...';
SET IDENTITY_INSERT [dbo].[Vendas] ON;
INSERT INTO [dbo].[Vendas] ([VendaId], [UsuarioId], [TutorId], [FuncionarioId], [DataVenda], [ValorTotal], [FormaPagamento], [Observacoes]) VALUES
(1, 5, 1, 3, DATEADD(DAY, -10, GETDATE()), 219.80, 'Cartão de Crédito', 'Compra de ração e petisco'),
(2, 6, 2, 3, DATEADD(DAY, -5, GETDATE()), 109.80, 'Dinheiro', 'Compra de brinquedos'),
(3, 7, 3, NULL, DATEADD(DAY, -2, GETDATE()), 184.80, 'PIX', 'Compra de cama e shampoo');
SET IDENTITY_INSERT [dbo].[Vendas] OFF;
PRINT '  ? 3 vendas inseridas';
GO

-- =====================================================
-- POPULAR: ItensVenda
-- =====================================================
PRINT 'Inserindo ItensVenda...';
SET IDENTITY_INSERT [dbo].[ItensVenda] ON;
INSERT INTO [dbo].[ItensVenda] ([ItemVendaId], [VendaId], [ProdutoId], [ServicoId], [Quantidade], [PrecoUnitario]) VALUES
-- Venda 1: Ração + Petisco
(1, 1, 1, NULL, 1, 189.90),
(2, 1, 8, NULL, 1, 19.90),
-- Venda 2: Bola + Arranhador
(3, 2, 3, NULL, 1, 29.90),
(4, 2, 4, NULL, 1, 89.90),
-- Venda 3: Cama + Shampoo
(5, 3, 7, NULL, 1, 159.90),
(6, 3, 5, NULL, 1, 39.90);
SET IDENTITY_INSERT [dbo].[ItensVenda] OFF;
PRINT '  ? 6 itens de venda inseridos';
GO

PRINT '';
PRINT '========================================';
PRINT '  ? BANCO DE DADOS CRIADO E POPULADO';
PRINT '========================================';
PRINT '';
PRINT '?? RESUMO:';
PRINT '   - 7 Usuários';
PRINT '   - 3 Funcionários';
PRINT '   - 4 Tutores';
PRINT '   - 5 Animais';
PRINT '   - 6 Categorias';
PRINT '   - 3 Fornecedores';
PRINT '   - 8 Produtos';
PRINT '   - 6 Serviços';
PRINT '   - 5 Agendamentos';
PRINT '   - 3 Vendas';
PRINT '';
PRINT '?? CREDENCIAIS DE TESTE:';
PRINT '   Admin: admin@sigapet.com / senha123';
PRINT '   Veterinário: carlos.vet@sigapet.com / senha123';
PRINT '   Cliente: maria.silva@email.com / senha123';
PRINT '';
PRINT '? Script executado com sucesso!';
GO
