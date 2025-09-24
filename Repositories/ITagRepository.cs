using ThorAPI.Models;

namespace ThorAPI.Repositories;

public interface ITagRepository
{
    Task AtualizarAsync(int id, Tag dto);
    Task DeletarPorIdAsync(int id);
    Task<int> InserirAsync(Tag dto);
    Task<Tag?> ObterPorIdAsync(int id);
    Task<Tag?> ObterPorNomeEIdTagTipoAsync(string nome, int idTagTipo);
    Task<IEnumerable<Tag>> ObterTodosAsync(int limit, int offset, int idTagTipo);
}