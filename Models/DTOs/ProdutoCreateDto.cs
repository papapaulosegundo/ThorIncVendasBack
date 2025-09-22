namespace ThorAPI.Models.DTOs;

public sealed class ProdutoCreateDto
{
    public string Nome { get; set; } = default!;
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public int Preco { get; set; }            
    public int? IdTagTipo { get; set; }
}