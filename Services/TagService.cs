using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class TagService
{
    private readonly IUnitOfWork _uof;

    public TagService(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task<Tag> Atualizar(int id, Tag dto)
    {
        var existente = await _uof.TagRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para atualização");

        var tagTipo = await _uof.TagTipoRepository.ObterPorIdAsync(dto.IdTagTipo);
        if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");

        await _uof.TagRepository.AtualizarAsync(id, dto);

        var atualizado = await _uof.TagRepository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar a Tag");

        return atualizado;
    }

    public async Task<Tag> Criar(Tag dto)
    {
        var existente = await _uof.TagRepository.ObterPorNomeEIdTagTipoAsync(dto.Nome, dto.IdTagTipo);
        if (existente != null) throw new InvalidOperationException("O nome já está sendo utilizado neste tipo de tag");

        var tagTipo = await _uof.TagTipoRepository.ObterPorIdAsync(dto.IdTagTipo);
        if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");

        var novoId = await _uof.TagRepository.InserirAsync(dto);
        var novaTag = await _uof.TagRepository.ObterPorIdAsync(novoId);

        if (novaTag == null) throw new Exception("Falha ao criar e recuperar a nova Tag");

        return novaTag;
    }

    public async Task DeletarPorId(int id)
    {
        var existente = await _uof.TagRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Tag não encontrada para exclusão");

        await _uof.TagRepository.DeletarPorIdAsync(id);

        var aindaExiste = await _uof.TagRepository.ObterPorIdAsync(id);
        if (aindaExiste != null) throw new Exception("Não foi possível deletar esta Tag");
    }

    public async Task<Tag> ObterPorId(int id)
    {
        var tag = await _uof.TagRepository.ObterPorIdAsync(id);
        if (tag == null) throw new KeyNotFoundException("Tag não encontrada");

        return tag;
    }

    public async Task<IEnumerable<Tag>> ObterTodos(int limit, int offset, int idTipoTag)
    {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        return await _uof.TagRepository.ObterTodosAsync(limit, offset, idTipoTag);
    }
}