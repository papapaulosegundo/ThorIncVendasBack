using ThorAPI.Models;

namespace ThorAPI.Repositories;

public interface ICarrinhoRepository
{
    Task<Carrinho?> ObterAsync(int idUsuario, int idProduto);
    Task InserirAsync(Carrinho carrinho);
    Task AtualizarQuantidadeAsync(int idUsuario, int idProduto, int quantidade);
    Task RemoverAsync(int idUsuario, int idProduto);
    Task<IEnumerable<(Carrinho Carrinho, int Preco, string Nome)>> ObterTodosAsync(int idUsuario);
    Task RemoverTodosAsync(int idUsuario);
}