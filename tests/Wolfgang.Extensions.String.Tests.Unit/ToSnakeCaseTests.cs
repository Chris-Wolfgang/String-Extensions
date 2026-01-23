namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToSnakeCaseTests
{


    [Fact]
    public void ToSnakeCase_with_passed_null_throws_ArgumentNullException()
    {
        string sut = null;

        Assert.Throws<ArgumentNullException>(() => sut.ToSnakeCase());
    }



    [Theory]
    [InlineData("a string in all lower.", "a_string_in_all_lower.")]
    [InlineData("A STRING IN ALL UPPER!", "a_string_in_all_upper!")]
    [InlineData("a-string-with-no-spaces.", "a-string-with-no-spaces.")]
    [InlineData("\nControl\tCHARACTERS\aremain\nand\tdon't\acause\nunderscores", "\ncontrol\tcharacters\aremain\nand\tdon't\acause\nunderscores")]
    [InlineData(" A STRING starting with a leading and trailing space ", "_a_string_starting_with_a_leading_and_trailing_space_")]
    public void ToSnakeCase_convert_the_specified_string_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToSnakeCase());
    }
}