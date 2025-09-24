using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class UsuarioRepository {
    private readonly string _connectionString;

    public UsuarioRepository(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private NpgsqlConnection Connection => new(_connectionString);

    public async Task<int> CriarAsync(Usuario usuario) {
        const string sql = @"
            INSERT INTO usuario (
                cpf, 
                nome, 
                email, 
                senha,
                tipo
            )
            VALUES (
                @Cpf, 
                @Nome, 
                @Email, 
                @Senha,
                'usuario'
            )
            RETURNING id";
        using var conn = Connection;
        return await conn.ExecuteScalarAsync<int>(sql, usuario);
    }

    public async Task<Usuario?> ObterPorIdAsync(int id) {
        const string sql = @"
            SELECT 
                id,
                cpf,
                nome,
                email,
                senha,
                tipo
            FROM 
                usuario 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email) {
        const string sql = @"
            SELECT 
                id AS ""Id"",
                cpf AS ""Cpf"",
                nome AS ""Nome"", 
                email AS ""Email"",
                senha AS ""Senha"",
                tipo AS ""Tipo""
            FROM 
                usuario 
            WHERE 
                email = @Email 
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
    }

    public async Task<Usuario?> ObterPorEmailSenhaAsync(string email, string senha) {
        const string sql = @"
            SELECT 
                id,
                cpf,
                nome, 
                email,
                senha,
                tipo
            FROM 
                usuario 
            WHERE 
                email = @Email 
                AND senha = @Senha
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email, Senha = senha });
    }

    public async Task AtualizarAsync(Usuario usuario) {
        const string sql = @"
            UPDATE 
                usuario
            SET 
                cpf = @Cpf, 
                nome = @Nome, 
                email = @Email, 
                senha = @Senha
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, usuario);
    }

    public async Task DeletarAsync(int id) {
        const string sql = @"
            DELETE 
            FROM 
                usuario 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { Id = id });
    }
}
