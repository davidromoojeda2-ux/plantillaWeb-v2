using Aplication.DTOs;
using Aplication.Mappers;
using Aplication.Services;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using FinanceHorizon.Models.Category;
using FinanceHorizon.Models.Home;

namespace FinanceHorizon.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IArticleService _articleService;
    private readonly ImageService _imageService;

    public CategoryController(
        ICategoryService categoryService,
        IArticleService articleService,
        ImageService imageService)
    {
        _categoryService = categoryService;
        _articleService = articleService;
        _imageService = imageService;
    }

    [HttpGet("categoria/{slug}")]
    public async Task<IActionResult> Categoria(string slug, int page = 1)
    {
        var category = await _categoryService.GetBySlugAsync(slug);
        if (category is null) return NotFound();

        var allCategories = await _categoryService.GetAllAsync();
        CategoryResponseDto? parentCategory = null;
        List<CategoryResponseDto> subcategories = [];

        if (!string.IsNullOrEmpty(category.ParentId))
        {
            // La entidad consultada por slug es una subcategoría
            parentCategory = allCategories.FirstOrDefault(c => c.Id == category.ParentId)
                             ?? await _categoryService.GetByIdAsync(category.ParentId);
            // Subcategorías hermanas del mismo padre
            subcategories = allCategories
                .Where(c => c.ParentId == category.ParentId && c.IsActive)
                .OrderBy(c => c.Order)
                .ToList();
        }
        else
        {
            // La entidad es una categoría principal (raíz)
            subcategories = allCategories
                .Where(c => c.ParentId == category.Id && c.IsActive)
                .OrderBy(c => c.Order)
                .ToList();
        }

        subcategories = subcategories.Where(c => c.IsActive).ToList();

        var (articles, total) = await _articleService.GetByCategoryAsync(category.Id, page, 9);
        var images = await _imageService.GetVariantsDictAsync(
            articles.Select(a => a.CoverImageId).Where(id => !string.IsNullOrEmpty(id))!);

        var cardItems = articles.Select(art =>
        {
            var artCover = art.CoverImageId != null && images.TryGetValue(art.CoverImageId, out var img) ? img : null;
            var artImgUrl = ImageMapper.GetCardUrl(art, artCover);
            var artImgSrcSet = ImageMapper.GetSrcSetForUrl(artCover, artImgUrl);

            var catName = parentCategory != null
                ? $"{parentCategory.Name} • {category.Name}"
                : category.Name;

            return new ArticleCardViewModel(art, artImgUrl, catName, artImgSrcSet);
        }).ToList();

        var totalPages = (int)Math.Ceiling((double)total / 9);

        // Las categorías no tienen imagen propia: la cabecera destaca el artículo más
        // reciente de la categoría que tenga imagen (imagen + texto, como en la home).
        var heroCard = cardItems.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.ImageUrl));
        var heroArticle = heroCard is null ? null : new FeatureCardViewModel(
            Title: heroCard.Article.Title,
            Text: heroCard.Article.Summary ?? string.Empty,
            Category: heroCard.CategoryName,
            Href: Url.Action("Articulo", "Home", new { slug = heroCard.Article.Slug }) ?? $"/articulos/{heroCard.Article.Slug}",
            Image: new CardImage(heroCard.ImageUrl, heroCard.Article.Title, 1600, 900, heroCard.ImageSrcSet),
            PublishedAt: heroCard.Article.PublishedAt,
            Author: heroCard.Article.Author,
            ReadingMinutes: heroCard.Article.ReadingTimeMinutes);

        var viewModel = new CategoryPageViewModel(
            Category: category,
            CurrentPage: page,
            TotalPages: totalPages,
            TotalCount: total,
            Articles: cardItems,
            ParentCategory: parentCategory,
            Subcategories: subcategories,
            HeroArticle: heroArticle,
            HeroArticleId: heroCard?.Article.Id);

        return View(viewModel);
    }
}
