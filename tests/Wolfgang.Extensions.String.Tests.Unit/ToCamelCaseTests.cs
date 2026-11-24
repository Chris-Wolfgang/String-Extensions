using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToCamelCaseTests
{


    [Fact]
    public void ToCamelCase_when_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.ToCamelCase());
        Assert.Equal("s", exception.ParamName);
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
    [InlineData("a string in all lower.", "aStringInAllLower.")]
    [InlineData("A STRING IN ALL UPPER!", "aStringInAllUpper!")]
    [InlineData("a-string-with-no-spaces.", "a-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\ncapitalization", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\ncapitalization")]
    [InlineData(" A STRING starting with a leading space%", "aStringStartingWithALeadingSpace%")]
    [InlineData("  consecutive  spaces  ", "consecutiveSpaces")]
    public void ToCamelCase_when_passed_valid_string_converts_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }



    [Theory]
    [InlineData("UserLoginCount", "userLoginCount")]
    [InlineData("userLoginCount", "userLoginCount")]
    [InlineData("Id", "id")]
    [InlineData("ID", "id")]
    [InlineData("ZoneGroupId", "zoneGroupId")]
    [InlineData("SKUCode", "skuCode")]
    [InlineData("ZoneSKU", "zoneSku")]
    [InlineData("GS1Barcode", "gs1Barcode")]
    [InlineData("Line2Qty", "line2Qty")]
    [InlineData("ResultSha256", "resultSha256")]
    [InlineData("IANAZone", "ianaZone")]
    [InlineData("oid", "oid")]
    [InlineData("user LoginCount", "userLoginCount")]
    [InlineData("User loginCount", "userLoginCount")]
    [InlineData(" UserLogin  count ", "userLoginCount")]
    [InlineData("user_loginCount", "user_loginCount")]
    public void ToCamelCase_when_passed_an_identifier_keeps_its_words_apart(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }
}
