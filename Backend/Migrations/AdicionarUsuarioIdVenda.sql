-- Script de Migração: Adicionar UsuarioId à tabela Vendas
-- Data: 08/12/2024
-- Descrição: Permite rastrear qual usuário fez a compra, mesmo que não seja tutor

USE [SIGA-PET];
GO

-- Verificar se a coluna já existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Vendas]') AND name = 'UsuarioId')
BEGIN
    PRINT 'Adicionando coluna UsuarioId à tabela Vendas...';
    
    -- Adicionar coluna UsuarioId
    ALTER TABLE [dbo].[Vendas]
    ADD [UsuarioId] INT NULL;
    
    PRINT 'Coluna UsuarioId adicionada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Coluna UsuarioId já existe na tabela Vendas.';
END
GO

-- Adicionar Foreign Key se ainda não existir
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Vendas_Usuarios_UsuarioId')
BEGIN
    PRINT 'Adicionando Foreign Key FK_Vendas_Usuarios_UsuarioId...';
    
    ALTER TABLE [dbo].[Vendas]
    ADD CONSTRAINT [FK_Vendas_Usuarios_UsuarioId] 
    FOREIGN KEY ([UsuarioId]) 
    REFERENCES [dbo].[Usuarios]([UsuarioId])
    ON DELETE NO ACTION;
    
    PRINT 'Foreign Key adicionada com sucesso.';
END
ELSE
BEGIN
    PRINT 'Foreign Key FK_Vendas_Usuarios_UsuarioId já existe.';
END
GO

-- Criar índice para melhor performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vendas_UsuarioId' AND object_id = OBJECT_ID('Vendas'))
BEGIN
    PRINT 'Criando índice IX_Vendas_UsuarioId...';
    
    CREATE INDEX [IX_Vendas_UsuarioId] 
    ON [dbo].[Vendas]([UsuarioId]);
    
    PRINT 'Índice criado com sucesso.';
END
ELSE
BEGIN
    PRINT 'Índice IX_Vendas_UsuarioId já existe.';
END
GO

-- Atualizar vendas existentes: vincular ao usuário do tutor quando possível
PRINT 'Atualizando vendas existentes...';

UPDATE v
SET v.UsuarioId = t.UsuarioId
FROM [dbo].[Vendas] v
INNER JOIN [dbo].[Tutores] t ON v.TutorId = t.TutorId
WHERE v.UsuarioId IS NULL AND t.UsuarioId IS NOT NULL;

PRINT 'Migração concluída com sucesso!';
GO
