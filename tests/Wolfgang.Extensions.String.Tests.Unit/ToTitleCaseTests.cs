namespace Wolfgang.Extensions.String.Tests.Unit;

public class ToTitleCaseTests
{


    [Fact]
    public void ToTitleCase_with_passed_null_throws_ArgumentNullException()
    {
        string? sut = null;

        Assert.Throws<ArgumentNullException>(() => sut!.ToTitleCase());
    }



    [Theory]
    [InlineData("a string in   all lower.", "A String In   All Lower.")]
    [InlineData("A STRING IN ALL UPPER STAYS ALL UPPER!", "A STRING IN ALL UPPER STAYS ALL UPPER!")]
    [InlineData("a-string-with-no-spaces.", "A-String-With-No-Spaces.")]
    [InlineData("\nControl\tCharacters\aremain\n", "\nControl\tCharacters\aRemain\n")]
    [InlineData(" A string starting with a leading and trailing space ", " A String Starting With A Leading And Trailing Space ")]
    [InlineData("A string with acronym CRM in it.", "A String With Acronym CRM In It.")]
    public void ToTitleCase_convert_the_specified_string_properly(string input, string expectedResult)
    {
        Assert.Equal(expectedResult, input.ToTitleCase());
    }
}