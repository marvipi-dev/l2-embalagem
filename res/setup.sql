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


CREATE TABLE [Usuario] (
    id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    email VARCHAR(50) UNIQUE NOT NULL,
    salt UNIQUEIDENTIFIER DEFAULT NEWID(),
    senha_hash VARCHAR(256) NOT NULL
);
GO

CREATE FUNCTION [MascararSenha] (@senha VARCHAR(100), @salt UNIQUEIDENTIFIER)
RETURNS VARCHAR(128)
AS BEGIN
    DECLARE @senha_hash VARCHAR(128),
            @senha_e_salt VARCHAR(128);
    
    SET @senha_e_salt = CONCAT(@senha, CAST(@salt AS VARCHAR(36)));
    SET @senha_hash = CONVERT(VARCHAR(128), HASHBYTES('SHA2_512', @senha_e_salt), 2);

    RETURN @senha_hash
END;
GO

CREATE PROCEDURE [RegistrarUsuario] (@email VARCHAR(50), @senha VARCHAR(128))
AS BEGIN
    DECLARE @salt UNIQUEIDENTIFIER = NEWID();
    INSERT INTO [Usuario] (email, salt, senha_hash)
                   VALUES (@email, @salt, dbo.MascararSenha(@senha, @salt));
END;
GO

CREATE FUNCTION [ValidarUsuario] (@email VARCHAR(50), @senha VARCHAR(128))
RETURNS BIT
AS BEGIN
    DECLARE @salt UNIQUEIDENTIFIER,
            @senha_hash VARCHAR(256),
            @usuario_valido BIT;
    
    SELECT @salt = salt, 
           @senha_hash = senha_hash
    FROM Usuario
    WHERE email = @email;

    SET @usuario_valido = CASE WHEN @senha_hash = dbo.MascararSenha(@senha, @salt) THEN 1 ELSE 0 END
    
    RETURN @usuario_valido;
END;
GO