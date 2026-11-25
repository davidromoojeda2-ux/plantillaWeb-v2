using FinanceHorizon.Seo;

namespace Seo.Tests;

public class CanonicalUrlTests
{
    private const string BaseUrl = "https://financehorizon.tech";

    private static IEnumerable<KeyValuePair<string, string?>> Query(params (string Key, string? Value)[] pairs)
        => pairs.Select(pair => new KeyValuePair<string, string?>(pair.Key, pair.Value));

    [Fact]
    public void Build_ReturnsRootUrl()
    {
        var url = CanonicalUrl.Build(BaseUrl, "/");

        Assert.Equal("https://financehorizon.tech/", url);
    }

    [Fact]
    public void Build_DropsTrackingParameters()
    {
        var url = CanonicalUrl.Build(
            BaseUrl,
            "/articulos/placas-solares",
            Query(("utm_source", "x"), ("utm_medium", "social"), ("ref", "home")));

        Assert.Equal("https://financehorizon.tech/articulos/placas-solares", url);
    }

    [Fact]
    public void Build_KeepsPagination()
    {
        var url = CanonicalUrl.Build(
            BaseUrl,
            "/categoria/energia",
            Query(("page", "3"), ("utm_source", "x")));

        Assert.Equal("https://financehorizon.tech/categoria/energia?page=3", url);
    }

    [Fact]
    public void Build_DropsFirstPageParameter()
    {
        var url = CanonicalUrl.Build(BaseUrl, "/publicaciones", Query(("page", "1")));

        Assert.Equal("https://financehorizon.tech/publicaciones", url);
    }

    [Fact]
    public void Build_IgnoresSearchQuery()
    {
        var url = CanonicalUrl.Build(
            BaseUrl,
            "/publicaciones",
            Query(("q", "baterías"), ("page", "2")));

        Assert.Equal("https://financehorizon.tech/publicaciones?page=2", url);
    }

    [Fact]
    public void Build_TrimsTrailingSlashFromBaseUrlAndNormalizesPath()
    {
        var url = CanonicalUrl.Build("https://financehorizon.tech/", "info/aviso-legal");

        Assert.Equal("https://financehorizon.tech/info/aviso-legal", url);
    }

    [Fact]
    public void Build_EscapesQueryValuesAndHonoursCustomKeys()
    {
        var url = CanonicalUrl.Build(
            BaseUrl,
            "/publicaciones",
            Query(("orden", "más vistos"), ("page", "2")),
            ["orden"]);

        Assert.Equal("https://financehorizon.tech/publicaciones?orden=m%C3%A1s%20vistos", url);
    }
}
