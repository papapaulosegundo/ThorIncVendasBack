using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IDbTransaction _transaction;
    private IDbConnection Connection => _transaction.Connection;

    public ProdutoRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<int> InserirAsync(Produto dto)
    {
        var sql = @"
            INSERT INTO produto (nome, descricao, imagem, preco, id_tag_tipo, id_categoria) 
            VALUES (@Nome, @Descricao, @Imagem, @Preco, @IdTagTipo, @IdCategoria) 
            RETURNING id";
        return await Connection.ExecuteScalarAsync<int>(sql, dto, transaction: _transaction);
    }

    public async Task AtualizarAsync(int id, Produto dto)
    {
        var sql = @"
            UPDATE produto
            SET nome = @Nome, descricao = @Descricao, imagem = @Imagem, preco = @Preco, 
                atualizado_em = NOW(), id_tag_tipo = @IdTagTipo, id_categoria = @IdCategoria
            WHERE id = @Id";
        await Connection.ExecuteAsync(sql,
            new { Id = id, dto.Nome, dto.Descricao, dto.Imagem, dto.Preco, dto.IdTagTipo, dto.IdCategoria }, transaction: _transaction);
    }

    public async Task DeletarPorIdAsync(int id)
    {
        var sql = "DELETE FROM produto WHERE id = @Id";
        await Connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        var sql = @"
            SELECT id, nome, descricao, imagem, preco, criado_em AS CriadoEm, 
                   atualizado_em AS AtualizadoEm, id_tag_tipo AS IdTagTipo, id_categoria AS IdCategoria
            FROM produto 
            WHERE id = @Id";
        return await Connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync(int limit, int offset, string? nome = null, int? idCategoria = null)
    {
        var where = new List<string>();
        if (!string.IsNullOrWhiteSpace(nome)) where.Add("nome ILIKE @Nome");
        if (idCategoria.HasValue) where.Add("id_categoria = @IdCategoria");

        var sql = @"
          SELECT id, nome, descricao, imagem, preco,
                 criado_em AS CriadoEm, atualizado_em AS AtualizadoEm,
                 id_tag_tipo AS IdTagTipo, id_categoria AS IdCategoria
          FROM produto";

        if (where.Count > 0)
            sql += " WHERE " + string.Join(" AND ", where);

        sql += " ORDER BY id DESC LIMIT @Limit OFFSET @Offset";

        return await Connection.QueryAsync<Produto>(sql,
            new { Limit = limit, Offset = offset, Nome = $"%{nome}%", IdCategoria = idCategoria },
            transaction: _transaction);
    }
}