using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;
using ThorAPI.Repositories;
using ThorAPI.Services;

namespace ThorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagTipoController : ControllerBase
{
    private readonly TagTipoService _tagTipoService;
    private readonly IUnitOfWork _uof;

    public TagTipoController(TagTipoService tagTipoService, IUnitOfWork uof)
    {
        _tagTipoService = tagTipoService;
        _uof = uof;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, TagTipo dto)
    {
        try
        {
            var resultado = await _tagTipoService.Atualiza(id, dto);
            await _uof.CommitAsync();
            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpPost]
    public async Task<IActionResult> Post(TagTipo dto)
    {
        try
        {
            var resultado = await _tagTipoService.Criar(dto);
            await _uof.CommitAsync();
            return StatusCode(201, resultado);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _tagTipoService.DeletarPorId(id);
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
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit, [FromQuery] int offset)
    {
        try
        {
            var resultadoPaginado = await _tagTipoService.ObterTodos(limit, offset);
            return Ok(resultadoPaginado);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var resultado = await _tagTipoService.ObterPorId(id);
            if (resultado == null) return NotFound();
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }
}