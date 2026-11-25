using Aplication.DTOs;
using FinanceHorizon.Models.Category;

namespace ContenidoBloques.Tests;

public class CategoryPageViewModelTests
{
    [Fact]
    public void La_rejilla_no_repite_el_articulo_destacado_en_la_cabecera()
    {
        var model = CreateModel(heroArticleId: "a2");

        Assert.Equal(new[] { "a1", "a3" }, model.GridArticles.Select(a => a.Article.Id));
    }

    [Fact]
    public void La_lista_completa_conserva_el_articulo_destacado()
    {
        var model = CreateModel(heroArticleId: "a2");

        Assert.Equal(3, model.Articles.Count);
    }

    [Fact]
    public void Sin_destacado_la_rejilla_muestra_todos_los_articulos()
    {
        var model = CreateModel(heroArticleId: null);

        Assert.Equal(3, model.GridArticles.Count);
    }

    private static CategoryPageViewModel CreateModel(string? heroArticleId)
    {
        var articles = new[] { "a1", "a2", "a3" }
            .Select(id => new ArticleCardViewModel(
                new ArticleResponseDto { Id = id, Title = id, Slug = id },
                ImageUrl: $"/img/{id}.webp",
                CategoryName: "Ahorro"))
            .ToList();

        return new CategoryPageViewModel(
            Category: new CategoryResponseDto { Id = "c1", Name = "Ahorro", Slug = "ahorro" },
            CurrentPage: 1,
            TotalPages: 1,
            TotalCount: articles.Count,
            Articles: articles,
            HeroArticleId: heroArticleId);
    }
}
