using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class UsuarioService {
    private readonly UsuarioRepository _repository;

    public UsuarioService(UsuarioRepository repository) {
        _repository = repository;
    }

    public async Task<Usuario> Criar(Usuario usuario) {
        var jaExiste = await _repository.ObterPorEmailAsync(usuario.Email);
        if (jaExiste != null) throw new InvalidOperationException("Email ja esta sendo utilizado");

        var id = await _repository.CriarAsync(usuario);

        var criado = await _repository.ObterPorIdAsync(id);
        if (criado == null) throw new Exception("Falha ao criar usuário");

        return criado;
    }

    public async Task<Usuario> Login(string email, string senha) {
        var usuario = await _repository.ObterPorEmailSenhaAsync(email, senha);
        if (usuario == null) throw new UnauthorizedAccessException("Email ou senha inválidos");
        return usuario;
    }

    public async Task<Usuario> ObterPorId(int id) {
        var usuario = await _repository.ObterPorIdAsync(id);
        if (usuario == null) throw new KeyNotFoundException("Usuário não encontrado");
        return usuario;
    }

    public async Task<Usuario> Atualizar(int id, Usuario usuario) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Usuário não encontrado");

        usuario.Id = id;
        await _repository.AtualizarAsync(usuario);
        return usuario;
    }

    public async Task Deletar(int id) {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente == null) throw new KeyNotFoundException("Usuário não encontrado");

        await _repository.DeletarAsync(id);
    }
}

