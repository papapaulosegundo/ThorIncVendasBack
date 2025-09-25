using System.Text;
using System.Globalization;
using ThorAPI.Models;
using ThorAPI.Repositories;

namespace ThorAPI.Services;

public class CategoriaService {
    private readonly CategoriaRepository _repo;
    public CategoriaService(CategoriaRepository repo) => _repo = repo;

    private static string Slugify(string s) {
        if (string.IsNullOrWhiteSpace(s)) return "";
        var nf = s.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(capacity: nf.Length);
        foreach (var ch in nf) {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark) {
                sb.Append(ch switch {
                    ' ' => '-',
                    _ when char.IsLetterOrDigit(ch) || ch == '-' => ch,
                    _ => '-'
                });
            }
        }
        return sb.ToString().Trim('-');
    }

    public async Task<Categoria> Criar(string nome, string? descricao, bool ativa) {
        var slug = Slugify(nome);
        var existente = await _repo.ObterPorSlugAsync(slug);
        if (existente != null) throw new InvalidOperationException("Nome/slug já em uso.");

        var c = new Categoria { Nome = nome, Descricao = descricao, Ativa = ativa, Slug = slug };
        c.Id = await _repo.InserirAsync(c);
        return (await _repo.ObterPorIdAsync(c.Id))!;
    }

    public async Task<Categoria> Atualizar(int id, string nome, string? descricao, bool ativa) {
        var slug = Slugify(nome);
        var atual = await _repo.ObterPorIdAsync(id) ?? throw new KeyNotFoundException("Categoria não encontrada.");
        var conflito = await _repo.ObterPorSlugAsync(slug);
        if (conflito != null && conflito.Id != id) throw new InvalidOperationException("Nome/slug já em uso.");

        atual.Nome = nome; atual.Slug = slug; atual.Descricao = descricao; atual.Ativa = ativa;
        await _repo.AtualizarAsync(id, atual);
        return (await _repo.ObterPorIdAsync(id))!;
    }

    public Task<Categoria?> ObterPorSlug(string slug) => _repo.ObterPorSlugAsync(slug);
    public Task<Categoria?> ObterPorId(int id) => _repo.ObterPorIdAsync(id);
    public Task<IEnumerable<Categoria>> Listar(int limit, int offset, string? nome=null) => _repo.ObterTodosAsync(limit, offset, nome);
    public Task Deletar(int id) => _repo.DeletarAsync(id);
}
