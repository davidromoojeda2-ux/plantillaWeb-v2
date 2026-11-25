using Aplication.DTOs;

namespace FinanceHorizon.Models.Category;

/// <summary>
/// Representa los datos requeridos para renderizar una tarjeta de artículo estándar
/// en el listado de categoría.
/// </summary>
public sealed record ArticleCardViewModel(
    ArticleResponseDto Article,
    string? ImageUrl,
    string CategoryName,
    string? ImageSrcSet = null)
{
    public static implicit operator (ArticleResponseDto Article, string? ImageUrl, string? CategoryName)(ArticleCardViewModel vm)
        => (vm.Article, vm.ImageUrl, vm.CategoryName);
}

/// <summary>
/// ViewModel fuertemente tipado para la vista de listado por categoría.
/// Soporta tanto categorías principales como subcategorías, proveyendo acceso
/// a la categoría padre, subcategoría actual y lista de subcategorías asociadas.
/// </summary>
public sealed record CategoryPageViewModel(
    CategoryResponseDto Category,
    int CurrentPage,
    int TotalPages,
    long TotalCount,
    IReadOnlyList<ArticleCardViewModel> Articles,
    CategoryResponseDto? ParentCategory = null,
    IReadOnlyList<CategoryResponseDto>? Subcategories = null,
    FinanceHorizon.Models.Home.FeatureCardViewModel? HeroArticle = null,
    string? HeroArticleId = null)
{
    /// <summary>
    /// Artículos de la rejilla: todos los de la página salvo el que ya se destaca en la cabecera,
    /// para no repetirlo. <see cref="Articles"/> conserva la lista completa (datos estructurados).
    /// </summary>
    public IReadOnlyList<ArticleCardViewModel> GridArticles =>
        string.IsNullOrEmpty(HeroArticleId)
            ? Articles
            : Articles.Where(a => a.Article.Id != HeroArticleId).ToList();

    /// <summary>Solo se muestra la franja de subcategorías si hay alguna.</summary>
    public bool HasSubcategories => Subcategories is { Count: > 0 };
}
