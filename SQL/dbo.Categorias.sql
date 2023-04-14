CREATE TABLE [dbo].[Categorias]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nombre] NCHAR(50) NOT NULL, 
    [TipoOperacionId] INT NOT NULL, 
    [UsuarioId] INT NOT NULL, 
    CONSTRAINT [FK_Categorias_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios]([Id]), 
    CONSTRAINT [FK_Categorias_TiposOperaciones] FOREIGN KEY ([TipoOperacionId]) REFERENCES [TiposOperaciones]([Id])
)
