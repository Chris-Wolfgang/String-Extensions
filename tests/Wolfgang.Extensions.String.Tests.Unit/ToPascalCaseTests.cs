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
    [InlineData("a string in all lower.", "AStringInAllLower.")]
    [InlineData("A STRING IN ALL UPPER!", "AStringInAllUpper!")]
    [InlineData("a-string-with-no-spaces.", "A-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\ncapitalization", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\ncapitalization")]
    [InlineData(" A STRING starting with a leading space%", "AStringStartingWithALeadingSpace%")]
    [InlineData("  consecutive  spaces  ", "ConsecutiveSpaces")]
    public void ToPascalCase_when_passed_valid_string_converts_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }
}
