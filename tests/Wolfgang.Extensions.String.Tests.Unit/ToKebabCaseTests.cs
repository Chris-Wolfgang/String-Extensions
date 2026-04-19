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
    [InlineData(" A STRING starting with a leading and trailing space ", "-a-string-starting-with-a-leading-and-trailing-space-")]
    [InlineData("  consecutive  spaces  ", "--consecutive--spaces--")]
    public void ToKebabCase_when_passed_valid_string_converts_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToKebabCase());
    }
}
