-- Script utilizado na criação do banco de dados do projeto.
-- Não é necessário executá-lo: serve somente de referência.

CREATE DATABASE [Embalagem]
GO

USE [Embalagem]
GO

CREATE TABLE [Caixa] (
    caixa_id VARCHAR(10) NOT NULL PRIMARY KEY,
    altura INT NOT NULL,
    largura INT NOT NULL,
    comprimento INT NOT NULL
)

CREATE TABLE [PedidoEmbalado] (
    pedido_id INT NOT NULL,
    caixa_id VARCHAR(10) NULL,
    produto_id VARCHAR(50) NOT NULL,
    CONSTRAINT FKCaixa FOREIGN KEY(caixa_id) REFERENCES Caixa(caixa_id),
)
GO


INSERT INTO [Caixa] VALUES 
    ('caixa 1', 30, 40, 80),
    ('caixa 2', 80, 50, 40),
    ('caixa 3', 50, 80, 60);
GO