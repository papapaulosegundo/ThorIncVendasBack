namespace ThorAPI.Models;

public class Endereco
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public required string Rua { get; set; }
    public required string Numero { get; set; }
    public string? Complemento { get; set; }
    public required string Bairro { get; set; }
    public required string Cidade { get; set; }
    public required string Estado { get; set; }
    public required string CEP { get; set; }
    public bool IsPrincipal { get; set; } = false;
}