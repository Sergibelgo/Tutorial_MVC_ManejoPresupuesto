CREATE PROCEDURE [dbo].[Transacciones_SelectConTipoOperacion]

AS
Begin
	select T.*, O.Descripcion as Tipo
		from Transacciones T,TiposOperaciones O
		where T.TiposOperacionId=O.Id
end