using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToKebabCaseTests
{


    [Fact]
    public void ToKebabCase_when_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.ToKebabCase());
        Assert.Equal("s", exception.ParamName);
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
    [InlineData("a string in all lower.", "a-string-in-all-lower.")]
    [InlineData("A STRING IN ALL UPPER!", "a-string-in-all-upper!")]
    [InlineData("a-string-with-no-spaces.", "a-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\nunderscores", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\nunderscores")]
    [InlineData(" A STRING starting with a leading and trailing space ", "a-string-starting-with-a-leading-and-trailing-space")]
    [InlineData("  consecutive  spaces  ", "consecutive-spaces")]
    public void ToKebabCase_when_passed_valid_string_converts_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("Id", "id")]
    [InlineData("UserLoginCount", "user-login-count")]
    [InlineData("userLoginCount", "user-login-count")]
    [InlineData("ZoneGroupId", "zone-group-id")]
    [InlineData("SKUCode", "sku-code")]
    [InlineData("ZoneSKU", "zone-sku")]
    [InlineData("GS1Barcode", "gs1-barcode")]
    [InlineData("Line2Qty", "line2-qty")]
    [InlineData("ResultSha256", "result-sha256")]
    [InlineData("Sha256Hash", "sha256-hash")]
    [InlineData("IANAZone", "iana-zone")]
    [InlineData("ID", "id")]
    [InlineData("oid", "oid")]
    public void ToKebabCase_when_passed_an_identifier_splits_it_into_its_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData("With Space", "with-space")]
    [InlineData("User LoginCount", "user-login-count")]
    [InlineData("A Sample", "a-sample")]
    [InlineData("already-delimited", "already-delimited")]
    [InlineData("Trailing ", "trailing")]
    [InlineData("Two  Spaces", "two-spaces")]
    [InlineData("Trailing-", "trailing")]
    [InlineData("--Leading", "leading")]
    [InlineData("a---b", "a-b")]
    [InlineData("With - Both", "with-both")]
    [InlineData("User-LoginCount", "user-login-count")]
    public void ToKebabCase_when_words_are_divided_more_than_once_puts_one_delimiter_between_them_and_none_at_the_ends(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }



    [Theory]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("-")]
    [InlineData("- -")]
    public void ToKebabCase_when_passed_only_dividers_returns_empty_string(string input)
    {
        Assert.Equal(string.Empty, input.ToKebabCase());
    }
}
