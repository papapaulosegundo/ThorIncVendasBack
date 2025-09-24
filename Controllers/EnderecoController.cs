using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;
using ThorAPI.Repositories;
using ThorAPI.Services;

namespace ThorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecoController : ControllerBase
{
    private readonly EnderecoService _enderecoService;
    private readonly IUnitOfWork _uof;

    public EnderecoController(EnderecoService enderecoService, IUnitOfWork uof)
    {
        _enderecoService = enderecoService;
        _uof = uof;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Endereco endereco)
    {
        try
        {
            var criado = await _enderecoService.CriarAsync(endereco);
            await _uof.CommitAsync();
            return StatusCode(201, criado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<IActionResult> ObterPorUsuario(int idUsuario)
    {
        try
        {
            var enderecos = await _enderecoService.ObterPorUsuarioIdAsync(idUsuario);
            return Ok(enderecos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Endereco endereco)
    {
        try
        {
            var atualizado = await _enderecoService.AtualizarAsync(id, endereco);
            await _uof.CommitAsync();
            return Ok(atualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        try
        {
            await _enderecoService.DeletarAsync(id);
            await _uof.CommitAsync();
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}