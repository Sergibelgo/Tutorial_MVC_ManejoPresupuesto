using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public class TiposCuentasServices : ITiposCuentasServices
    {
        private readonly string connectionString;
        public TiposCuentasServices(IConfiguration configuration)
        {

            this.connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO TiposCuentas(Nombre,Orden) VALUES(@Nombre,0);
                                                    SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }
    }
}
