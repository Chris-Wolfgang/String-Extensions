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
}
