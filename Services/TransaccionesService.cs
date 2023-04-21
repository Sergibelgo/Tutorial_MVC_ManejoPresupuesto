using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ITransaccionesService
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Crear(Transaccion transaccion);
        Task<Transaccion> GetById(int id, int usuarioId);
        Task<IEnumerable<Transaccion>> GetByUserId(int usuarioId);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
    }
    public class TransaccionesService : ITransaccionesService
    {
        private readonly string connectionString;
        public TransaccionesService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleOrDefaultAsync<int>(@"Transacciones_Insertar", new
            {
                transaccion.UsuarioId,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota
            }, commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Transaccion>> GetByUserId(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>(@"SELECT * FROM TRANSACCIONES WHERE UsuarioId=@usuarioId", new { usuarioId });
        }
        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TransaccionDTO>(@"SELECT T.*,C.Nombre as Categoria 
                                                                    FROM Transacciones T,Categorias C 
                                                                    WHERE T.CuentaId=@CuentaId and T.UsuarioId=@UsuarioId and FechaTransaccion BETWEEN @FechaInicio AND @FECHAFIN"
                                                            ,modelo);
        }
        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnteriorId)
        {
            using var con = new SqlConnection(connectionString);
            await con.ExecuteAsync("Transaccion_Actualizar", new
            {
                transaccion.Id,
                transaccion.FechaTransaccion,
                transaccion.Monto,
                transaccion.CategoriaId,
                transaccion.CuentaId,
                transaccion.Nota,
                montoAnterior,
                cuentaAnteriorId
            }, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<Transaccion> GetById(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(@"SELECT T.*,C.TipoOperacionId 
                                                                            FROM TRANSACCIONES T,Categorias C 
                                                                            WHERE T.Id=@id AND T.UsuarioId=@usuarioId AND T.CategoriaId=C.id", 
                                                                            new { id, usuarioId });
        }
    }
}
