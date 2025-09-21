namespace ThorAPI.Models;

public class Produto {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public int Preco { get; set; }
    public int Quantidade { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public int? IdTagTipo { get; set; }
    public Tag[]? tags { get; set; } = [];
}
