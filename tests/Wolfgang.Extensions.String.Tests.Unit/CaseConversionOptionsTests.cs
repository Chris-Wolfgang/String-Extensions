using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

/// <summary>
/// The overloads of the four case conversions that take a <see cref="CaseConversionOptions"/>.
/// </summary>
public class CaseConversionOptionsTests
{
    // Inputs that between them use every rule of the built-in splitter. The two built at run time
    // hold a surrogate without its partner, which cannot be written in a string literal safely.
    private static readonly string[] Corpus =
    [
        "",
        "a",
        "A sample string, for processing!",
        "  leading and trailing  ",
        "UserLoginCount",
        "userLoginCount",
        "SKUCode ZoneSKU GS1Barcode Line2Qty",
        "user_login-count",
        "don't stop, O'Brien",
        "it\u2019s (here) [now]",
        "tab\tand\nnew line",
        "no\u00A0break\u2003em\u2028line",
        "caf\u00E9 cr\u00E8me cafe\u0301Bar",
        "\uD801\uDC00\uD801\uDC28 word",
        "emoji \uD83D\uDE00 face",
        "9lives 100% done",
        "!?.",
        "ab" + new string('\uD801', 1) + "cd",
        new string('\uDC00', 1) + "ab",
    ];



    [Fact]
    public void Constructor_when_called_sets_the_rules_the_conversions_use_without_options()
    {
        var options = new CaseConversionOptions();

        Assert.Equal(PunctuationHandling.Drop, options.Punctuation);
        Assert.Equal(WordBoundaries.All, options.Boundaries);
        Assert.Equal(AcronymHandling.AsWord, options.Acronyms);
        Assert.Null(options.WordSplitter);
    }



    [Fact]
    public void All_when_read_is_every_boundary_together()
    {
        Assert.Equal
        (
            WordBoundaries.Separators | WordBoundaries.Delimiters | WordBoundaries.CaseChanges | WordBoundaries.ControlCharacters,
            WordBoundaries.All
        );
        Assert.Equal(15, (int)WordBoundaries.All);
        Assert.Equal(0, (int)WordBoundaries.None);
    }



