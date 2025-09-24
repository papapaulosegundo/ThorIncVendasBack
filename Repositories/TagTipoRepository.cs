using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class TagTipoRepository : ITagTipoRepository
{
    private readonly IDbTransaction _transaction;
    private IDbConnection Connection => _transaction.Connection;

    public TagTipoRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<int> InserirAsync(TagTipo tagTipo)
    {
        var sql = "INSERT INTO tag_tipo (nome) VALUES (@Nome) RETURNING id;";
        return await Connection.ExecuteScalarAsync<int>(sql, new { tagTipo.Nome }, transaction: _transaction);
    }

    public async Task<TagTipo?> ObterPorIdAsync(int id)
    {
        var sql = "SELECT id, nome FROM tag_tipo WHERE id = @Id";
        return await Connection.QueryFirstOrDefaultAsync<TagTipo>(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<IEnumerable<TagTipo>> ObterTodosAsync(int limit, int offset)
    {
        var sql = "SELECT id, nome FROM tag_tipo LIMIT @Limit OFFSET @Offset";
        return await Connection.QueryAsync<TagTipo>(sql, new { Limit = limit, Offset = offset },
            transaction: _transaction);
    }

    public async Task Atualiza(int id, TagTipo tagTipo)
    {
        var sql = "UPDATE tag_tipo SET nome = @Nome WHERE id = @Id";
        await Connection.ExecuteAsync(sql, new { Id = id, Nome = tagTipo.Nome }, transaction: _transaction);
    }

    public async Task DeletarPorIdAsync(int id)
    {
        var sql = "DELETE FROM tag_tipo WHERE id = @Id";
        await Connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<TagTipo?> ObterPorNomeAsync(string nome)
    {
        var sql = "SELECT id, nome FROM tag_tipo WHERE nome = @Nome";
        return await Connection.QueryFirstOrDefaultAsync<TagTipo>(sql, new { Nome = nome }, transaction: _transaction);
    }
}