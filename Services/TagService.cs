using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class TagService {
    private readonly TagRepository _tagRepository;
    private readonly TagTipoRepository _tagTipoRepository;

    public TagService(TagRepository tagRepository, TagTipoRepository tagTipoRepository) {
        _tagRepository = tagRepository;
        _tagTipoRepository = tagTipoRepository;
    }

    internal async Task<Tag> Atualizar(int id, Tag dto) {
        var existente = await _tagRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para atualização");

        var tagTipo = await _tagTipoRepository.ObterPorIdAsync(dto.IdTagTipo);
        if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe"); 

        await _tagRepository.AtualizarAsync(id, dto);

        var atualizado = await _tagRepository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar a Tag");

        return atualizado;
    }

    internal async Task<Tag> Criar(Tag dto) {
        var existente = await _tagRepository.ObterPorNomeEIdTagTipoAsync(dto.Nome, dto.IdTagTipo);
        if (existente != null) throw new InvalidOperationException("O nome já está sendo utilizado neste tipo de tag");

        var tagTipo = await _tagTipoRepository.ObterPorIdAsync(dto.IdTagTipo);
        if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe"); 

        var novoId = await _tagRepository.InserirAsync(dto);
        var novaTag = await _tagRepository.ObterPorIdAsync(novoId);

        if (novaTag == null) throw new Exception("Falha ao criar e recuperar a nova Tag");

        return novaTag;
    }

    internal async Task DeletarPorId(int id) {
        var existente = await _tagRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para exclusão");

        await _tagRepository.DeletarPorIdAsync(id);

        var aindaExiste = await _tagRepository.ObterPorIdAsync(id);
        if (aindaExiste != null) throw new Exception("Não foi possível deletar esta Tag");
    }

    internal async Task<Tag> ObterPorId(int id) {
        var tag = await _tagRepository.ObterPorIdAsync(id);
        if (tag == null) throw new KeyNotFoundException("Tag não encontrada");

        return tag;
    }

    internal async Task<IEnumerable<Tag>> ObterTodos(int limit, int offset, int idTipoTag) {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        return await _tagRepository.ObterTodosAsync(limit, offset, idTipoTag);
    }
}

