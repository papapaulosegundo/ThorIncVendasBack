using Microsoft.AspNetCore.Mvc;
using ThorAPI.Services;

namespace ThorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrinhoController : ControllerBase {
    private readonly CarrinhoService _service;

    public CarrinhoController(CarrinhoService service) {
        _service = service;
    }

    [HttpPut("{idUsuario}/{idProduto}")]
    public async Task<IActionResult> Atualizar(int idUsuario, int idProduto, [FromQuery] int quantidade) {
        try {
            var resultado = await _service.AtualizarCarrinho(idUsuario, idProduto, quantidade);
            return Ok(resultado);
        } catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{idUsuario}")]
    public async Task<IActionResult> Get(int idUsuario) {
        try {
            var resultado = await _service.ObterCarrinho(idUsuario);
            return Ok(resultado);
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{idUsuario}")]
    public async Task<IActionResult> Delete(int idUsuario) {
        try {
            await _service.RemoverCarrinho(idUsuario);
            return NoContent();
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }
}

