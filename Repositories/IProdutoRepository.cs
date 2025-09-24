using ThorAPI.Models;

namespace ThorAPI.Repositories;

public interface IProdutoRepository
{
    Task<int> InserirAsync(Produto dto);
    Task AtualizarAsync(int id, Produto dto);
    Task DeletarPorIdAsync(int id);
    Task<Produto?> ObterPorIdAsync(int id);
    Task<IEnumerable<Produto>> ObterTodosAsync(int limit, int offset, string? nome = null);
}