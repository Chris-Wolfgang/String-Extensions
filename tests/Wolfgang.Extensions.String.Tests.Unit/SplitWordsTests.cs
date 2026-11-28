using System;
using System.Collections.Generic;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class SplitWordsTests
{


    [Fact]
    public void SplitWords_when_passed_null_returns_an_empty_sequence()
    {
        string? sut = null;

        Assert.Empty(sut.SplitWords());
    }



    [Fact]
    public void SplitWords_when_passed_null_string_and_options_returns_an_empty_sequence()
    {
        string? sut = null;

        Assert.Empty(sut.SplitWords(new CaseConversionOptions()));
    }



    [Fact]
    public void SplitWords_when_passed_null_string_and_null_options_throws_ArgumentNullException_for_options()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.SplitWords(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Fact]
    public void SplitWords_when_passed_null_options_throws_ArgumentNullException_before_it_is_enumerated()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => "a b".SplitWords(null!));
        Assert.Equal("options", exception.ParamName);
    }



    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("!?.")]
    [InlineData("'")]
    public void SplitWords_when_passed_no_letters_or_digits_returns_no_words(string input)
    {
        Assert.Empty(input.SplitWords());
    }



    [Theory]
    [InlineData("a", "a")]
    [InlineData("A sample string, for processing!", "A|sample|string|for|processing")]
    [InlineData("  leading and trailing  ", "leading|and|trailing")]
    [InlineData("UserLoginCount", "User|Login|Count")]
    [InlineData("SKUCode for user_login!", "SKU|Code|for|user|login")]
    [InlineData("Line2Qty GS1Barcode", "Line2|Qty|GS1|Barcode")]
    [InlineData("user-login_count", "user|login|count")]
    [InlineData("don't stop", "dont|stop")]
    [InlineData("it\u2019s O'Brien", "its|O|Brien")]
    [InlineData("tab\tand\nnew line", "tab|and|new|line")]
    [InlineData("caf\u00E9 cafe\u0301 au lait", "caf\u00E9|cafe\u0301|au|lait")]
    [InlineData("\uD801\uDC00\uD801\uDC28 word", "\uD801\uDC00\uD801\uDC28|word")]
    [InlineData("emoji \uD83D\uDE00 face", "emoji|face")]
    [InlineData("trailing word", "trailing|word")]
    public void SplitWords_when_passed_text_returns_its_words_in_order_with_their_casing(string input, string expectedWords)
    {
        Assert.Equal(Bars(expectedWords), input.SplitWords());
    }



    [Fact]
    public void SplitWords_when_passed_new_options_returns_what_the_method_without_options_returns()
    {
        const string input = "SKUCode for user_login, don't (stop)!";

        Assert.Equal(input.SplitWords(), input.SplitWords(new CaseConversionOptions()));
    }



    [Theory]
    [InlineData("Hello, big World!", "Hello,|big|World!")]
    [InlineData("don't stop", "don't|stop")]
    [InlineData("(a) [b]", "(a)|[b]")]
    [InlineData("x - y", "x|y")]
    [InlineData("end.", "end.")]
    [InlineData(". start", ".|start")]
    public void SplitWords_when_punctuation_is_kept_returns_it_inside_the_words(string input, string expectedWords)
    {
        var options = new CaseConversionOptions { Punctuation = PunctuationHandling.Keep };

        Assert.Equal(Bars(expectedWords), input.SplitWords(options));
    }



    [Fact]
    public void SplitWords_when_case_changes_are_not_boundaries_keeps_an_identifier_whole()
    {
        var options = new CaseConversionOptions { Boundaries = WordBoundaries.All & ~WordBoundaries.CaseChanges };

        Assert.Equal(["UserLoginCount", "SKUCode"], "UserLoginCount SKUCode".SplitWords(options));
    }



    [Fact]
    public void SplitWords_when_a_word_splitter_is_set_returns_its_words_without_the_null_and_empty_ones()
    {
        var options = new CaseConversionOptions { WordSplitter = _ => ["", "One", null!, "two words", ""] };

        Assert.Equal(["One", "two words"], "anything".SplitWords(options));
    }



    [Fact]
    public void SplitWords_when_the_word_splitter_returns_null_throws_InvalidOperationException_before_it_is_enumerated()
    {
        var options = new CaseConversionOptions { WordSplitter = _ => null! };

        Assert.Throws<InvalidOperationException>(() => "anything".SplitWords(options));
    }



    [Fact]
    public void SplitWords_when_not_enumerated_has_not_read_the_word_splitters_sequence()
    {
        var enumerations = 0;
        var options = new CaseConversionOptions { WordSplitter = text => Counting(text.Split(' '), () => enumerations++) };

        var words = "one two".SplitWords(options);

        Assert.Equal(0, enumerations);
        Assert.Equal(["one", "two"], words);
        Assert.Equal(1, enumerations);
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
