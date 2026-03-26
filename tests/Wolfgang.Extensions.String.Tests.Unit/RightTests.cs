namespace Wolfgang.Extensions.String.Tests.Unit;

public class RightTests
{

    [Fact]
    public void Right_WhenPassedLengthLessThanZero_ThrowsArgumentOutOfRangeException()
    {
        var sut = "Sample Text";
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Right(-1));
        Assert.Equal("length", exception.ParamName);
    }



    [Fact]
    public void Right_WhenPassedNull_ReturnsNull()
    {
        string sut = null;

        Assert.Null(sut.Right(5));
    }



    [Fact]
    public void Right_WhenPassedEmptyString_ReturnsEmptyString()
    {
        var sut = string.Empty;

        Assert.Equal(string.Empty, sut.Right(5));
    }



    [Fact]
    public void Right_WhenPassedStringShorterThanSpecifiedLength_ReturnEntireString()
    {
        var sut = "1234";

        Assert.Equal("1234", sut.Right(5));
    }



    [Fact]
    public void Right_WhenPassedStringIsEqualToLengthSpecified_ReturnEntireString()
    {
        var sut = "12345";

        Assert.Equal("12345", sut.Right(5));
    }



    [Theory]
    [InlineData("123456", 5, "23456")]
    [InlineData("bobcat", 3, "cat")]
    [InlineData("Sample Text", 4, "Text")]
    public void Right_WhenPassedStringIsGreaterThanLengthSpecified_ReturnOnlyTheSpecifiedPortion(string value, int length, string expectedResult)
    {
        var sut = value;

        Assert.Equal(expectedResult, sut.Right(length));
    }

}