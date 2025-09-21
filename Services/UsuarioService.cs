using ThorAPI.DTOs;
using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class UsuarioService
{
    public readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CriarUsuarioAsync(UsuarioCreateDTO dto)
    {
        if (dto.Senha != dto.ConfirmarSenha)
            throw new ArgumentException("Senha e Confirmar Senha não coincidem.");

        var usuarioExistente = await _repository.ObterPorEmailAsync(dto.Email);
        if (usuarioExistente != null)
            throw new ArgumentException("Já existe um usuário com esse email.");

        // Aqui você pode hashear a senha antes de salvar
        var usuario = new Usuario()
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha // para produção: usar hash!
        };

        return await _repository.InserirAsync(usuario);
    }
}
