namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToCamelCaseTests
{


    [Fact]
    public void ToCamelCase_with_passed_null_throws_ArgumentNullException()
    {
        string sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.ToCamelCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "aStringInAllLower.")]
    [InlineData("A STRING IN ALL UPPER!", "aStringInAllUpper!")]
    [InlineData("a-string-with-no-spaces.", "a-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\ncapitalization", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\ncapitalization")]
    [InlineData(" A STRING starting with a leading space%", "aStringStartingWithALeadingSpace%")]
    public void ToCamelCase_convert_the_specified_string_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToCamelCase());
    }
}