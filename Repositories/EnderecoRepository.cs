using System.Data;
using Dapper;
using Npgsql;
using ThorAPI.Models;

namespace ThorAPI.Repositories;

public class EnderecoRepository {
    private readonly string _connectionString;

    public EnderecoRepository(IConfiguration configuration) {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new NpgsqlConnection(_connectionString);

    public async Task<int> InserirAsync(Endereco endereco) {
        const string sql = @"
            INSERT INTO endereco (
                id_usuario, rua, numero, complemento, bairro, cidade, estado, cep, is_principal
            ) VALUES (
                @IdUsuario, @Rua, @Numero, @Complemento, @Bairro, @Cidade, @Estado, @CEP, @IsPrincipal
            ) RETURNING id;";
        using var conn = Connection;
        return await conn.ExecuteScalarAsync<int>(sql, endereco);
    }

    public async Task<IEnumerable<Endereco>> ObterPorUsuarioIdAsync(int idUsuario) {
        const string sql = @"
            SELECT id, id_usuario AS IdUsuario, rua, numero, complemento, bairro, cidade, estado, cep, is_principal AS IsPrincipal
            FROM endereco WHERE id_usuario = @IdUsuario;";
        using var conn = Connection;
        return await conn.QueryAsync<Endereco>(sql, new { IdUsuario = idUsuario });
    }
    
    public async Task<Endereco?> ObterPorIdAsync(int id)
    {
        const string sql = @"
            SELECT id, id_usuario AS IdUsuario, rua, numero, complemento, bairro, cidade, estado, cep, is_principal AS IsPrincipal
            FROM endereco WHERE id = @Id;";
        using var conn = Connection;
        return await conn.QueryFirstOrDefaultAsync<Endereco>(sql, new { Id = id });
    }

    public async Task AtualizarAsync(Endereco endereco) {
        const string sql = @"
            UPDATE endereco SET
                rua = @Rua,
                numero = @Numero,
                complemento = @Complemento,
                bairro = @Bairro,
                cidade = @Cidade,
                estado = @Estado,
                cep = @CEP,
                is_principal = @IsPrincipal
            WHERE id = @Id;";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, endereco);
    }

    public async Task RemoverAsync(int id) {
        const string sql = "DELETE FROM endereco WHERE id = @Id;";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { Id = id });
    }
    
    public async Task LimparPrincipalAntigoAsync(int idUsuario, int idEnderecoAtual)
    {
        const string sql = @"
            UPDATE endereco 
            SET is_principal = false 
            WHERE id_usuario = @IdUsuario AND id <> @IdEnderecoAtual AND is_principal = true;";
        using var conn = Connection;
        await conn.ExecuteAsync(sql, new { IdUsuario = idUsuario, IdEnderecoAtual = idEnderecoAtual });
    }
}