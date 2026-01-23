namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToKebabCaseTests
{


    [Fact]
    public void ToKebabCase_with_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.ToKebabCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "a-string-in-all-lower.")]
    [InlineData("A STRING IN ALL UPPER!", "a-string-in-all-upper!")]
    [InlineData("a-string-with-no-spaces.", "a-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\nunderscores", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\nunderscores")]
    [InlineData(" A STRING starting with a leading and trailing space ", "-a-string-starting-with-a-leading-and-trailing-space-")]
    public void ToKebabCase_convert_the_specified_string_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }
}