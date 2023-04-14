CREATE TABLE [dbo].[TiposCuentas]
(
	[Id] INT NOT NULL IDENTITY, 
    [Nombre] NCHAR(50) NOT NULL, 
    [Orden] INT NOT NULL, 
    CONSTRAINT [PK_Cuentas] PRIMARY KEY ([Id]) 
)
