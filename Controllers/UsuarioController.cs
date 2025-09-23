using Microsoft.AspNetCore.Mvc;
using ThorAPI.Models;
using ThorAPI.Services;
using ThorAPI.Utils;

namespace ThorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase {
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service) {
        _service = service;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Criar([FromBody] Usuario usuario) {
        try {
            var criado = await _service.Criar(usuario);
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);

        } catch (InvalidOperationException ex) {
            return StatusCode(401, ex.Message);
        } catch (Exception ex) {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario login) {
        try {
            var usuario = await _service.Login(login.Email, login.Senha);
            return Ok(new { 
                Usuario = usuario, 
                Jwt = Jwt.GenerateToken(usuario) 
            });
        } catch (UnauthorizedAccessException ex) {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id) {
        try {
            var usuario = await _service.ObterPorId(id);
            return Ok(usuario);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario usuario) {
        try {
            var atualizado = await _service.Atualizar(id, usuario);
            return Ok(atualizado);
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id) {
        try {
            await _service.Deletar(id);
            return NoContent();
        } catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
    }
}
