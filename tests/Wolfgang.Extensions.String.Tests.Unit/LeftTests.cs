namespace Wolfgang.Extensions.String.Tests.Unit;

public class LeftTests
{
    [Fact]
    public void Left_when_passed_length_less_than_zero_throws_ArgumentOutOfRangeException()
    {
        var sut = "Sample Text";
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Left(-1));
        Assert.Equal("length", exception.ParamName);
    }



    [Fact]
    public void Left_when_passed_null_returns_null()
    {
        var sut = (string?)null;

        Assert.Null(sut.Left(5));
    }



    [Fact]
    public void Left_when_passed_empty_string_returns_empty_string()
    {
        var sut = string.Empty;

        Assert.Equal(string.Empty, sut.Left(5));
    }

        

    [Fact]
    public void Left_when_passed_string_shorter_than_specified_length_returns_entire_string()
    {
        var sut = "1234";

        Assert.Equal("1234", sut.Left(5));
    }



    [Fact]
    public void Left_when_passed_string_is_equal_to_length_specified_returns_entire_string()
    {
        var sut = "12345";

        Assert.Equal("12345", sut.Left(5));
    }



    [Theory]
    [InlineData("123456", 5, "12345")]
    [InlineData("bobcat", 3, "bob")]
    [InlineData("Sample Text", 4, "Samp")]
    public void Left_when_passed_string_is_greater_than_length_specified_returns_only_the_specified_portion(string sut, int length, string expectedResult)
    {
        Assert.Equal(expectedResult, sut.Left(length));
    }
}