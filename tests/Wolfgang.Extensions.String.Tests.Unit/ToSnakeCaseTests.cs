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
    [InlineData("a string in all lower.", "a_string_in_all_lower.")]
    [InlineData("A STRING IN ALL UPPER!", "a_string_in_all_upper!")]
    [InlineData("a-string-with-no-spaces.", "a-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\nunderscores", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\nunderscores")]
    [InlineData(" A STRING starting with a leading and trailing space ", "_a_string_starting_with_a_leading_and_trailing_space_")]
    [InlineData("  consecutive  spaces  ", "__consecutive__spaces__")]
    public void ToSnakeCase_when_passed_valid_string_converts_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("Id", "id")]
    [InlineData("UserLoginCount", "user_login_count")]
    [InlineData("userLoginCount", "user_login_count")]
    [InlineData("ZoneGroupId", "zone_group_id")]
    [InlineData("SKUCode", "sku_code")]
    [InlineData("ZoneSKU", "zone_sku")]
    [InlineData("GS1Barcode", "gs1_barcode")]
    [InlineData("Line2Qty", "line2_qty")]
    [InlineData("ResultSha256", "result_sha256")]
    [InlineData("Sha256Hash", "sha256_hash")]
    [InlineData("IANAZone", "iana_zone")]
    [InlineData("ID", "id")]
    [InlineData("oid", "oid")]
    public void ToSnakeCase_when_passed_an_identifier_splits_it_into_its_words(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }



    [Theory]
    [InlineData("With Space", "with_space")]
    [InlineData("User LoginCount", "user_login_count")]
    [InlineData("A Sample", "a_sample")]
    [InlineData("already_delimited", "already_delimited")]
    [InlineData("Trailing ", "trailing_")]
    [InlineData("Two  Spaces", "two__spaces")]
    public void ToSnakeCase_when_a_separator_already_divides_two_words_adds_no_second_delimiter(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }
}
