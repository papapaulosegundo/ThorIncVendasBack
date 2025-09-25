using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class CategoriaRepository {
    private readonly string _cs;
    public CategoriaRepository(IConfiguration cfg) => _cs = cfg.GetConnectionString("DefaultConnection");
    private IDbConnection Conn => new NpgsqlConnection(_cs);

    public async Task<int> InserirAsync(Categoria c) {
        const string sql = @"
            INSERT INTO categoria (nome, slug, descricao, ativa)
            VALUES (@Nome, @Slug, @Descricao, @Ativa)
            RETURNING id";
        using var cn = Conn;
        return await cn.ExecuteScalarAsync<int>(sql, c);
    }

    public async Task AtualizarAsync(int id, Categoria c) {
        const string sql = @"
            UPDATE categoria
            SET nome=@Nome, slug=@Slug, descricao=@Descricao, ativa=@Ativa, atualizado_em=NOW()
            WHERE id=@Id";
        using var cn = Conn;
        await cn.ExecuteAsync(sql, new { Id = id, c.Nome, c.Slug, c.Descricao, c.Ativa });
    }

    public async Task<Categoria?> ObterPorIdAsync(int id) {
        const string sql = @"SELECT id, nome, slug, descricao, ativa, criado_em AS CriadoEm, atualizado_em AS AtualizadoEm
                             FROM categoria WHERE id=@Id";
        using var cn = Conn;
        return await cn.QueryFirstOrDefaultAsync<Categoria>(sql, new { Id = id });
    }

    public async Task<Categoria?> ObterPorSlugAsync(string slug) {
        const string sql = @"SELECT id, nome, slug, descricao, ativa, criado_em AS CriadoEm, atualizado_em AS AtualizadoEm
                             FROM categoria WHERE slug=@Slug";
        using var cn = Conn;
        return await cn.QueryFirstOrDefaultAsync<Categoria>(sql, new { Slug = slug });
    }

    public async Task<IEnumerable<Categoria>> ObterTodosAsync(int limit, int offset, string? nome=null) {
        var sql = @"SELECT id, nome, slug, descricao, ativa, criado_em AS CriadoEm, atualizado_em AS AtualizadoEm
                    FROM categoria";
        if (!string.IsNullOrWhiteSpace(nome)) sql += " WHERE nome ILIKE @Nome";
        sql += " ORDER BY nome LIMIT @Limit OFFSET @Offset";
        using var cn = Conn;
        return await cn.QueryAsync<Categoria>(sql, new { Limit = limit, Offset = offset, Nome = $"%{nome}%" });
    }

    public async Task DeletarAsync(int id) {
        const string sql = "DELETE FROM categoria WHERE id=@Id";
        using var cn = Conn;
        await cn.ExecuteAsync(sql, new { Id = id });
    }
}
