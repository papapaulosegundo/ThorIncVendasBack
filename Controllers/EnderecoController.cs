using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;
using ThorAPI.Services;

namespace ThorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnderecoController : ControllerBase {
    private readonly EnderecoService _service;

    public EnderecoController(EnderecoService service) {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] Endereco endereco) {
        try {
            var criado = await _service.CriarAsync(endereco);
            return StatusCode(201, criado);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<IActionResult> ObterPorUsuario(int idUsuario) {
        try {
            var enderecos = await _service.ObterPorUsuarioIdAsync(idUsuario);
            return Ok(enderecos);
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Endereco endereco)
    {
        try
        {
            var atualizado = await _service.AtualizarAsync(id, endereco);
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
    public async Task<IActionResult> Deletar(int id) {
        try {
            await _service.DeletarAsync(id);
            return NoContent();
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }
}