namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToPascalCaseTests
{


    [Fact]
    public void ToPascalCase_with_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.ToPascalCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "AStringInAllLower.")]
    [InlineData("A STRING IN ALL UPPER!", "AStringInAllUpper!")]
    [InlineData("a-string-with-no-spaces.", "A-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\ncapitalization", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\ncapitalization")]
    [InlineData(" A STRING starting with a leading space%", "AStringStartingWithALeadingSpace%")]
    public void ToPascalCase_convert_the_specified_string_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToPascalCase());
    }
}