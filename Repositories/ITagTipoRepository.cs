using ThorAPI.Models;

namespace ThorAPI.Repositories;

public interface ITagTipoRepository
{
    Task<int> InserirAsync(TagTipo tagTipo);
    Task<TagTipo?> ObterPorIdAsync(int id);
    Task<IEnumerable<TagTipo>> ObterTodosAsync(int limit, int offset);
    Task Atualiza(int id, TagTipo tagTipo);
    Task DeletarPorIdAsync(int id);
    Task<TagTipo?> ObterPorNomeAsync(string nome);
}