using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ThorAPI.Models;
using ThorAPI.Repositories;
using ThorAPI.Services;
using ThorAPI.Utils;

namespace ThorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;
    private readonly IUnitOfWork _uof;

    public UsuarioController(UsuarioService usuarioService, IUnitOfWork uof)
    {
        _usuarioService = usuarioService;
        _uof = uof;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] Usuario usuario)
    {
        try
        {
            if (usuario is null) return BadRequest("Body vazio.");
            if (string.IsNullOrWhiteSpace(usuario.Nome) ||
                string.IsNullOrWhiteSpace(usuario.Email) ||
                string.IsNullOrWhiteSpace(usuario.Senha))
                return BadRequest("Nome, Email e Senha são obrigatórios.");

            // Default sem sobrescrever papéis especiais:
            if (string.IsNullOrWhiteSpace(usuario.Tipo))
                usuario.Tipo = "usuario";

            var criado = await _usuarioService.Criar(usuario);
            await _uof.CommitAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
        }
        catch (InvalidOperationException ex)
        {
            // Mantém o comportamento anterior (email em uso -> 401)
            return StatusCode(401, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] Usuario login)
    {
        try
        {
            var usuario = await _usuarioService.Login(login.Email, login.Senha);
            return Ok(new
            {
                Usuario = usuario,
                Jwt = Jwt.GenerateToken(usuario)
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var usuario = await _usuarioService.ObterPorId(id);
            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario usuario)
    {
        try
        {
            var atualizado = await _usuarioService.Atualizar(id, usuario);
            await _uof.CommitAsync();
            return Ok(atualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        try
        {
            await _usuarioService.Deletar(id);
            await _uof.CommitAsync();
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}