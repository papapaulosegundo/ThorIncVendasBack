using ThorAPI.Models;

namespace ThorAPI.Repositories;

public interface IUsuarioRepository
{
    Task<int> CriarAsync(Usuario usuario);
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario?> ObterPorEmailSenhaAsync(string email, string senha);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(int id);
}