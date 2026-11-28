using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToKebabCaseTests
{


    [Fact]
    public void ToKebabCase_when_passed_null_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.ToKebabCase());
    }



    [Fact]
    public void ToKebabCase_when_passed_empty_string_returns_empty_string()
    {
        Assert.Equal(string.Empty, string.Empty.ToKebabCase());
    }



    [Fact]
    public void ToKebabCase_when_passed_single_character_returns_lowercase_character()
    {
        Assert.Equal("a", "A".ToKebabCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "a-string-in-all-lower")]
    [InlineData("A STRING IN ALL UPPER!", "a-string-in-all-upper")]
    [InlineData(" leading and trailing ", "leading-and-trailing")]
    [InlineData("  consecutive  spaces  ", "consecutive-spaces")]
    [InlineData("tab\tand\nnew line\r\nseparate", "tab-and-new-line-separate")]
    [InlineData("no\u00A0break\u2003em\u2028line", "no-break-em-line")]
    public void ToKebabCase_when_passed_text_joins_its_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("hello, world!", "hello-world")]
    [InlineData("(hello) world", "hello-world")]
    [InlineData("say \"hello world\"", "say-hello-world")]
    [InlineData("[draft] final report", "draft-final-report")]
    [InlineData("$total amount", "total-amount")]
    [InlineData("user@example.com", "user-example-com")]
    [InlineData("a.b.c", "a-b-c")]
    [InlineData("100% done", "100-done")]
    [InlineData("9lives cat", "9lives-cat")]
    [InlineData("emoji \uD83D\uDE00 face", "emoji-face")]
    [InlineData("one/two\\three", "one-two-three")]
    public void ToKebabCase_when_passed_punctuation_or_symbols_treats_them_as_word_boundaries_and_drops_them(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("don't stop", "dont-stop")]
    [InlineData("it\u2019s here", "its-here")]
    [InlineData("rock 'n' roll", "rock-n-roll")]
    [InlineData("'quoted' text", "quoted-text")]
    [InlineData("O'Brien", "o-brien")]
    public void ToKebabCase_when_passed_an_apostrophe_removes_it_without_ending_the_word(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("user_login_count", "user-login-count")]
    [InlineData("user-login-count", "user-login-count")]
    [InlineData("User-Login_Count", "user-login-count")]
    [InlineData("__Leading", "leading")]
    [InlineData("Trailing_", "trailing")]
    [InlineData("--Leading", "leading")]
    [InlineData("Trailing-", "trailing")]
    [InlineData("a___b", "a-b")]
    [InlineData("a - b", "a-b")]
    [InlineData("user_LoginCount", "user-login-count")]
    public void ToKebabCase_when_passed_underscores_or_dashes_treats_them_as_word_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("UserLoginCount", "user-login-count")]
    [InlineData("userLoginCount", "user-login-count")]
    [InlineData("Id", "id")]
    [InlineData("ID", "id")]
    [InlineData("oid", "oid")]
    [InlineData("ZoneGroupId", "zone-group-id")]
    [InlineData("SKUCode", "sku-code")]
    [InlineData("ZoneSKU", "zone-sku")]
    [InlineData("GS1Barcode", "gs1-barcode")]
    [InlineData("Line2Qty", "line2-qty")]
    [InlineData("ResultSha256", "result-sha256")]
    [InlineData("IANAZone", "iana-zone")]
    [InlineData("user LoginCount", "user-login-count")]
    public void ToKebabCase_when_passed_an_identifier_splits_it_at_its_case_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("caf\u00E9 cr\u00E8me", "caf\u00E9-cr\u00E8me")]
    [InlineData("\u00C9cole Nationale", "\u00E9cole-nationale")]
    [InlineData("\u00C9coleNationale", "\u00E9cole-nationale")]
    [InlineData("cafe\u0301 au lait", "cafe\u0301-au-lait")]
    [InlineData("\u0394\u03BF\u03BA\u03B9\u03BC\u03AE \u039B\u03AD\u03BE\u03B7", "\u03B4\u03BF\u03BA\u03B9\u03BC\u03AE-\u03BB\u03AD\u03BE\u03B7")]
    [InlineData("\u65E5\u672C\u8A9E text", "\u65E5\u672C\u8A9E-text")]
    [InlineData("\uD801\uDC00\uD801\uDC28 word", "\uD801\uDC00\uD801\uDC28-word")]
    public void ToKebabCase_when_passed_letters_of_other_alphabets_keeps_them_in_their_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
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
    public void ToKebabCase_when_passed_no_letters_or_digits_returns_empty_string(string input)
    {
        Assert.Equal(string.Empty, input.ToKebabCase());
    }
}
