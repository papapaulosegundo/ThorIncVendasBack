using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class EnderecoService
{
    private readonly IUnitOfWork _uof;

    public EnderecoService(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public async Task<Endereco> CriarAsync(Endereco endereco)
    {
        var usuario = await _uof.UsuarioRepository.ObterPorIdAsync(endereco.IdUsuario);
        if (usuario == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        if (endereco.IsPrincipal)
        {
            await _uof.EnderecoRepository.LimparPrincipalAntigoAsync(endereco.IdUsuario, 0);
        }

        var novoId = await _uof.EnderecoRepository.InserirAsync(endereco);
        var novoEndereco = await _uof.EnderecoRepository.ObterPorIdAsync(novoId);
        if (novoEndereco == null) throw new Exception("Falha ao criar e recuperar o endereço.");

        return novoEndereco;
    }

    public async Task<IEnumerable<Endereco>> ObterPorUsuarioIdAsync(int idUsuario)
    {
        return await _uof.EnderecoRepository.ObterPorUsuarioIdAsync(idUsuario);
    }

    public async Task<Endereco> AtualizarAsync(int id, Endereco endereco)
    {
        var existente = await _uof.EnderecoRepository.ObterPorIdAsync(id);
        if (existente == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        endereco.Id = id;
        endereco.IdUsuario = existente.IdUsuario;

        if (endereco.IsPrincipal)
        {
            await _uof.EnderecoRepository.LimparPrincipalAntigoAsync(endereco.IdUsuario, id);
        }

        await _uof.EnderecoRepository.AtualizarAsync(endereco);

        var atualizado = await _uof.EnderecoRepository.ObterPorIdAsync(id);
        if (atualizado == null) throw new Exception("Falha ao atualizar e recuperar o endereço.");

        return atualizado;
    }

    public async Task DeletarAsync(int id)
    {
        var existente = await _uof.EnderecoRepository.ObterPorIdAsync(id);
        if (existente == null)
        {
            throw new KeyNotFoundException("Endereço não encontrado.");
        }

        await _uof.EnderecoRepository.RemoverAsync(id);
    }
}