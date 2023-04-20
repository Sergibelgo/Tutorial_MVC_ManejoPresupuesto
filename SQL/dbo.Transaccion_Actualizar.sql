CREATE PROCEDURE [dbo].[Transaccion_Actualizar]
	@Id int,
	@FechaTransaccion datetime,
	@Monto decimal(18,2),
	@MontoAnterior decimal(18,2),
	@CuentaId int,
	@CuentaAnteriorId int,
	@CategoriaId int,
	@Nota nvarchar(1000)=null
AS
begin
	--Revertir transaccion anterior
	UPDATE Cuentas
	SET Balance-=@MontoAnterior
	WHERE Id=@CuentaAnteriorId;
	--Realizar nueva transaccion
	UPDATE Cuentas
	SET Balance+=@Monto
	WHERE Id=@CuentaId;

	UPDATE Transacciones
	SET Monto= ABS(@Monto),FechaTransaccion=@FechaTransaccion,CategoriaId=@CategoriaId,CuentaId=@CuentaId,Nota=@Nota
	WHERE Id=@Id;
	END
	GO
