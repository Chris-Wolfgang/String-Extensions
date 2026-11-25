using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToPascalCaseTests
{


    [Fact]
    public void ToPascalCase_when_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.ToPascalCase());
        Assert.Equal("s", exception.ParamName);
    }



    [Fact]
    public void ToPascalCase_when_passed_empty_string_returns_empty_string()
    {
        Assert.Equal(string.Empty, string.Empty.ToPascalCase());
    }



    [Fact]
    public void ToPascalCase_when_passed_single_character_returns_uppercase_character()
    {
        Assert.Equal("A", "a".ToPascalCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "AStringInAllLower")]
    [InlineData("A STRING IN ALL UPPER!", "AStringInAllUpper")]
    [InlineData(" leading and trailing ", "LeadingAndTrailing")]
    [InlineData("  consecutive  spaces  ", "ConsecutiveSpaces")]
    [InlineData("tab\tand\nnew line\r\nseparate", "TabAndNewLineSeparate")]
    [InlineData("no\u00A0break\u2003em\u2028line", "NoBreakEmLine")]
    public void ToPascalCase_when_passed_text_joins_its_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }



    [Theory]
    [InlineData("hello, world!", "HelloWorld")]
    [InlineData("(hello) world", "HelloWorld")]
    [InlineData("say \"hello world\"", "SayHelloWorld")]
    [InlineData("[draft] final report", "DraftFinalReport")]
    [InlineData("$total amount", "TotalAmount")]
    [InlineData("user@example.com", "UserExampleCom")]
    [InlineData("a.b.c", "ABC")]
    [InlineData("100% done", "100Done")]
    [InlineData("9lives cat", "9livesCat")]
    [InlineData("emoji \uD83D\uDE00 face", "EmojiFace")]
    [InlineData("one/two\\three", "OneTwoThree")]
    public void ToPascalCase_when_passed_punctuation_or_symbols_treats_them_as_word_boundaries_and_drops_them(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }



    [Theory]
    [InlineData("don't stop", "DontStop")]
    [InlineData("it\u2019s here", "ItsHere")]
    [InlineData("rock 'n' roll", "RockNRoll")]
    [InlineData("'quoted' text", "QuotedText")]
    [InlineData("O'Brien", "OBrien")]
    public void ToPascalCase_when_passed_an_apostrophe_removes_it_without_ending_the_word(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }



    [Theory]
    [InlineData("user_login_count", "UserLoginCount")]
    [InlineData("user-login-count", "UserLoginCount")]
    [InlineData("User-Login_Count", "UserLoginCount")]
    [InlineData("__Leading", "Leading")]
    [InlineData("Trailing_", "Trailing")]
    [InlineData("--Leading", "Leading")]
    [InlineData("Trailing-", "Trailing")]
    [InlineData("a___b", "AB")]
    [InlineData("a - b", "AB")]
    [InlineData("user_LoginCount", "UserLoginCount")]
    public void ToPascalCase_when_passed_underscores_or_dashes_treats_them_as_word_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }



    [Theory]
    [InlineData("UserLoginCount", "UserLoginCount")]
    [InlineData("userLoginCount", "UserLoginCount")]
    [InlineData("Id", "Id")]
    [InlineData("ID", "Id")]
    [InlineData("oid", "Oid")]
    [InlineData("ZoneGroupId", "ZoneGroupId")]
    [InlineData("SKUCode", "SkuCode")]
    [InlineData("ZoneSKU", "ZoneSku")]
    [InlineData("GS1Barcode", "Gs1Barcode")]
    [InlineData("Line2Qty", "Line2Qty")]
    [InlineData("ResultSha256", "ResultSha256")]
    [InlineData("IANAZone", "IanaZone")]
    [InlineData("user LoginCount", "UserLoginCount")]
    public void ToPascalCase_when_passed_an_identifier_splits_it_at_its_case_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }



    [Theory]
    [InlineData("caf\u00E9 cr\u00E8me", "Caf\u00E9Cr\u00E8me")]
    [InlineData("\u00C9cole Nationale", "\u00C9coleNationale")]
    [InlineData("\u00C9coleNationale", "\u00C9coleNationale")]
    [InlineData("cafe\u0301 au lait", "Cafe\u0301AuLait")]
    [InlineData("\u0394\u03BF\u03BA\u03B9\u03BC\u03AE \u039B\u03AD\u03BE\u03B7", "\u0394\u03BF\u03BA\u03B9\u03BC\u03AE\u039B\u03AD\u03BE\u03B7")]
    [InlineData("\u65E5\u672C\u8A9E text", "\u65E5\u672C\u8A9EText")]
    [InlineData("\uD801\uDC00\uD801\uDC28 word", "\uD801\uDC00\uD801\uDC28Word")]
    public void ToPascalCase_when_passed_letters_of_other_alphabets_keeps_them_in_their_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
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
    public void ToPascalCase_when_passed_no_letters_or_digits_returns_empty_string(string input)
    {
        Assert.Equal(string.Empty, input.ToPascalCase());
    }
}
