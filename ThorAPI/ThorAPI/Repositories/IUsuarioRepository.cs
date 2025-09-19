using ThorAPI.Models;

namespace ThorAPI.Repositories;

public interface IUsuarioRepository
{
    Task<int> InserirAsync(Usuario usuario);
    Task<Usuario?> ObterPorEmailAsync(string email);
}