using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Criar(Usuario usuario)
    {
        try
        {
            var criado = await _usuarioService.Criar(usuario);
            await _uof.CommitAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(401, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Usuario login)
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
    public async Task<IActionResult> Atualizar(int id, Usuario usuario)
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