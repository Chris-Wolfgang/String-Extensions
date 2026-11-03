using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Snapshots;

/// <summary>
/// Snapshot tests: each test renders one method's output over <see cref="Corpus"/> as a text table and
/// verifies it against <c>Snapshots/StringExtensionsSnapshotTests.&lt;Test&gt;.verified.txt</c>. Inputs
/// and outputs are written with every non-printable, non-ASCII or separator character escaped as
/// <c>\uXXXX</c>, so a change in white space or casing of a single character is visible in the diff.
/// </summary>
/// <remarks>
/// To approve an intended change, run the tests, check the <c>*.received.txt</c> file and rename it to
/// <c>*.verified.txt</c> (received files are git-ignored).
/// </remarks>
public class StringExtensionsSnapshotTests
{
    // Covers: empty, single char, mixed case, upper case, leading/trailing/repeated spaces, tab and
    // newline (not separators), NO-BREAK SPACE and EM SPACE (Zs), LINE and PARAGRAPH SEPARATOR (Zl, Zp),
    // Turkish dotted/dotless i, ASCII words starting with i (tr-TR title-cases them to U+0130),
    // sharp s, digits, punctuation and a surrogate pair.
    private static readonly string[] Corpus =
    [
        "",
        "a",
        "Hello",
        "A sample string for processing!",
        "  leading and trailing  ",
        "MiXeD cAsE wOrDs",
        "UPPER CASE WORDS",
        "tab\tand\nnewline",
        "no\u00A0break\u2003em",
        "line\u2028para\u2029sep",
        "\u0130stanbul \u0131i",
        "stra\u00DFe ma\u00DF",
        "istanbul is in italy",
        "123 numbers 456",
        "punctuation, stays; here!",
        "emoji \U0001F600 face",
    ];



    [Fact]
    public Task Left_over_the_corpus()
    {
        return Verify(Table(s => new[] { 0, 1, 3, 100 }.Select(n => $"Left({n}) = {Show(s.Left(n))}")));
    }



    [Fact]
    public Task Right_over_the_corpus()
    {
        return Verify(Table(s => new[] { 0, 1, 3, 100 }.Select(n => $"Right({n}) = {Show(s.Right(n))}")));
    }



    [Fact]
    public Task PadCenter_over_the_corpus()
    {
        return Verify
        (
            Table
            (
                s => new[] { 0, 5, 12, 40 }
                    .SelectMany
                    (
                        w => new[]
                        {
                            $"PadCenter({w}) = {Show(s.PadCenter(w))}",
                            $"PadCenter({w}, '*') = {Show(s.PadCenter(w, '*'))}",
                        }
                    )
            )
        );
    }



    [Fact]
    public Task Case_conversions_over_the_corpus()
    {
        return Verify
        (
            Table
            (
                s => new[]
                {
                    $"ToCamelCase = {Show(s.ToCamelCase())}",
                    $"ToKebabCase = {Show(s.ToKebabCase())}",
                    $"ToPascalCase = {Show(s.ToPascalCase())}",
                    $"ToSnakeCase = {Show(s.ToSnakeCase())}",
                }
            )
        );
    }



    [Theory]
    [InlineData("en-US")]
    [InlineData("tr-TR")]
    [InlineData("")]
    public Task ToTitleCase_over_the_corpus(string culture)
    {
        ArgumentNullException.ThrowIfNull(culture);

        var original = CultureInfo.CurrentCulture;
        string table;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            table = Table(s => new[] { $"ToTitleCase = {Show(s.ToTitleCase())}" }, AsciiOnly);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }

        return Verify(table).UseParameters(culture.Length == 0 ? "invariant" : culture);
    }



    private static SettingsTask Verify(string table) =>
        Verifier
            .Verify(table)
            .UseDirectory("Snapshots");



    // ToTitleCase delegates to TextInfo, whose casing of non-ASCII letters comes from the platform's
    // globalization library (ICU or NLS) and can differ between operating systems and library versions.
    // Its snapshot therefore uses the ASCII inputs only; the culture difference that matters (the
    // Turkish dotted capital I for 'i' under tr-TR) is still visible there.
    private static bool AsciiOnly(string input) => input.All(c => c <= '~');



    private static string Table
    (
        Func<string, System.Collections.Generic.IEnumerable<string>> render,
        Func<string, bool>? include = null
    )
    {
        var builder = new StringBuilder();
        foreach (var input in Corpus.Where(include ?? (_ => true)))
        {
            builder.Append(Show(input)).Append('\n');
            foreach (var line in render(input))
            {
                builder.Append("    ").Append(line).Append('\n');
            }
        }

        return builder.ToString();
    }



    // Left / Right return string?, but never null for the non-null corpus; the "!" says so.
    private static string Show(string? value)
    {
        var builder = new StringBuilder("\"");
        foreach (var c in value!)
        {
            builder.Append
            (
                c is >= ' ' and <= '~' && c != '\\' && c != '"'
                    ? c.ToString()
                    : $"\\u{(int)c:X4}"
            );
        }

        return builder.Append('"').ToString();
    }
}
