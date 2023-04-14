CREATE TABLE [dbo].[Transacciones] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [UsuarioId]        INT             NOT NULL,
    [FechaTransaccion] DATETIME        NOT NULL,
    [Monto]            DECIMAL (18, 2) NOT NULL,
    [TiposOperacionId] INT             NOT NULL,
    [Nota]             NCHAR (1000)    NULL,
    [CuentaId] INT NOT NULL, 
    [CategoriaId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transacciones_TiposOperaciones] FOREIGN KEY ([TiposOperacionId]) REFERENCES [dbo].[TiposOperaciones] ([Id]), 
    CONSTRAINT [FK_Transacciones_Cuenta] FOREIGN KEY ([CuentaId]) REFERENCES [Cuentas]([Id]), 
    CONSTRAINT [FK_Transacciones_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios]([Id]), 
    CONSTRAINT [FK_Transacciones_Categorias] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias]([Id])
);

