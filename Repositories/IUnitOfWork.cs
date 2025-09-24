namespace ThorAPI.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository UsuarioRepository { get; }
    IProdutoRepository ProdutoRepository { get; }
    ICarrinhoRepository CarrinhoRepository { get; }
    ITagRepository TagRepository { get; }
    ITagTipoRepository TagTipoRepository { get; }
    IEnderecoRepository EnderecoRepository { get; }
    
    Task CommitAsync();
    void Rollback();
}