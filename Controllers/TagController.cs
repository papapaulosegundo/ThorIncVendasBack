using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;
using ThorAPI.Repositories;
using ThorAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;
    private readonly IUnitOfWork _uof;

    public TagController(TagService tagService, IUnitOfWork uof)
    {
        _tagService = tagService;
        _uof = uof;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Tag dto)
    {
        try
        {
            var resultado = await _tagService.Criar(dto);
            await _uof.CommitAsync();
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Tag dto)
    {
        try
        {
            var resultado = await _tagService.Atualizar(id, dto);
            await _uof.CommitAsync();
            return Ok(resultado);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _tagService.DeletarPorId(id);
            await _uof.CommitAsync();
            return NoContent();
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var resultado = await _tagService.ObterPorId(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return StatusCode(401, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit, [FromQuery] int offset, [FromQuery] int idTipoTag)
    {
        try
        {
            var resultado = await _tagService.ObterTodos(limit, offset, idTipoTag);
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }
}