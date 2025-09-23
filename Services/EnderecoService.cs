using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class EnderecoService {
    private readonly EnderecoRepository _repository;
    private readonly UsuarioRepository _usuarioRepository; 

    public EnderecoService(EnderecoRepository repository, UsuarioRepository usuarioRepository) {
        _repository = repository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Endereco> CriarAsync(Endereco endereco) {
        var usuario = await _usuarioRepository.ObterPorIdAsync(endereco.IdUsuario);
        if (usuario == null) {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }
        
        if (endereco.IsPrincipal) {
            await _repository.LimparPrincipalAntigoAsync(endereco.IdUsuario, 0);
        }

        var novoId = await _repository.InserirAsync(endereco);
        var novoEndereco = await _repository.ObterPorIdAsync(novoId);
        if (novoEndereco == null) throw new Exception("Falha ao criar e recuperar o endereço.");

        return novoEndereco;
    }

    public async Task<IEnumerable<Endereco>> ObterPorUsuarioIdAsync(int idUsuario) {
        return await _repository.ObterPorUsuarioIdAsync(idUsuario);
    }
    
    public async Task<Endereco> AtualizarAsync(int id, Endereco endereco)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }
        
        endereco.Id = id;
        endereco.IdUsuario = existente.IdUsuario;
        
        if (endereco.IsPrincipal)
        {
            await _repository.LimparPrincipalAntigoAsync(endereco.IdUsuario, id);
        }

        await _repository.AtualizarAsync(endereco);
        
        var atualizado = await _repository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar o endereço.");
        
        return atualizado;
    }

    public async Task DeletarAsync(int id) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }
        await _repository.RemoverAsync(id);
    }
}