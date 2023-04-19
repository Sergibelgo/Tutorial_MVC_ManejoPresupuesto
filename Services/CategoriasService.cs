﻿using Dapper;
using Microsoft.Data.SqlClient;
using Tutorial2ManejoPresupuesto.Models;

namespace Tutorial2ManejoPresupuesto.Services
{
    public interface ICategoriasService
    {
        Task Create(Categoria categoria);
        Task<IEnumerable<Categoria>> GetByUserId(int usuarioId);
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
        public async Task<IEnumerable<Categoria>> GetByUserId(int usuarioId)
        {
            using var connection = new SqlConnection(this.connectionString);
            return await connection.QueryAsync<Categoria>(@"SELECT * FROM Categorias 
                                                            WHERE UsuarioId=@usuarioId", new { usuarioId });
        }
    }
}
