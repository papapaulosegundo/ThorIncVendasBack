using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class CarrinhoService
{
    private readonly IUnitOfWork _uof;

    public CarrinhoService(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task<object> AtualizarCarrinho(int idUsuario, int idProduto, int quantidade)
    {
        var existente = await _uof.CarrinhoRepository.ObterAsync(idUsuario, idProduto);

        if (existente == null && quantidade > 0)
        {
            await _uof.CarrinhoRepository.InserirAsync(new Carrinho
            {
                IdUsuario = idUsuario,
                IdProduto = idProduto,
                Quantidade = quantidade
            });
            return await ObterCarrinho(idUsuario);
        }

        if (existente == null)
        {
            throw new InvalidOperationException("O item precisa ter uma quantidade de pelo menos um para ser incluido");
        }

        if (quantidade <= 0)
        {
            await _uof.CarrinhoRepository.RemoverAsync(idUsuario, idProduto);
        }
        else
        {
            await _uof.CarrinhoRepository.AtualizarQuantidadeAsync(idUsuario, idProduto, quantidade);
        }

        return await ObterCarrinho(idUsuario);
    }

    public async Task<object> ObterCarrinho(int idUsuario)
    {
        var itens = await _uof.CarrinhoRepository.ObterTodosAsync(idUsuario);

        var total = 0;

        var produtos = itens.Select(i =>
        {
            total += i.Preco * i.Carrinho.Quantidade;
            return new
            {
                IdProduto = i.Carrinho.IdProduto,
                Produto = i.Nome,
                Quantidade = i.Carrinho.Quantidade,
                PrecoUnitario = i.Preco,
                PrecoTotal = i.Preco * i.Carrinho.Quantidade
            };
        }).ToList();

        return new { Total = total, Produtos = produtos };
    }

    public async Task RemoverCarrinho(int idUsuario)
    {
        await _uof.CarrinhoRepository.RemoverTodosAsync(idUsuario);
    }
}