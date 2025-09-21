using ThorAPI.DTOs;
using ThorAPI.Services;

namespace ThorAPI.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuariosController(UsuarioService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UsuarioCreateDTO dto)
    {
        try
        {
            var id = await _service.CriarUsuarioAsync(dto);
            return CreatedAtAction(nameof(GetByEmail), new { email = dto.Email }, dto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var usuario = await _service._repository.ObterPorEmailAsync(email);
        if (usuario == null) return NotFound();
        return Ok(new { usuario.Id, usuario.Nome, usuario.Email });
    }
}