using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface IUsuariosAuthService
    {
        Task<int> CrearUsuario(Usuario usuario);
        Task<Usuario> GetByEmail(string email);
    }
    public class UsuariosAuthService : IUsuariosAuthService
    {
        private readonly string connectionString;
        public UsuariosAuthService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CrearUsuario(Usuario usuario)
        {

            using var connection = new SqlConnection(connectionString);
            var UsuarioId = await connection.QuerySingleAsync<int>(@"INSERT INTO Usuarios (Email,EmailNormalizado,PasswordHash)
                                                                VALUES(@Email,@EmailNormalizado,@PasswordHash); SELECT SCOPE_IDENTITY();", usuario);
            await connection.ExecuteAsync("CrearDatosUsuarioNuevo", new { UsuarioId },commandType:System.Data.CommandType.StoredProcedure);
            return UsuarioId;
        }
        public async Task<Usuario> GetByEmail(string email)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(@"SELECT * FROM Usuarios WHERE EmailNormalizado=@email", new { email });
        }
    }
}
