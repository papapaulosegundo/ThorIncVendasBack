using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbTransaction _transaction;
    private IDbConnection Connection => _transaction.Connection;

    public UsuarioRepository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<int> CriarAsync(Usuario usuario)
    {
        const string sql = @"
            INSERT INTO usuario (cpf, nome, email, senha)
            VALUES (@Cpf, @Nome, @Email, @Senha)
            RETURNING id";
        return await Connection.ExecuteScalarAsync<int>(sql, usuario, transaction: _transaction);
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        const string sql = "SELECT id, cpf, nome, email, senha FROM usuario WHERE id = @Id";
        return await Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        const string sql =
            "SELECT id as Id, cpf as Cpf, nome as Nome, email as Email, senha as Senha FROM usuario WHERE email = @Email";
        return await Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email },
            transaction: _transaction);
    }

    public async Task<Usuario?> ObterPorEmailSenhaAsync(string email, string senha)
    {
        const string sql = "SELECT id, cpf, nome, email, senha FROM usuario WHERE email = @Email AND senha = @Senha";
        return await Connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email, Senha = senha },
            transaction: _transaction);
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        const string sql = @"
            UPDATE usuario
            SET cpf = @Cpf, nome = @Nome, email = @Email, senha = @Senha
            WHERE id = @Id";
        await Connection.ExecuteAsync(sql, usuario, transaction: _transaction);
    }

    public async Task DeletarAsync(int id)
    {
        const string sql = "DELETE FROM usuario WHERE id = @Id";
        await Connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }
}