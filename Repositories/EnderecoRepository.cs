using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class EnderecoRepository : IEnderecoRepository
{
    private readonly IDbTransaction _transaction;
    private IDbConnection Connection => _transaction.Connection;

    public EnderecoRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<int> InserirAsync(Endereco endereco)
    {
        const string sql = @"
            INSERT INTO endereco (id_usuario, rua, numero, complemento, bairro, cidade, estado, cep, is_principal) 
            VALUES (@IdUsuario, @Rua, @Numero, @Complemento, @Bairro, @Cidade, @Estado, @CEP, @IsPrincipal) 
            RETURNING id;";
        return await Connection.ExecuteScalarAsync<int>(sql, endereco, transaction: _transaction);
    }

    public async Task<IEnumerable<Endereco>> ObterPorUsuarioIdAsync(int idUsuario)
    {
        const string sql = @"
            SELECT id, id_usuario AS IdUsuario, rua, numero, complemento, bairro, cidade, estado, cep, is_principal AS IsPrincipal
            FROM endereco WHERE id_usuario = @IdUsuario;";
        return await Connection.QueryAsync<Endereco>(sql, new { IdUsuario = idUsuario }, transaction: _transaction);
    }

    public async Task<Endereco?> ObterPorIdAsync(int id)
    {
        const string sql = @"
            SELECT id, id_usuario AS IdUsuario, rua, numero, complemento, bairro, cidade, estado, cep, is_principal AS IsPrincipal
            FROM endereco WHERE id = @Id;";
        return await Connection.QueryFirstOrDefaultAsync<Endereco>(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task AtualizarAsync(Endereco endereco)
    {
        const string sql = @"
            UPDATE endereco SET rua = @Rua, numero = @Numero, complemento = @Complemento, bairro = @Bairro, 
                               cidade = @Cidade, estado = @Estado, cep = @CEP, is_principal = @IsPrincipal
            WHERE id = @Id;";
        await Connection.ExecuteAsync(sql, endereco, transaction: _transaction);
    }

    public async Task RemoverAsync(int id)
    {
        const string sql = "DELETE FROM endereco WHERE id = @Id;";
        await Connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task LimparPrincipalAntigoAsync(int idUsuario, int idEnderecoAtual)
    {
        const string sql = @"
            UPDATE endereco 
            SET is_principal = false 
            WHERE id_usuario = @IdUsuario AND id <> @IdEnderecoAtual AND is_principal = true;";
        await Connection.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdEnderecoAtual = idEnderecoAtual },
            transaction: _transaction);
    }
}