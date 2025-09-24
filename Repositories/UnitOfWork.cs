using System.Data;
using Npgsql;

namespace ThorAPI.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private bool _disposed;
    
    private IUsuarioRepository? _usuarioRepository;
    private IProdutoRepository? _produtoRepository;
    private ICarrinhoRepository? _carrinhoRepository;
    private ITagRepository? _tagRepository;
    private ITagTipoRepository? _tagTipoRepository;
    private IEnderecoRepository? _enderecoRepository;


    public UnitOfWork(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }
    
    public IUsuarioRepository UsuarioRepository => _usuarioRepository ??= new UsuarioRepository(_transaction);
    public IProdutoRepository ProdutoRepository => _produtoRepository ??= new ProdutoRepository(_transaction);
    public ICarrinhoRepository CarrinhoRepository => _carrinhoRepository ??= new CarrinhoRepository(_transaction);
    public ITagRepository TagRepository => _tagRepository ??= new TagRepository(_transaction);
    public ITagTipoRepository TagTipoRepository => _tagTipoRepository ??= new TagTipoRepository(_transaction);
    public IEnderecoRepository EnderecoRepository => _enderecoRepository ??= new EnderecoRepository(_transaction);


    public async Task CommitAsync()
    {
        try
        {
            await ((NpgsqlTransaction)_transaction).CommitAsync();
        }
        catch
        {
            await ((NpgsqlTransaction)_transaction).RollbackAsync();
            throw;
        }
        finally
        {
            _transaction.Dispose();
        }
    }

    public void Rollback()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }

    public void Dispose()
    {
        dispose(true);
        GC.SuppressFinalize(this);
    }

    private void dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
            _disposed = true;
        }
    }

    ~UnitOfWork()
    {
        dispose(false);
    }
}