    [Fact]
    public void ToCamelCase_when_passed_null_string_and_options_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.ToCamelCase(new CaseConversionOptions()));
    }



    [Fact]
    public void ToCamelCase_when_passed_null_string_and_null_options_throws_ArgumentNullException_for_options()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ToCamelCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToKebabCase_when_passed_null_string_and_options_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.ToKebabCase(new CaseConversionOptions()));
    }



    [Fact]
    public void ToKebabCase_when_passed_null_string_and_null_options_throws_ArgumentNullException_for_options()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ToKebabCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToPascalCase_when_passed_null_string_and_options_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.ToPascalCase(new CaseConversionOptions()));
    }



    [Fact]
    public void ToPascalCase_when_passed_null_string_and_null_options_throws_ArgumentNullException_for_options()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ToPascalCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToSnakeCase_when_passed_null_string_and_options_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.ToSnakeCase(new CaseConversionOptions()));
    }



    [Fact]
    public void ToSnakeCase_when_passed_null_string_and_null_options_throws_ArgumentNullException_for_options()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ToSnakeCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToCamelCase_when_passed_null_options_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => "a b".ToCamelCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToKebabCase_when_passed_null_options_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => "a b".ToKebabCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToPascalCase_when_passed_null_options_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => "a b".ToPascalCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void ToSnakeCase_when_passed_null_options_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => "a b".ToSnakeCase(null!));
        Assert.Equal("options", exception.ParamName);
    }



    // The no-argument methods walk the string once; the overloads build a word list first. These
    // four tests hold the two ways of applying the default rules to the same result.
    [Fact]
    public void ToCamelCase_when_passed_new_options_returns_what_the_method_without_options_returns()
    {
        Assert.All(Corpus, input => Assert.Equal(input.ToCamelCase(), input.ToCamelCase(new CaseConversionOptions())));
    }



    [Fact]
    public void ToKebabCase_when_passed_new_options_returns_what_the_method_without_options_returns()
    {
        Assert.All(Corpus, input => Assert.Equal(input.ToKebabCase(), input.ToKebabCase(new CaseConversionOptions())));
    }



    [Fact]
    public void ToPascalCase_when_passed_new_options_returns_what_the_method_without_options_returns()
    {
        Assert.All(Corpus, input => Assert.Equal(input.ToPascalCase(), input.ToPascalCase(new CaseConversionOptions())));
    }



    [Fact]
    public void ToSnakeCase_when_passed_new_options_returns_what_the_method_without_options_returns()
    {
        Assert.All(Corpus, input => Assert.Equal(input.ToSnakeCase(), input.ToSnakeCase(new CaseConversionOptions())));
    }



    [Theory]
    [InlineData("hello, world!", "hello,_world!")]
    [InlineData("don't stop", "don't_stop")]
    [InlineData("user_login-count", "user_login_count")]
    [InlineData("tab\tand\nnewline", "tab_and_newline")]
    [InlineData("UserLogin now", "user_login_now")]
    [InlineData("emoji \uD83D\uDE00 face", "emoji_\uD83D\uDE00_face")]
    [InlineData("a.b", "a.b")]
    [InlineData("!!! go", "!!!_go")]
    [InlineData("go!!!", "go!!!")]
    public void ToSnakeCase_when_punctuation_is_kept_leaves_it_in_the_word_it_touches(string input, string expectedResult)
    {
        var options = new CaseConversionOptions { Punctuation = PunctuationHandling.Keep };

        Assert.Equal(expectedResult, input.ToSnakeCase(options));
    }



    [Theory]
    [InlineData("hello, world!", "Hello,World!")]
    [InlineData("(hello) world", "(Hello)World")]
    [InlineData("say \"hello world\"", "Say\"HelloWorld\"")]
    [InlineData("!!! go", "!!!Go")]
    [InlineData("9lives cat", "9livesCat")]
    public void ToPascalCase_when_punctuation_is_kept_capitalizes_the_first_letter_or_digit_of_each_word(string input, string expectedResult)
    {
        var options = new CaseConversionOptions { Punctuation = PunctuationHandling.Keep };

        Assert.Equal(expectedResult, input.ToPascalCase(options));
    }



    [Theory]
    [InlineData("(hello) world", "(hello)World")]
    [InlineData("HELLO, (world)", "hello,(World)")]
    public void ToCamelCase_when_punctuation_is_kept_lower_cases_the_first_word_and_capitalizes_the_rest(string input, string expectedResult)
    {
        var options = new CaseConversionOptions { Punctuation = PunctuationHandling.Keep };

        Assert.Equal(expectedResult, input.ToCamelCase(options));
    }



    [Theory]
    [InlineData(WordBoundaries.None, "User_Login-Count now\tHere", "user_login-count now\there")]
    [InlineData(WordBoundaries.Separators, "User_Login-Count now\tHere", "user_login-count|now\there")]
    [InlineData(WordBoundaries.Delimiters, "User_Login-Count now\tHere", "user|login|count now\there")]
    [InlineData(WordBoundaries.ControlCharacters, "User_Login-Count now\tHere", "user_login-count now|here")]
    [InlineData(WordBoundaries.CaseChanges, "UserLogin_Count now", "user|login_count now")]
    [InlineData(WordBoundaries.Separators | WordBoundaries.Delimiters, "User_LoginCount now\tHere", "user|logincount|now\there")]
    [InlineData(WordBoundaries.All, "User_LoginCount now\tHere", "user|login|count|now|here")]
    public void ToSnakeCase_when_punctuation_is_kept_divides_words_only_at_the_boundaries_named(WordBoundaries boundaries, string input, string expectedWords)
    {
        var options = new CaseConversionOptions { Punctuation = PunctuationHandling.Keep, Boundaries = boundaries };

        // The expected words are written with a bar between them so that a delimiter kept inside a
        // word can be told from one the conversion added.
        Assert.Equal(Bars(expectedWords), input.SplitWords(options).Select(word => word.ToLowerInvariant()));
        Assert.Equal(string.Join("_", Bars(expectedWords)), input.ToSnakeCase(options));
    }



    [Theory]
    [InlineData(WordBoundaries.None, "UserLogin_count now", "userlogin_count_now")]
    [InlineData(WordBoundaries.Separators, "UserLogin_count now", "userlogin_count_now")]
    [InlineData(WordBoundaries.CaseChanges, "UserLogin_count now", "user_login_count_now")]
    [InlineData(WordBoundaries.All & ~WordBoundaries.CaseChanges, "iPhone case", "iphone_case")]
    public void ToSnakeCase_when_punctuation_is_dropped_ends_a_word_at_every_other_character_whatever_the_boundaries(WordBoundaries boundaries, string input, string expectedResult)
    {
        var options = new CaseConversionOptions { Boundaries = boundaries };

        Assert.Equal(expectedResult, input.ToSnakeCase(options));
    }



    [Theory]
    [InlineData("SKUCode", "SKUCode")]
    [InlineData("ZoneSKU", "ZoneSKU")]
    [InlineData("GS1Barcode", "GS1Barcode")]
    [InlineData("a big ID", "ABigID")]
    [InlineData("A STRING", "ASTRING")]
    [InlineData("123 go", "123Go")]
    [InlineData("user login", "UserLogin")]
    [InlineData("Id", "Id")]
    public void ToPascalCase_when_acronyms_are_preserved_keeps_a_word_with_no_lower_case_letter_as_it_is(string input, string expectedResult)
    {
        var options = new CaseConversionOptions { Acronyms = AcronymHandling.Preserve };

        Assert.Equal(expectedResult, input.ToPascalCase(options));
    }



    [Theory]
    [InlineData("SKUCode", "skuCode")]
    [InlineData("ZoneSKU", "zoneSKU")]
    [InlineData("user ID card", "userIDCard")]
    public void ToCamelCase_when_acronyms_are_preserved_still_lower_cases_the_first_word(string input, string expectedResult)
    {
        var options = new CaseConversionOptions { Acronyms = AcronymHandling.Preserve };

        Assert.Equal(expectedResult, input.ToCamelCase(options));
    }



    [Theory]
    [InlineData("iPhone case", "IphoneCase")]
    [InlineData("McDonald farm", "McdonaldFarm")]
    public void ToPascalCase_when_acronyms_are_preserved_still_cases_a_word_that_has_a_lower_case_letter(string input, string expectedResult)
    {
        var options = new CaseConversionOptions
        {
            Acronyms = AcronymHandling.Preserve,
            Boundaries = WordBoundaries.All & ~WordBoundaries.CaseChanges,
        };

        Assert.Equal(expectedResult, input.ToPascalCase(options));
    }



    [Fact]
    public void ToSnakeCase_when_acronyms_are_preserved_lower_cases_them_all_the_same()
    {
        var options = new CaseConversionOptions { Acronyms = AcronymHandling.Preserve };

        Assert.Equal("sku_code", "SKUCode".ToSnakeCase(options));
        Assert.Equal("sku-code", "SKUCode".ToKebabCase(options));
    }



    [Fact]
    public void ToPascalCase_when_acronyms_are_treated_as_words_cases_them_like_any_other_word()
    {
        var options = new CaseConversionOptions { Acronyms = AcronymHandling.AsWord };

        Assert.Equal("SkuCode", "SKUCode".ToPascalCase(options));
    }



    [Fact]
    public void ToSnakeCase_when_a_word_splitter_is_set_joins_the_words_it_returns_and_nothing_else()
    {
        var options = new CaseConversionOptions { WordSplitter = text => text.Split('|') };

        Assert.Equal("alpha_beta_gamma delta!", "Alpha|BETA|gamma Delta!".ToSnakeCase(options));
    }



    [Fact]
    public void ToKebabCase_when_a_word_splitter_is_set_joins_the_words_it_returns_and_nothing_else()
    {
        var options = new CaseConversionOptions { WordSplitter = text => text.Split('|') };

        Assert.Equal("alpha-beta-gamma delta!", "Alpha|BETA|gamma Delta!".ToKebabCase(options));
    }



    [Fact]
    public void ToPascalCase_when_a_word_splitter_is_set_capitalizes_the_words_it_returns()
    {
        var options = new CaseConversionOptions { WordSplitter = text => text.Split('|') };

        Assert.Equal("AlphaBetaGamma delta!", "alpha|BETA|gamma Delta!".ToPascalCase(options));
    }



    [Fact]
    public void ToCamelCase_when_a_word_splitter_is_set_lower_cases_the_first_word_it_returns()
    {
        var options = new CaseConversionOptions { WordSplitter = text => text.Split('|') };

        Assert.Equal("alphaBetaGamma delta!", "ALPHA|beta|gamma Delta!".ToCamelCase(options));
    }



    [Fact]
    public void ToSnakeCase_when_a_word_splitter_is_set_passes_it_the_string_unchanged_and_ignores_the_other_rules()
    {
        string? received = null;
        var options = new CaseConversionOptions
        {
            Punctuation = PunctuationHandling.Keep,
            Boundaries = WordBoundaries.None,
            WordSplitter = text =>
            {
                received = text;

                return ["One", "Two"];
            },
        };

        var result = " Don't_Touch-This ".ToSnakeCase(options);

        Assert.Equal(" Don't_Touch-This ", received);
        Assert.Equal("one_two", result);
    }



    [Fact]
    public void ToSnakeCase_when_the_word_splitter_returns_null_and_empty_words_skips_them()
    {
        var options = new CaseConversionOptions { WordSplitter = _ => ["", "a", null!, "", "b", ""] };

        Assert.Equal("a_b", "anything".ToSnakeCase(options));
        Assert.Equal("aB", "anything".ToCamelCase(options));
    }



    [Fact]
    public void ToSnakeCase_when_the_word_splitter_returns_no_words_returns_empty_string()
    {
        var options = new CaseConversionOptions { WordSplitter = _ => [] };

        Assert.Equal(string.Empty, "anything".ToSnakeCase(options));
    }



    [Fact]
    public void ToSnakeCase_when_the_word_splitter_returns_null_throws_InvalidOperationException()
    {
        var options = new CaseConversionOptions { WordSplitter = _ => null! };

        var exception = Assert.Throws<InvalidOperationException>(() => "anything".ToSnakeCase(options));
        Assert.Contains("word splitter", exception.Message, StringComparison.Ordinal);
    }



    [Fact]
    public void ToSnakeCase_when_the_word_splitter_returns_a_lazy_sequence_enumerates_it_once()
    {
        var enumerations = 0;
        var options = new CaseConversionOptions { WordSplitter = text => Counting(text.Split(' '), () => enumerations++) };

        var result = "one two three".ToSnakeCase(options);

        Assert.Equal("one_two_three", result);
        Assert.Equal(1, enumerations);
    }



    [Fact]
    public void ToPascalCase_when_a_word_splitter_is_set_and_acronyms_are_preserved_keeps_the_acronyms_it_returns()
    {
        var options = new CaseConversionOptions
        {
            Acronyms = AcronymHandling.Preserve,
            WordSplitter = text => text.Split(' '),
        };

        Assert.Equal("SKUCodeX", "SKU code x".ToPascalCase(options));
    }



    [Theory]
    [InlineData(PunctuationHandling.Drop)]
    [InlineData(PunctuationHandling.Keep)]
    public void Punctuation_when_set_to_a_member_returns_it(PunctuationHandling value)
    {
        var options = new CaseConversionOptions { Punctuation = value };

        Assert.Equal(value, options.Punctuation);
    }



    [Fact]
    public void Punctuation_when_set_to_a_value_that_is_not_a_member_throws_ArgumentOutOfRangeException()
    {
        var options = new CaseConversionOptions();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => options.Punctuation = (PunctuationHandling)2);

        Assert.Equal("value", exception.ParamName);
        Assert.Equal((PunctuationHandling)2, exception.ActualValue);
        Assert.StartsWith("The value is not a member of PunctuationHandling.", exception.Message, StringComparison.Ordinal);
        Assert.Equal(PunctuationHandling.Drop, options.Punctuation);
    }



    [Theory]
    [InlineData(AcronymHandling.AsWord)]
    [InlineData(AcronymHandling.Preserve)]
    public void Acronyms_when_set_to_a_member_returns_it(AcronymHandling value)
    {
        var options = new CaseConversionOptions { Acronyms = value };

        Assert.Equal(value, options.Acronyms);
    }



    [Fact]
    public void Acronyms_when_set_to_a_value_that_is_not_a_member_throws_ArgumentOutOfRangeException()
    {
        var options = new CaseConversionOptions();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => options.Acronyms = (AcronymHandling)2);

        Assert.Equal("value", exception.ParamName);
        Assert.Equal((AcronymHandling)2, exception.ActualValue);
        Assert.StartsWith("The value is not a member of AcronymHandling.", exception.Message, StringComparison.Ordinal);
        Assert.Equal(AcronymHandling.AsWord, options.Acronyms);
    }



    [Theory]
    [InlineData(WordBoundaries.None)]
    [InlineData(WordBoundaries.Separators)]
    [InlineData(WordBoundaries.Delimiters)]
    [InlineData(WordBoundaries.CaseChanges)]
    [InlineData(WordBoundaries.ControlCharacters)]
    [InlineData(WordBoundaries.All)]
    public void Boundaries_when_set_to_members_returns_them(WordBoundaries value)
    {
        var options = new CaseConversionOptions { Boundaries = value };

        Assert.Equal(value, options.Boundaries);
    }



    [Theory]
    [InlineData((WordBoundaries)16)]
    [InlineData(WordBoundaries.All | (WordBoundaries)16)]
    [InlineData((WordBoundaries)(-1))]
    public void Boundaries_when_set_to_a_value_with_a_bit_that_is_not_a_member_throws_ArgumentOutOfRangeException(WordBoundaries value)
    {
        var options = new CaseConversionOptions { Boundaries = WordBoundaries.Separators };

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => options.Boundaries = value);

        Assert.Equal("value", exception.ParamName);
        Assert.Equal(value, exception.ActualValue);
        Assert.StartsWith("The value has a bit that is not a member of WordBoundaries.", exception.Message, StringComparison.Ordinal);
        Assert.Equal(WordBoundaries.Separators, options.Boundaries);
    }



    // The expected words of a row, which are written with a bar between them.
    private static string[] Bars(string? words)
    {
        return (words ?? string.Empty).Split('|');
    }



    private static IEnumerable<string> Counting(IEnumerable<string> words, Action started)
    {
        started();

        foreach (var word in words)
        {
            yield return word;
        }
    }
}
