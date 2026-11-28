using System;
using System.Linq;
using Wolfgang.Extensions.String;

// Native AOT smoke consumer. Calls every public method of StringExtensions at least once, including
// the argument-validation paths, so the trimmer keeps everything the library needs and a fault on any
// path fails the workflow's run step. Each check prints the failing case; the process exits with 1
// when any check failed.

var failures = 0;

// Left / Right: clamping, null tolerance, argument validation.
Check("Left", string.Equals("A sample string".Left(8), "A sample", StringComparison.Ordinal));
Check("Left clamps", string.Equals("Hello".Left(10), "Hello", StringComparison.Ordinal));
Check("Left null", ((string?)null).Left(3) is null);
Throws<ArgumentOutOfRangeException>("Left negative", () => "x".Left(-1));
Check("Right", string.Equals("A sample string".Right(6), "string", StringComparison.Ordinal));
Check("Right clamps", string.Equals("Hello".Right(10), "Hello", StringComparison.Ordinal));
Check("Right null", ((string?)null).Right(3) is null);
Throws<ArgumentOutOfRangeException>("Right negative", () => "x".Right(-1));

// PadCenter: both overloads, the unchanged path and argument validation.
Check("PadCenter", string.Equals("Hello".PadCenter(11), "   Hello   ", StringComparison.Ordinal));
Check("PadCenter uneven", string.Equals("Hello".PadCenter(8, '-'), "-Hello--", StringComparison.Ordinal));
Check("PadCenter narrower", string.Equals("Hello".PadCenter(3), "Hello", StringComparison.Ordinal));
Check("PadCenter null", string.Equals(((string?)null).PadCenter(4), "    ", StringComparison.Ordinal));
Throws<ArgumentOutOfRangeException>("PadCenter negative", () => "x".PadCenter(-1, '*'));

// Case conversions.
const string input = "  user LOGIN count  ";
Check("ToCamelCase", string.Equals(input.ToCamelCase(), "userLoginCount", StringComparison.Ordinal));
Check("ToKebabCase", string.Equals(input.ToKebabCase(), "user-login-count", StringComparison.Ordinal));
Check("ToPascalCase", string.Equals(input.ToPascalCase(), "UserLoginCount", StringComparison.Ordinal));
Check("ToSnakeCase", string.Equals(input.ToSnakeCase(), "user_login_count", StringComparison.Ordinal));
Check("ToTitleCase", string.Equals("user login count".ToTitleCase(), "User Login Count", StringComparison.Ordinal));
Check("ToCamelCase null", ((string?)null).ToCamelCase() is null);
Check("ToKebabCase null", ((string?)null).ToKebabCase() is null);
Check("ToPascalCase null", ((string?)null).ToPascalCase() is null);
Check("ToSnakeCase null", ((string?)null).ToSnakeCase() is null);
Check("ToTitleCase null", ((string?)null).ToTitleCase() is null);

// Case conversions with options, and SplitWords.
var keep = new CaseConversionOptions
{
    Punctuation = PunctuationHandling.Keep,
    Boundaries = WordBoundaries.Separators,
    Acronyms = AcronymHandling.Preserve,
};
var custom = new CaseConversionOptions { WordSplitter = text => text.Split('|') };
Check("ToCamelCase options", string.Equals("big ID, here".ToCamelCase(keep), "bigID,Here", StringComparison.Ordinal));
Check("ToKebabCase options", string.Equals("Hello, World!".ToKebabCase(keep), "hello,-world!", StringComparison.Ordinal));
Check("ToPascalCase options", string.Equals("(hello) SKU".ToPascalCase(keep), "(Hello)SKU", StringComparison.Ordinal));
Check("ToSnakeCase options", string.Equals("Hello, World!".ToSnakeCase(keep), "hello,_world!", StringComparison.Ordinal));
Check("ToSnakeCase splitter", string.Equals("One|Two Three".ToSnakeCase(custom), "one_two three", StringComparison.Ordinal));
Check("SplitWords", "SKUCode for user_login!".SplitWords().SequenceEqual(new[] { "SKU", "Code", "for", "user", "login" }, StringComparer.Ordinal));
Check("SplitWords options", "Hello, World!".SplitWords(keep).SequenceEqual(new[] { "Hello,", "World!" }, StringComparer.Ordinal));
Throws<ArgumentNullException>("ToCamelCase null options", () => "x".ToCamelCase(null!));
Throws<ArgumentNullException>("ToKebabCase null options", () => "x".ToKebabCase(null!));
Throws<ArgumentNullException>("ToPascalCase null options", () => "x".ToPascalCase(null!));
Throws<ArgumentNullException>("ToSnakeCase null options", () => "x".ToSnakeCase(null!));
Check("SplitWords null", !((string?)null).SplitWords().Any());
Throws<ArgumentNullException>("SplitWords null options", () => string.Concat("x".SplitWords(null!)));
Throws<InvalidOperationException>("ToSnakeCase splitter null", () => "x".ToSnakeCase(new CaseConversionOptions { WordSplitter = _ => null! }));

if (failures > 0)
{
    Console.Error.WriteLine($"AOT smoke: {failures} check(s) failed.");
    return 1;
}

Console.WriteLine("AOT smoke: every StringExtensions method ran correctly.");
return 0;



void Check(string label, bool ok)
{
    if (!ok)
    {
        Console.Error.WriteLine($"AOT smoke FAILED: {label}");
        failures++;
    }
}



void Throws<TException>(string label, Func<string?> action)
    where TException : Exception
{
    try
    {
        _ = action();
        Check(label + " (no exception)", false);
    }
    catch (TException)
    {
        // expected
    }
}
