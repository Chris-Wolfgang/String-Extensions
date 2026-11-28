using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToCamelCaseTests
{


    [Fact]
    public void ToCamelCase_when_passed_null_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.ToCamelCase());
    }



    [Fact]
    public void ToCamelCase_when_passed_empty_string_returns_empty_string()
    {
        Assert.Equal(string.Empty, string.Empty.ToCamelCase());
    }



    [Fact]
    public void ToCamelCase_when_passed_single_character_returns_lowercase_character()
    {
        Assert.Equal("a", "A".ToCamelCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "aStringInAllLower")]
    [InlineData("A STRING IN ALL UPPER!", "aStringInAllUpper")]
    [InlineData(" leading and trailing ", "leadingAndTrailing")]
    [InlineData("  consecutive  spaces  ", "consecutiveSpaces")]
    [InlineData("tab\tand\nnew line\r\nseparate", "tabAndNewLineSeparate")]
    [InlineData("no\u00A0break\u2003em\u2028line", "noBreakEmLine")]
    public void ToCamelCase_when_passed_text_joins_its_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("hello, world!", "helloWorld")]
    [InlineData("(hello) world", "helloWorld")]
    [InlineData("say \"hello world\"", "sayHelloWorld")]
    [InlineData("[draft] final report", "draftFinalReport")]
    [InlineData("$total amount", "totalAmount")]
    [InlineData("user@example.com", "userExampleCom")]
    [InlineData("a.b.c", "aBC")]
    [InlineData("100% done", "100Done")]
    [InlineData("9lives cat", "9livesCat")]
    [InlineData("emoji \uD83D\uDE00 face", "emojiFace")]
    [InlineData("one/two\\three", "oneTwoThree")]
    public void ToCamelCase_when_passed_punctuation_or_symbols_treats_them_as_word_boundaries_and_drops_them(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("don't stop", "dontStop")]
    [InlineData("it\u2019s here", "itsHere")]
    [InlineData("rock 'n' roll", "rockNRoll")]
    [InlineData("'quoted' text", "quotedText")]
    [InlineData("O'Brien", "oBrien")]
    public void ToCamelCase_when_passed_an_apostrophe_removes_it_without_ending_the_word(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("user_login_count", "userLoginCount")]
    [InlineData("user-login-count", "userLoginCount")]
    [InlineData("User-Login_Count", "userLoginCount")]
    [InlineData("__Leading", "leading")]
    [InlineData("Trailing_", "trailing")]
    [InlineData("--Leading", "leading")]
    [InlineData("Trailing-", "trailing")]
    [InlineData("a___b", "aB")]
    [InlineData("a - b", "aB")]
    [InlineData("user_LoginCount", "userLoginCount")]
    public void ToCamelCase_when_passed_underscores_or_dashes_treats_them_as_word_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("UserLoginCount", "userLoginCount")]
    [InlineData("userLoginCount", "userLoginCount")]
    [InlineData("Id", "id")]
    [InlineData("ID", "id")]
    [InlineData("oid", "oid")]
    [InlineData("ZoneGroupId", "zoneGroupId")]
    [InlineData("SKUCode", "skuCode")]
    [InlineData("ZoneSKU", "zoneSku")]
    [InlineData("GS1Barcode", "gs1Barcode")]
    [InlineData("Line2Qty", "line2Qty")]
    [InlineData("ResultSha256", "resultSha256")]
    [InlineData("IANAZone", "ianaZone")]
    [InlineData("user LoginCount", "userLoginCount")]
    public void ToCamelCase_when_passed_an_identifier_splits_it_at_its_case_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("caf\u00E9 cr\u00E8me", "caf\u00E9Cr\u00E8me")]
    [InlineData("\u00C9cole Nationale", "\u00E9coleNationale")]
    [InlineData("\u00C9coleNationale", "\u00E9coleNationale")]
    [InlineData("cafe\u0301 au lait", "cafe\u0301AuLait")]
    [InlineData("\u0394\u03BF\u03BA\u03B9\u03BC\u03AE \u039B\u03AD\u03BE\u03B7", "\u03B4\u03BF\u03BA\u03B9\u03BC\u03AE\u039B\u03AD\u03BE\u03B7")]
    [InlineData("\u65E5\u672C\u8A9E text", "\u65E5\u672C\u8A9EText")]
    [InlineData("\uD801\uDC00\uD801\uDC28 word", "\uD801\uDC00\uD801\uDC28Word")]
    public void ToCamelCase_when_passed_letters_of_other_alphabets_keeps_them_in_their_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("---")]
    [InlineData("___")]
    [InlineData("!?.")]
    [InlineData("'")]
    [InlineData("\t\r\n")]
    [InlineData("\uD83D\uDE00")]
    [InlineData("_ - _")]
    public void ToCamelCase_when_passed_no_letters_or_digits_returns_empty_string(string input)
    {
        Assert.Equal(string.Empty, input.ToCamelCase());
    }
}
