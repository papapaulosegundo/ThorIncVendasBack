using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class TagTipoService {
    private readonly TagTipoRepository _repository;

    public TagTipoService(TagTipoRepository repository) {
        _repository = repository;
    }

    public async Task<TagTipo> Atualiza(int id, TagTipo tagTipo) {
        var existente = _repository.ObterPorIdAsync(id); 
        if (existente == null) throw new KeyNotFoundException("O tipo de tag não foi encontrado");

        var conflito = await _repository.ObterPorNomeAsync(tagTipo.Nome);
        if (conflito != null) {
            throw new InvalidOperationException("Este nome de tipo de tag ja esta sendo utilizado");
        }

        await _repository.Atualiza(id, tagTipo);

        var novaTag = await _repository.ObterPorIdAsync(id);
        if (novaTag == null) throw new Exception("Falha ao alterar e recuperar o novo tipo de tag");

        return novaTag;
    }

    public async Task<TagTipo> Criar(TagTipo tagTipo) {
        var existente = await _repository.ObterPorNomeAsync(tagTipo.Nome);
        if (existente != null) throw new InvalidOperationException("O nome já está sendo utilizado");

        var novoId = await _repository.InserirAsync(tagTipo);
        var novaTag = await _repository.ObterPorIdAsync(novoId);

        if (novaTag == null) throw new Exception("Falha ao criar e recuperar o novo tipo de tag");

        return novaTag;
    }

    public async Task DeletarPorId(int id) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tipo de tag não encontrado para exclusão");

        await _repository.DeletarPorIdAsync(id);

        var aindaExiste = await _repository.ObterPorIdAsync(id);
        if (aindaExiste != null) throw new Exception("Não foi possível deletar este tipo de tag");
    }

    public async Task<TagTipo?> ObterPorId(int id) {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<TagTipo?> ObterPorNome(string nome) {
        return await _repository.ObterPorNomeAsync(nome);
    }

    public async Task<IEnumerable<TagTipo>> ObterTodos(int limit, int offset) {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");
        return await _repository.ObterTodosAsync(limit, offset);
    }
}

