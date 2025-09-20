using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class TagTipoController : ControllerBase {
    private readonly TagTipoService _service;

    public TagTipoController(TagTipoService service) {
        _service = service;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TagTipo dto) {
        try {
            var resultado = await _service.Atualiza(id, dto);
            return Ok(resultado);
        } catch (InvalidOperationException ex) {
            return NotFound(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TagTipo dto) {
        try {
            var resultado = await _service.Criar(dto);
            return StatusCode(201, resultado);
        } catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        } catch (InvalidOperationException ex) {
            return Conflict(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit, [FromQuery] int offset) {
        try {
            var resultadoPaginado = await _service.ObterTodos(limit, offset);
            return Ok(resultadoPaginado);
        } catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        try {
            var resultado = await _service.ObterPorId(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
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
}

