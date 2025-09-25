using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models.DTOs;
using ThorAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase {
    private readonly CategoriaService _svc;
    public CategoriaController(CategoriaService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CategoriaCreateDto dto) {
        try {
            var c = await _svc.Criar(dto.Nome, dto.Descricao, dto.Ativa);
            return Ok(c);
        } catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] CategoriaUpdateDto dto) {
        try {
            var c = await _svc.Atualizar(id, dto.Nome, dto.Descricao, dto.Ativa);
            return Ok(c);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        } catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) {
        var c = await _svc.ObterPorId(id);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug) {
        var c = await _svc.ObterPorSlug(slug);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit=50, [FromQuery] int offset=0, [FromQuery] string? nome=null) {
        var lst = await _svc.Listar(limit, offset, nome);
        return Ok(lst);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) {
        await _svc.Deletar(id);
        return NoContent();
    }
}
