using Microsoft.AspNetCore.Mvc;
using ThorAPI.Extensions;
using ThorAPI.Models;
using ThorAPI.Services;
using ThorAPI.Models.DTOs;
using ThorAPI.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoService _produtoService;
    private readonly IUnitOfWork _uof;

    public ProdutoController(ProdutoService service, IUnitOfWork uof)
    {
        _produtoService = service;
        _uof = uof;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProdutoCreateDto body)
    {
        try
        {
            if (body is null) return BadRequest("Body vazio.");
            if (string.IsNullOrWhiteSpace(body.Nome))
                return BadRequest("Nome é obrigatório.");
            if (body.Preco <= 0)
                return BadRequest("Preco deve ser maior que 0 (em centavos).");

            var produto = body.ToProduto();

            var resultado = await _produtoService.Criar(produto);
            await _uof.CommitAsync();
            return StatusCode(201, resultado);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(400, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Um erro ocorreu: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Produto dto)
    {
        try
        {
            var resultado = await _produtoService.Atualizar(id, dto);
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
            await _produtoService.DeletarPorId(id);
            await _uof.CommitAsync();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(400, ex.Message);
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
            var resultado = await _produtoService.ObterPorId(id);
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

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit, [FromQuery] int offset, [FromQuery] string? nome = null)
    {
        try
        {
            var resultado = await _produtoService.ObterTodos(limit, offset, nome);
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