using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class TagRepository : ITagRepository
{
    private readonly IDbTransaction _transaction;
    private IDbConnection Connection => _transaction.Connection;

    public TagRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task AtualizarAsync(int id, Tag dto)
    {
        var sql = "UPDATE tag SET nome = @Nome, id_tag_tipo = @IdTagTipo WHERE id = @Id";
        await Connection.ExecuteAsync(sql, new { Id = id, dto.Nome, dto.IdTagTipo }, transaction: _transaction);
    }

    public async Task DeletarPorIdAsync(int id)
    {
        var sql = "DELETE FROM tag WHERE id = @Id";
        await Connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<int> InserirAsync(Tag dto)
    {
        var sql = "INSERT INTO tag (id_tag_tipo, nome) VALUES (@IdTagTipo, @Nome) RETURNING id";
        return await Connection.ExecuteScalarAsync<int>(sql, dto, transaction: _transaction);
    }

    public async Task<Tag?> ObterPorIdAsync(int id)
    {
        var sql = "SELECT id, id_tag_tipo AS IdTagTipo, nome FROM tag WHERE id = @Id";
        return await Connection.QueryFirstOrDefaultAsync<Tag>(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<Tag?> ObterPorNomeEIdTagTipoAsync(string nome, int idTagTipo)
    {
        var sql = "SELECT id, id_tag_tipo AS IdTagTipo, nome FROM tag WHERE nome = @Nome AND id_tag_tipo = @IdTagTipo";
        return await Connection.QueryFirstOrDefaultAsync<Tag>(sql, new { Nome = nome, IdTagTipo = idTagTipo },
            transaction: _transaction);
    }

    public async Task<IEnumerable<Tag>> ObterTodosAsync(int limit, int offset, int idTagTipo)
    {
        var sql =
            "SELECT id, id_tag_tipo AS IdTagTipo, nome FROM tag WHERE id_tag_tipo = @IdTagTipo LIMIT @Limit OFFSET @Offset";
        return await Connection.QueryAsync<Tag>(sql, new { Limit = limit, Offset = offset, IdTagTipo = idTagTipo },
            transaction: _transaction);
    }
}