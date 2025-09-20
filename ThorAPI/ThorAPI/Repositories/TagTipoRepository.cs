using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class TagTipoRepository {
    private readonly string _connectionString;

    public TagTipoRepository(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    public async Task<int> InserirAsync(TagTipo tagTipo) {
        using var conn = Connection;
        string sql = @"
            INSERT INTO tag_tipo (nome)
            VALUES (@Nome)
            RETURNING id;";
        return await conn.ExecuteScalarAsync<int>(sql, new { tagTipo.Nome });
    }

    public async Task<TagTipo?> ObterPorIdAsync(int id) {
        using var conn = Connection;
        string sql = @"
            SELECT 
                id,
                nome 
            FROM 
                tag_tipo 
            WHERE 
                id = @Id
        ";
        return await conn.QueryFirstOrDefaultAsync<TagTipo>(sql, new { Id = id });
    }

    public async Task<IEnumerable<TagTipo>> ObterTodosAsync(int limit, int offset) {
        using var conn = Connection;
        string sql = @"
            SELECT 
                id,
                nome
            FROM 
                tag_tipo 
            LIMIT 
                @Limit 
            OFFSET 
                @Offset
        ";
        return await conn.QueryAsync<TagTipo>(sql, new { Limit = limit, Offset = offset });
    }

    internal async Task Atualiza(int id, TagTipo tagTipo) {
        using var conn = Connection;
        string sql = @"
            UPDATE 
                tag_tipo
            SET 
                nome = @Nome 
            WHERE 
                id = @Id
        ";
        await conn.ExecuteAsync(sql, new { Id = id, Nome = tagTipo.Nome });
    }

    internal async Task DeletarPorIdAsync(int id) {
        using var conn = Connection;
        string sql = @"
            DELETE 
            FROM 
                tag_tipo 
            WHERE 
                id = @Id
        ";
        await conn.ExecuteAsync(sql, new { Id = id });
    }

    internal async Task<TagTipo?> ObterPorNomeAsync(string nome) {
        using var conn = Connection;
        string sql = @"
            SELECT 
                id, 
                nome 
            FROM 
                tag_tipo 
            WHERE 
                nome = @Nome
        ";
        return await conn.QueryFirstOrDefaultAsync<TagTipo>(sql, new { Nome = nome });
    }
}
