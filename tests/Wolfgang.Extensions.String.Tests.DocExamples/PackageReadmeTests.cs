using System.Reflection;
using System.Text.RegularExpressions;

namespace Wolfgang.Extensions.String.Tests.DocExamples;

/// <summary>
/// Keeps <c>src/Wolfgang.Extensions.String/PACKAGE.md</c>, the readme packed into the NuGet package,
/// usable on nuget.org and in step with the public API.
/// </summary>
/// <remarks>
/// nuget.org renders a relative link as plain text and shows images only from an allowed list of hosts,
/// so every link must be absolute and every image must come from <c>img.shields.io</c>.
/// </remarks>
public sealed class PackageReadmeTests
{
    private static readonly Regex LinkTarget = new
    (
        @"\]\(([^)\s]+)\)",
        RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1)
    );

    private static readonly Regex ImageTarget = new
    (
        @"!\[[^\]]*\]\(([^)\s]+)\)",
        RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1)
    );



    [Fact]
    public async Task PackageReadme_when_read_lists_every_public_StringExtensions_method()
    {
        var readme = await ReadPackageReadmeAsync();

        var missing = typeof(StringExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(m => m.Name)
            .Distinct(StringComparer.Ordinal)
            .Where(name => !readme.Contains("`" + name, StringComparison.Ordinal))
            .ToArray();

        Assert.Empty(missing);
    }



    [Fact]
    public async Task PackageReadme_when_read_has_only_absolute_https_links()
    {
        var relative = LinkTarget
            .Matches(await ReadPackageReadmeAsync())
            .Select(m => m.Groups[1].Value)
            .Where(target => !target.StartsWith("https://", StringComparison.Ordinal))
            .ToArray();

        Assert.Empty(relative);
    }



    [Fact]
    public async Task PackageReadme_when_read_has_images_only_from_img_shields_io()
    {
        var images = ImageTarget
            .Matches(await ReadPackageReadmeAsync())
            .Select(m => m.Groups[1].Value)
            .ToArray();

        Assert.NotEmpty(images);
        Assert.All
        (
            images,
            target => Assert.StartsWith("https://img.shields.io/", target, StringComparison.Ordinal)
        );
    }



    private static Task<string> ReadPackageReadmeAsync()
        => File.ReadAllTextAsync
        (
            Path.Combine(DocExampleSource.FindSrcDirectory(AppContext.BaseDirectory), "PACKAGE.md")
        );
}
