using ThorAPI.Models;

namespace ThorAPI.Repositories;


public interface IEnderecoRepository
{
    Task<int> InserirAsync(Endereco endereco);
    Task<IEnumerable<Endereco>> ObterPorUsuarioIdAsync(int idUsuario);
    Task<Endereco?> ObterPorIdAsync(int id);
    Task AtualizarAsync(Endereco endereco);
    Task RemoverAsync(int id);
    Task LimparPrincipalAntigoAsync(int idUsuario, int idEnderecoAtual);
}