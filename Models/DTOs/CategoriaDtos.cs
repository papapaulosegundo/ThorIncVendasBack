namespace ThorAPI.Models.DTOs;

public class CategoriaCreateDto {
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativa { get; set; } = true;
}

public class CategoriaUpdateDto {
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativa { get; set; } = true;
}
