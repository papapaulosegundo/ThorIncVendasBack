using ThorAPI.Models;
using ThorAPI.Repositories;

public class TagTipoService {
    private readonly TagTipoRepository _repository;

    public TagTipoService(TagTipoRepository repository) {
        _repository = repository;
    }

    public async Task<TagTipo> Criar(TagTipo tagTipo) {
        var existente = await _repository.ObterPorNomeAsync(tagTipo.Nome);
        if (existente != null) throw new InvalidOperationException("O nome já está sendo utilizado");

        var novoId = await _repository.InserirAsync(tagTipo);
        var novaTag = await _repository.ObterPorIdAsync(novoId);

        if (novaTag == null) throw new Exception("Falha ao criar e recuperar a nova TagTipo");

        return novaTag;
    }

    public async Task DeletarPorId(int id) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("TagTipo não encontrado para exclusão");

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

