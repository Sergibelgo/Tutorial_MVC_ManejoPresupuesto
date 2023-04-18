using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ICuentasService
    {
        Task Crear(Cuenta cuenta);
        Task<IEnumerable<Cuenta>> GetByUserId(int userId);
    }
    public class CuentasService : ICuentasService
    {
        private readonly string connectionString;
        public CuentasService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            int id = await connection.QuerySingleAsync<int>(@"INSERT INTO Cuentas(TipoCuentaId,Descripcion,Balance,UserId) 
                                                            VALUES(@TipoCuentaId,@Descripcion,@Balance,@UsuarioId);
                                                        SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id;
        }
        public async Task<IEnumerable<Cuenta>> GetByUserId(int userId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Cuenta>(@"SELECT C.Id,C.Balance,C.UserId, T.Nombre as TipoCuenta,T.Id as TipoCuentaId
                                                            FROM Cuentas C,TiposCuentas T
                                                            WHERE C.UserId=@userId and C.TipoCuentaId=T.Id",new { userId });
        }
    }
}
