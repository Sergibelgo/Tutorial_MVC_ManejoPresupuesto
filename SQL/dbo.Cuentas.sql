CREATE TABLE [dbo].[Cuentas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TipoCuentaId] INT NOT NULL, 
    [Balance] DECIMAL(18, 2) NOT NULL, 
    [Descripcion] NCHAR(1000) NULL, 
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_Cuentas_TiposCuentas] FOREIGN KEY ([TipoCuentaId]) REFERENCES [TiposCuentas]([Id]), 
    CONSTRAINT [FK_Cuentas_Usuarios] FOREIGN KEY ([UserId]) REFERENCES [Usuarios]([Id])
)
