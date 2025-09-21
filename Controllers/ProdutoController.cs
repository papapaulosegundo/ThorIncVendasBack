using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;
using ThorAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase {
    private readonly ProdutoService _service;

    public ProdutoController(ProdutoService service) {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Produto dto) {
        try {
            var resultado = await _service.Criar(dto);
            return Ok(resultado);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Produto dto) {
        try {
            var resultado = await _service.Atualizar(id, dto);
            return Ok(resultado);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        try {
            await _service.DeletarPorId(id);
            return NoContent();
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        try {
            var resultado = await _service.ObterPorId(id);
            return Ok(resultado);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int limit,
        [FromQuery] int offset,
        [FromQuery] string? nome = null
    ) {
        try {
            var resultado = await _service.ObterTodos(limit, offset, nome);
            return Ok(resultado);
        } catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }
}
