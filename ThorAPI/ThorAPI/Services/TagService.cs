using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class TagService {
    private readonly TagRepository _repository;

    public TagService(TagRepository repository) {
        _repository = repository;
    }

    internal async Task<Tag> Atualizar(int id, Tag dto) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para atualização");

        await _repository.AtualizarAsync(id, dto);

        var atualizado = await _repository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar a Tag");

        return atualizado;
    }

    internal async Task<Tag> Criar(Tag dto) {
        var existente = await _repository.ObterPorNomeEIdTagTipoAsync(dto.Nome, dto.IdTagTipo);
        if (existente != null) throw new InvalidOperationException("O nome já está sendo utilizado neste tipo de tag");

        var novoId = await _repository.InserirAsync(dto);
        var novaTag = await _repository.ObterPorIdAsync(novoId);

        if (novaTag == null) throw new Exception("Falha ao criar e recuperar a nova Tag");

        return novaTag;
    }

    internal async Task DeletarPorId(int id) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para exclusão");

        await _repository.DeletarPorIdAsync(id);

        var aindaExiste = await _repository.ObterPorIdAsync(id);
        if (aindaExiste != null) throw new Exception("Não foi possível deletar esta Tag");
    }

    internal async Task<Tag> ObterPorId(int id) {
        var tag = await _repository.ObterPorIdAsync(id);
        if (tag == null) throw new KeyNotFoundException("Tag não encontrada");

        return tag;
    }

    internal async Task<IEnumerable<Tag>> ObterTodos(int limit, int offset, int idTipoTag) {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        return await _repository.ObterTodosAsync(limit, offset, idTipoTag);
    }
}

