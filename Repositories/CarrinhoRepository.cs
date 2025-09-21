using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class CarrinhoRepository {
    private readonly string _connectionString;

    public CarrinhoRepository(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    internal async Task<Carrinho?> ObterAsync(int idUsuario, int idProduto) {
        var sql = @"
            SELECT 
                id_usuario as IdUsuario,
                id_produto as IdProduto, 
                quantiade as Quantiade 
            FROM 
                carrinho 
            WHERE 
                id_usuario = @IdUsuario 
                AND id_produto = @IdProduto
        ";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Carrinho>(sql, new { IdUsuario = idUsuario, IdProduto = idProduto });
    }

    internal async Task InserirAsync(Carrinho carrinho) {
        var sql = @"
            INSERT INTO carrinho (
                id_usuario, 
                id_produto, 
                quantidade
            ) 
            VALUES (
                @IdUsuario, 
                @IdProduto, 
                @Quantidade
            )
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, carrinho);
    }

    internal async Task AtualizarQuantidadeAsync(int idUsuario, int idProduto, int quantidade) {
        var sql = @"
            UPDATE 
                carrinho 
            SET 
                quantidade = @Quantidade 
            WHERE 
                id_usuario = @IdUsuario 
                AND id_produto = @IdProduto
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdProduto = idProduto, Quantidade = quantidade });
    }

    internal async Task RemoverAsync(int idUsuario, int idProduto) {
        var sql = @"
            DELETE 
            FROM 
                carrinho 
            WHERE 
                id_usuario = @IdUsuario 
                AND id_produto = @IdProduto
        ";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdProduto = idProduto });
    }

    internal async Task<IEnumerable<(Carrinho Carrinho, int Preco, string Nome)>> ObterTodosAsync(int idUsuario) {
        var sql = @"
            SELECT 
                c.id_usuario, 
                c.id_produto, 
                c.quantidade, 
                SUM(p.preco), 
                p.nome
            FROM 
                carrinho c
                JOIN produto p ON p.id = c.id_produto
            WHERE 
                c.id_usuario = @IdUsuario
            GROUB BY 
                c.id_produto
        ";
        using var conn = Connection;
        return await conn.QueryAsync<Carrinho, int, string, (Carrinho, int, string)>(
            sql,
            (c, preco, nome) => (c, preco, nome),
            new { IdUsuario = idUsuario }
        );
    }

    internal async Task RemoverTodosAsync(int idUsuario) {
        var sql = "DELETE FROM carrinho WHERE id_usuario = @IdUsuario";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { IdUsuario = idUsuario });
    }
}

