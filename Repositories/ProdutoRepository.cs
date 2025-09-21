using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class ProdutoRepository {
    private readonly string _connectionString;

    public ProdutoRepository(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    internal async Task<int> InserirAsync(Produto dto) {
        var sql = @"
            INSERT INTO produto (
                nome, 
                descricao, 
                imagem, 
                preco, 
                quantidade, 
                id_tag_tipo
            ) 
            VALUES (
                @Nome, 
                @Descricao, 
                @Imagem, 
                @Preco, 
                @Quantidade, 
                @IdTagTipo
            ) 
            RETURNING id";
        using var conn = Connection;
        return await conn.ExecuteScalarAsync<int>(sql, dto);
    }

    internal async Task AtualizarAsync(int id, Produto dto) {
        var sql = @"
            UPDATE 
                produto
            SET 
                nome = @Nome,
                descricao = @Descricao,
                imagem = @Imagem,
                preco = @Preco,
                quantidade = @Quantidade,
                atualizado_em = NOW(),
                id_tag_tipo = @IdTagTipo
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { Id = id, dto.Nome, dto.Descricao, dto.Imagem, dto.Preco, dto.Quantidade, dto.IdTagTipo });
    }

    internal async Task DeletarPorIdAsync(int id) {
        var sql = @"
            DELETE 
            FROM 
                produto 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { Id = id });
    }

    internal async Task<Produto?> ObterPorIdAsync(int id) {
        var sql = @"
            SELECT 
                id, 
                nome, 
                descricao, 
                imagem, 
                preco, 
                quantidade, 
                criado_em AS CriadoEm, 
                atualizado_em AS AtualizadoEm, 
                id_tag_tipo AS IdTagTipo
            FROM 
                produto 
            WHERE 
                id = @Id
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
    }

    internal async Task<IEnumerable<Produto>> ObterTodosAsync(
        int limit, 
        int offset, 
        string? nome = null
    ) {
        var sql = @"
            SELECT 
                id, 
                nome, 
                descricao, 
                imagem, 
                preco, 
                quantidade, 
                criado_em AS CriadoEm, 
                atualizado_em AS AtualizadoEm, 
                id_tag_tipo AS IdTagTipo
            FROM produto
        ";

        if (!string.IsNullOrEmpty(nome)) {
            sql += @" 
                WHERE 
                    nome ILIKE @Nome
            ";
        }

        sql += @"
            LIMIT 
                @Limit 
            OFFSET 
                @Offset
        ";

        using var conn = Connection;
        return await conn.QueryAsync<Produto>(sql, new { Limit = limit, Offset = offset, Nome = $"%{nome}%" });
    }

}
