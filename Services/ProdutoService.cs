using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class ProdutoService
{
    private IUnitOfWork _uof;

    public ProdutoService(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task<Produto> Criar(Produto dto)
    {
        if (dto.IdTagTipo != null)
        {
            var tagTipo = await _uof.TagTipoRepository.ObterPorIdAsync((int)dto.IdTagTipo);
            if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");
        }

        var novoId = await _uof.ProdutoRepository.InserirAsync(dto);
        var produto = await _uof.ProdutoRepository.ObterPorIdAsync(novoId);
        if (produto == null) throw new Exception("Falha ao criar e recuperar o Produto");

        if (produto.IdTagTipo != null)
        {
            var tags = await _uof.TagRepository.ObterTodosAsync(1000, 0, (int)produto.IdTagTipo);
            produto.tags = tags.ToArray();
        }

        return produto;
    }

    public async Task<Produto> Atualizar(int id, Produto dto)
    {
        var existente = await _uof.ProdutoRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Produto não encontrado para atualização");

        if (dto.IdTagTipo != null)
        {
            var tagTipo = await _uof.TagTipoRepository.ObterPorIdAsync((int)dto.IdTagTipo);
            if (tagTipo == null) throw new InvalidOperationException("O tipo de tag solicitado não existe");
        }

        await _uof.ProdutoRepository.AtualizarAsync(id, dto);

        var atualizado = await _uof.ProdutoRepository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar o Produto");

        if (atualizado.IdTagTipo != null)
        {
            var tags = await _uof.TagRepository.ObterTodosAsync(1000, 0, (int)atualizado.IdTagTipo);
            atualizado.tags = tags.ToArray();
        }

        return atualizado;
    }

    public async Task DeletarPorId(int id)
    {
        var existente = await _uof.ProdutoRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Produto não encontrado para exclusão");

        await _uof.ProdutoRepository.DeletarPorIdAsync(id);

        var aindaExiste = await _uof.ProdutoRepository.ObterPorIdAsync(id);
        if (aindaExiste != null) throw new Exception("Não foi possível deletar este Produto");
    }

    public async Task<Produto> ObterPorId(int id)
    {
        var produto = await _uof.ProdutoRepository.ObterPorIdAsync(id);
        if (produto == null) throw new KeyNotFoundException("Produto não encontrado");

        if (produto.IdTagTipo != null)
        {
            var tags = await _uof.TagRepository.ObterTodosAsync(1000, 0, (int)produto.IdTagTipo);
            produto.tags = tags.ToArray();
        }

        return produto;
    }

    public async Task<IEnumerable<Produto>> ObterTodos(int limit, int offset, string? nome = null, int? idCategoria = null)
    {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        var produtos = await _uof.ProdutoRepository.ObterTodosAsync(limit, offset, nome, idCategoria);

        var produtosComTagsTasks = produtos.Select(async produto =>
        {
            if (produto.IdTagTipo != null)
            {
                var tags = await _uof.TagRepository.ObterTodosAsync(1000, 0, (int)produto.IdTagTipo);
                produto.tags = tags.ToArray();
            }

            return produto;
        });

        return await Task.WhenAll(produtosComTagsTasks);
    }
}