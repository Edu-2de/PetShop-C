-- Script para atualizar usuários existentes com nomes
-- Execute este script no banco SIGA-PET-DB

-- Limpar dados existentes primeiro
DELETE FROM [ItensVenda];
DELETE FROM [Vendas];
DELETE FROM [Agendamentos];
DELETE FROM [ServicoFuncionarios];
DELETE FROM [RegistroProntuario];
DELETE FROM [Servicos];
DELETE FROM [Animais];
DELETE FROM [ProdutoImagens];
DELETE FROM [Produtos];
DELETE FROM [Categorias];
DELETE FROM [Fornecedores];
DELETE FROM [Funcionarios];
DELETE FROM [Tutores];
DELETE FROM [Usuarios];

-- Resetar identidades
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

PRINT 'Banco limpo com sucesso!'
PRINT 'Agora use o Swagger: POST /api/Seed/popular-completo'