using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ITransaccionesService
    {
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> GetByUserId(int usuarioId);
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
            },commandType:CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Transaccion>> GetByUserId(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>(@"SELECT * FROM TRANSACCIONES WHERE UsuarioId=@usuarioId", new { usuarioId});
        }
    }
}
