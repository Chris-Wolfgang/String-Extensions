using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToSnakeCaseTests
{


    [Fact]
    public void ToSnakeCase_when_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.ToSnakeCase());
        Assert.Equal("s", exception.ParamName);
    }



    [Fact]
    public void ToSnakeCase_when_passed_empty_string_returns_empty_string()
    {
        Assert.Equal(string.Empty, string.Empty.ToSnakeCase());
    }



    [Fact]
    public void ToSnakeCase_when_passed_single_character_returns_lowercase_character()
    {
        Assert.Equal("a", "A".ToSnakeCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "a_string_in_all_lower")]
    [InlineData("A STRING IN ALL UPPER!", "a_string_in_all_upper")]
    [InlineData(" leading and trailing ", "leading_and_trailing")]
    [InlineData("  consecutive  spaces  ", "consecutive_spaces")]
    [InlineData("tab\tand\nnew line\r\nseparate", "tab_and_new_line_separate")]
    [InlineData("no\u00A0break\u2003em\u2028line", "no_break_em_line")]
    public void ToSnakeCase_when_passed_text_joins_its_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("hello, world!", "hello_world")]
    [InlineData("(hello) world", "hello_world")]
    [InlineData("say \"hello world\"", "say_hello_world")]
    [InlineData("[draft] final report", "draft_final_report")]
    [InlineData("$total amount", "total_amount")]
    [InlineData("user@example.com", "user_example_com")]
    [InlineData("a.b.c", "a_b_c")]
    [InlineData("100% done", "100_done")]
    [InlineData("9lives cat", "9lives_cat")]
    [InlineData("emoji \uD83D\uDE00 face", "emoji_face")]
    [InlineData("one/two\\three", "one_two_three")]
    public void ToSnakeCase_when_passed_punctuation_or_symbols_treats_them_as_word_boundaries_and_drops_them(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("don't stop", "dont_stop")]
    [InlineData("it\u2019s here", "its_here")]
    [InlineData("rock 'n' roll", "rock_n_roll")]
    [InlineData("'quoted' text", "quoted_text")]
    [InlineData("O'Brien", "o_brien")]
    public void ToSnakeCase_when_passed_an_apostrophe_removes_it_without_ending_the_word(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("user_login_count", "user_login_count")]
    [InlineData("user-login-count", "user_login_count")]
    [InlineData("User-Login_Count", "user_login_count")]
    [InlineData("__Leading", "leading")]
    [InlineData("Trailing_", "trailing")]
    [InlineData("--Leading", "leading")]
    [InlineData("Trailing-", "trailing")]
    [InlineData("a___b", "a_b")]
    [InlineData("a - b", "a_b")]
    [InlineData("user_LoginCount", "user_login_count")]
    public void ToSnakeCase_when_passed_underscores_or_dashes_treats_them_as_word_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("UserLoginCount", "user_login_count")]
    [InlineData("userLoginCount", "user_login_count")]
    [InlineData("Id", "id")]
    [InlineData("ID", "id")]
    [InlineData("oid", "oid")]
    [InlineData("ZoneGroupId", "zone_group_id")]
    [InlineData("SKUCode", "sku_code")]
    [InlineData("ZoneSKU", "zone_sku")]
    [InlineData("GS1Barcode", "gs1_barcode")]
    [InlineData("Line2Qty", "line2_qty")]
    [InlineData("ResultSha256", "result_sha256")]
    [InlineData("IANAZone", "iana_zone")]
    [InlineData("user LoginCount", "user_login_count")]
    public void ToSnakeCase_when_passed_an_identifier_splits_it_at_its_case_boundaries(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("caf\u00E9 cr\u00E8me", "caf\u00E9_cr\u00E8me")]
    [InlineData("\u00C9cole Nationale", "\u00E9cole_nationale")]
    [InlineData("\u00C9coleNationale", "\u00E9cole_nationale")]
    [InlineData("cafe\u0301 au lait", "cafe\u0301_au_lait")]
    [InlineData("\u0394\u03BF\u03BA\u03B9\u03BC\u03AE \u039B\u03AD\u03BE\u03B7", "\u03B4\u03BF\u03BA\u03B9\u03BC\u03AE_\u03BB\u03AD\u03BE\u03B7")]
    [InlineData("\u65E5\u672C\u8A9E text", "\u65E5\u672C\u8A9E_text")]
    [InlineData("\uD801\uDC00\uD801\uDC28 word", "\uD801\uDC00\uD801\uDC28_word")]
    public void ToSnakeCase_when_passed_letters_of_other_alphabets_keeps_them_in_their_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
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
    public void ToSnakeCase_when_passed_no_letters_or_digits_returns_empty_string(string input)
    {
        Assert.Equal(string.Empty, input.ToSnakeCase());
    }
}
