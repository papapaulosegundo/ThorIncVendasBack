using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class CarrinhoService {
    private readonly CarrinhoRepository _repository;

    public CarrinhoService(CarrinhoRepository repository) {
        _repository = repository;
    }

    internal async Task<object> AtualizarCarrinho(int idUsuario, int idProduto, int quantidade) {
        var existente = await _repository.ObterAsync(idUsuario, idProduto);

        if (existente == null && quantidade > 0) {
            await _repository.InserirAsync(new Carrinho {
                IdUsuario = idUsuario,
                IdProduto = idProduto,
                Quantidade = quantidade
            });
            return await ObterCarrinho(idUsuario);
        }

        if (existente == null) {
            throw new InvalidOperationException("O item precisa ter uma quantidade de pelo menos um para ser incluido");
        }

        if (quantidade <= 0) {
            await _repository.RemoverAsync(idUsuario, idProduto);
        } else {
            await _repository.AtualizarQuantidadeAsync(idUsuario, idProduto, quantidade);
        }
        return await ObterCarrinho(idUsuario);
    }

    internal async Task<object> ObterCarrinho(int idUsuario) {
        var itens = await _repository.ObterTodosAsync(idUsuario);

        var total = 0;

        var produtos = itens.Select(i => {
            total += i.Preco * i.Carrinho.Quantidade;
            return new {
                Produto = i.Nome,
                Quantidade = i.Carrinho.Quantidade,
                PrecoUnitario = i.Preco,
                PrecoTotal = i.Preco * i.Carrinho.Quantidade
            };
        });

        return new { Total = total, Produtos = produtos };
    }

    internal async Task RemoverCarrinho(int idUsuario) {
        await _repository.RemoverTodosAsync(idUsuario);
    }
}

