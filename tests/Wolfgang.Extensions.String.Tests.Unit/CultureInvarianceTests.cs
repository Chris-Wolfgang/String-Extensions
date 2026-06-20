using System.Globalization;

namespace Wolfgang.Extensions.String.Tests.Unit;

/// <summary>
/// Verifies that the casing conversions which are meant to be culture-independent
/// (<see cref="StringExtensions.ToCamelCase"/>, <see cref="StringExtensions.ToPascalCase"/>,
/// <see cref="StringExtensions.ToKebabCase"/>, <see cref="StringExtensions.ToSnakeCase"/>)
/// produce identical output regardless of the ambient <see cref="CultureInfo.CurrentCulture"/>.
/// The canonical trap is Turkish (tr-TR), where the default casing of <c>i</c>/<c>I</c>
/// maps to the dotted/dotless variants (İ / ı); these methods use the invariant
/// <see cref="char.ToUpperInvariant(char)"/> / <see cref="char.ToLowerInvariant(char)"/>
/// to avoid it. <see cref="StringExtensions.ToTitleCase"/> is intentionally
/// culture-sensitive and is covered separately.
/// </summary>
public class CultureInvarianceTests
{
    private static T UnderCulture<T>(string culture, Func<T> action)
    {
        var original = CultureInfo.CurrentCulture;
        try
        {
            // Set synchronously and restore in finally with no intervening await,
            // so this only affects the current thread for the duration of the call —
            // safe under xUnit's parallel test execution.
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }


    [Theory]
    [InlineData("tr-TR")]
    [InlineData("de-DE")]
    [InlineData("en-US")]
    public void ToCamelCase_when_run_under_any_culture_uses_invariant_casing(string culture)
    {
        // 'I' must lower-case to ASCII 'i' (not the Turkish dotless 'ı').
        var result = UnderCulture(culture, () => "INFO Data".ToCamelCase());

        Assert.Equal("infoData", result);
    }


    [Theory]
    [InlineData("tr-TR")]
    [InlineData("de-DE")]
    [InlineData("en-US")]
    public void ToPascalCase_when_run_under_any_culture_uses_invariant_casing(string culture)
    {
        // 'i' must upper-case to ASCII 'I' (not the Turkish dotted 'İ').
        var result = UnderCulture(culture, () => "important info".ToPascalCase());

        Assert.Equal("ImportantInfo", result);
    }


    [Theory]
    [InlineData("tr-TR")]
    [InlineData("de-DE")]
    [InlineData("en-US")]
    public void ToKebabCase_when_run_under_any_culture_uses_invariant_casing(string culture)
    {
        var result = UnderCulture(culture, () => "INFO Data".ToKebabCase());

        Assert.Equal("info-data", result);
    }


    [Theory]
    [InlineData("tr-TR")]
    [InlineData("de-DE")]
    [InlineData("en-US")]
    public void ToSnakeCase_when_run_under_any_culture_uses_invariant_casing(string culture)
    {
        var result = UnderCulture(culture, () => "INFO Data".ToSnakeCase());

        Assert.Equal("info_data", result);
    }


    [Fact]
    public void ToTitleCase_when_run_under_invariant_culture_produces_deterministic_result()
    {
        // ToTitleCase intentionally honors CurrentCulture (it delegates to
        // TextInfo.ToTitleCase). Pinning the culture makes the result
        // deterministic regardless of the host machine's locale.
        var result = UnderCulture(CultureInfo.InvariantCulture.Name, () => "a sample string".ToTitleCase());

        Assert.Equal("A Sample String", result);
    }
}
