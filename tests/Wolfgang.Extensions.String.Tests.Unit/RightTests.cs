namespace Wolfgang.Extensions.String.Tests.Unit;

public class RightTests
{

    [Fact]
    public void Right_when_passed_length_less_than_zero_throws_ArgumentOutOfRangeException()
    {
        var sut = "Sample Text";
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Right(-1));
        Assert.Equal("length", exception.ParamName);
    }



    [Fact]
    public void Right_when_passed_null_returns_null()
    {
        string? sut = null;

        Assert.Null(sut.Right(5));
    }



    [Fact]
    public void Right_when_passed_empty_string_returns_empty_string()
    {
        var sut = string.Empty;

        Assert.Equal(string.Empty, sut.Right(5));
    }



    [Fact]
    public void Right_when_passed_string_shorter_than_specified_length_returns_entire_string()
    {
        var sut = "1234";

        Assert.Equal("1234", sut.Right(5));
    }



    [Fact]
    public void Right_when_passed_string_is_equal_to_length_specified_returns_entire_string()
    {
        var sut = "12345";

        Assert.Equal("12345", sut.Right(5));
    }



    [Theory]
    [InlineData("123456", 5, "23456")]
    [InlineData("bobcat", 3, "cat")]
    [InlineData("Sample Text", 4, "Text")]
    public void Right_when_passed_string_is_greater_than_length_specified_returns_only_the_specified_portion(string value, int length, string expectedResult)
    {
        var sut = value;

        Assert.Equal(expectedResult, sut.Right(length));
    }

}