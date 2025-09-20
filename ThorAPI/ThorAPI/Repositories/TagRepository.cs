using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class TagRepository {
    private readonly string _connectionString;

    public TagRepository(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    internal async Task AtualizarAsync(int id, Tag dto) {
        var sql = @"
            UPDATE 
                tag 
            SET 
                nome = @Nome, 
                id_tag_tipo = @IdTagTipo 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { Id = id, dto.Nome, dto.IdTagTipo });
    }

    internal async Task DeletarPorIdAsync(int id) {
        var sql = @"
            DELETE 
            FROM 
                tag 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { Id = id });
    }

    internal async Task<int> InserirAsync(Tag dto) {
        var sql = @"
            INSERT INTO tag (
                id_tag_tipo, 
                nome
            ) 
            VALUES (
                @IdTagTipo, 
                @Nome
            ) 
            RETURNING id
        ";
        using var conn = Connection;
        return await conn.ExecuteScalarAsync<int>(sql, dto);
    }

    internal async Task<Tag?> ObterPorIdAsync(int id) {
        var sql = @"
            SELECT 
                id, 
                id_tag_tipo AS IdTagTipo, 
                nome 
            FROM 
                tag 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Tag>(sql, new { Id = id });
    }

    internal async Task<Tag?> ObterPorNomeEIdTagTipoAsync(string nome, int idTagTipo) {
        var sql = @"
            SELECT 
                id, 
                id_tag_tipo AS IdTagTipo, 
                nome 
            FROM 
                tag 
            WHERE 
                nome = @Nome 
                AND id_tag_tipo = @IdTagTipo
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Tag>(sql, new { Nome = nome, IdTagTipo = idTagTipo });
    }

    internal async Task<IEnumerable<Tag>> ObterTodosAsync(
        int limit, 
        int offset, 
        int idTagTipo
    ) {
        var sql = @"
            SELECT 
                id, 
                id_tag_tipo AS IdTagTipo, 
                nome 
            FROM 
                tag 
            WHERE 
                id_tag_tipo = @IdTagTipo 
            LIMIT 
                @Limit 
            OFFSET 
                @Offset
        ";
        using var conn = Connection;
        return await conn.QueryAsync<Tag>(sql, new { Limit = limit, Offset = offset, IdTagTipo = idTagTipo });
    }
}

