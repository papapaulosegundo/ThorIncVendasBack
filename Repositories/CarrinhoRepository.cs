using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class CarrinhoRepository : ICarrinhoRepository
{
    private readonly IDbTransaction _transaction;
    private IDbConnection Connection => _transaction.Connection;

    public CarrinhoRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<Carrinho?> ObterAsync(int idUsuario, int idProduto)
    {
        var sql =
            "SELECT id_usuario as IdUsuario, id_produto as IdProduto, quantidade as Quantiade FROM carrinho WHERE id_usuario = @IdUsuario AND id_produto = @IdProduto";
        return await Connection.QueryFirstOrDefaultAsync<Carrinho>(sql,
            new { IdUsuario = idUsuario, IdProduto = idProduto }, transaction: _transaction);
    }

    public async Task InserirAsync(Carrinho carrinho)
    {
        var sql =
            "INSERT INTO carrinho (id_usuario, id_produto, quantidade) VALUES (@IdUsuario, @IdProduto, @Quantidade)";
        await Connection.ExecuteAsync(sql, carrinho, transaction: _transaction);
    }

    public async Task AtualizarQuantidadeAsync(int idUsuario, int idProduto, int quantidade)
    {
        var sql =
            "UPDATE carrinho SET quantidade = @Quantidade WHERE id_usuario = @IdUsuario AND id_produto = @IdProduto";
        await Connection.ExecuteAsync(sql,
            new { IdUsuario = idUsuario, IdProduto = idProduto, Quantidade = quantidade }, transaction: _transaction);
    }

    public async Task RemoverAsync(int idUsuario, int idProduto)
    {
        var sql = "DELETE FROM carrinho WHERE id_usuario = @IdUsuario AND id_produto = @IdProduto";
        await Connection.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdProduto = idProduto },
            transaction: _transaction);
    }

    public async Task<IEnumerable<(Carrinho Carrinho, int Preco, string Nome)>> ObterTodosAsync(int idUsuario)
    {
        var sql = @"
            SELECT c.id_usuario as IdUsuario, c.id_produto as IdProduto, c.quantidade as Quantidade, 
                   p.preco as Preco, p.nome as Nome
            FROM carrinho c
            JOIN produto p ON p.id = c.id_produto
            WHERE c.id_usuario = @IdUsuario";
        return await Connection.QueryAsync<Carrinho, int, string, (Carrinho, int, string)>(
            sql,
            (c, preco, nome) => (c, preco, nome),
            new { IdUsuario = idUsuario },
            splitOn: "Preco,Nome",
            transaction: _transaction
        );
    }

    public async Task RemoverTodosAsync(int idUsuario)
    {
        var sql = "DELETE FROM carrinho WHERE id_usuario = @IdUsuario";
        await Connection.ExecuteAsync(sql, new { IdUsuario = idUsuario }, transaction: _transaction);
    }
}