using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ICuentasService
    {
        Task Crear(Cuenta cuenta);
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
    }
}
