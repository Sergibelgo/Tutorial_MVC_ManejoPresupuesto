using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public class TiposCuentasService : ITiposCuentasService
    {
        private readonly string connectionString;
        public TiposCuentasService(IConfiguration configuration)
        {

            this.connectionString = configuration.GetConnectionString("DefaultConnection");

        }
        public async Task Crear(TipoCuenta tipoCuenta)
        {

            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO TiposCuentas(Nombre,Orden) VALUES(@Nombre,(SELECT MAX(Orden)+1 FROM TiposCuentas));
                                                    SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }
        public async Task<bool> IsPresent(string nombre)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas WHERE Nombre=@Nombre", new { nombre });
            return existe == 1;
        }
        public async Task<IEnumerable<TipoCuenta>> GetAll()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>("SELECT * FROM TiposCuentas ORDER BY Orden");
        }
        public async Task Update(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre=@Nombre WHERE Id=@Id", tipoCuenta);
        }
        public async Task<TipoCuenta> GetTipoCuentaById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas WHERE Id=@Id", new { id });
        }
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE TiposCuentas WHERE Id=@Id", new { Id });
        public async Task Ordenar(IEnumerable<TipoCuenta> lista)
        {
            var query = "UPDATE TiposCuentas SET Orden=@Orden WHERE Id=@Id";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, lista);
        }
    }
}
