using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public UsuarioRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    public async Task<int> InserirAsync(Usuario usuario)
    {
        using var conn = Connection;
        string sql = @"
            INSERT INTO Usuarios (Nome, Email, Senha)
            VALUES (@Nome, @Email, @Senha)
            RETURNING Id;";
        return await conn.ExecuteScalarAsync<int>(sql, usuario);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        using var conn = Connection;
        string sql = "SELECT * FROM Usuarios WHERE Email = @Email";
        return await conn.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
    }
}