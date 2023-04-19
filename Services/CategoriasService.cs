using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ICategoriasService
    {
        Task Create(Categoria categoria);
    }
    public class CategoriasService : ICategoriasService
    {
        private readonly string connectionString;

        public CategoriasService(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre,TipoOperacionId,UsuarioId) 
                                                                VALUES(@Nombre,@TipoOperacionId,@UsuarioId);
                                                                SELECT SCOPE_IDENTITY();", categoria);
            categoria.Id = id;
        }
    }
}
