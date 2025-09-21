using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class ProdutoService {
    private readonly ProdutoRepository _produtoRepository;
    private readonly TagRepository _tagRepository;

    public ProdutoService(ProdutoRepository produtoRepository, TagRepository tagRepository) {
        _produtoRepository = produtoRepository;
        _tagRepository = tagRepository;
    }

    internal async Task<Produto> Criar(Produto dto) {
        var novoId = await _produtoRepository.InserirAsync(dto);
        var produto = await _produtoRepository.ObterPorIdAsync(novoId);
        if (produto == null) throw new Exception("Falha ao criar e recuperar o Produto");

        if (produto.IdTagTipo != null) {
            var tags = await _tagRepository.ObterTodosAsync(1000, 0, (int) produto.IdTagTipo);
            produto.tags = tags.ToArray();
        }

        return produto;
    }

    internal async Task<Produto> Atualizar(int id, Produto dto) {
        var existente = await _produtoRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Produto não encontrado para atualização");

        await _produtoRepository.AtualizarAsync(id, dto);

        var atualizado = await _produtoRepository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar o Produto");

        if (atualizado.IdTagTipo != null) {
            var tags = await _tagRepository.ObterTodosAsync(1000, 0, (int) atualizado.IdTagTipo);
            atualizado.tags = tags.ToArray();
        }

        return atualizado;
    }

    internal async Task DeletarPorId(int id) {
        var existente = await _produtoRepository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Produto não encontrado para exclusão");

        await _produtoRepository.DeletarPorIdAsync(id);

        var aindaExiste = await _produtoRepository.ObterPorIdAsync(id);
        if (aindaExiste != null) throw new Exception("Não foi possível deletar este Produto");
    }

    internal async Task<Produto> ObterPorId(int id) {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null) throw new KeyNotFoundException("Produto não encontrado");

        if (produto.IdTagTipo != null) {
            var tags = await _tagRepository.ObterTodosAsync(1000, 0, (int) produto.IdTagTipo);
            produto.tags = tags.ToArray();
        }

        return produto;
    }

    internal async Task<IEnumerable<Produto>> ObterTodos(int limit, int offset, string? nome = null) {
        if (limit == 0) throw new ArgumentException("A variável 'limit' não pode ser 0");

        var produtos = await _produtoRepository.ObterTodosAsync(limit, offset, nome);

        var produtosComTagsTasks = produtos.Select(async produto => {
            if (produto.IdTagTipo != null) {
                var tags = await _tagRepository.ObterTodosAsync(1000, 0, (int)produto.IdTagTipo);
                produto.tags = tags.ToArray();
            }
            return produto;
        });

        return await Task.WhenAll(produtosComTagsTasks);
    }

}
