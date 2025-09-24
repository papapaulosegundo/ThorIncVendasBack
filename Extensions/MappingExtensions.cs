using ThorAPI.Models;
using ThorAPI.Models.DTOs;

namespace ThorAPI.Extensions;

public static class MappingExtensions
{
    public static Produto ToProduto(this ProdutoCreateDto dto)
    {
        if (dto == null)
        {
            return null;
        }
        
        return new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Imagem = dto.Imagem,
            Preco = dto.Preco,
            IdTagTipo = dto.IdTagTipo
        };
    }
}