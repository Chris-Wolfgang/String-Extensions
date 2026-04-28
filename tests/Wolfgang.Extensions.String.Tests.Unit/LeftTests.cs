namespace Wolfgang.Extensions.String.Tests.Unit;

public class LeftTests
{
    [Fact]
    public void Left_WhenPassedLengthLessThanZero_ThrowsArgumentOutOfRangeException()
    {
        var sut = "Sample Text";
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.Left(-1));
        Assert.Equal("length", exception.ParamName);
    }



    [Fact]
    public void Left_WhenPassedNull_ReturnsNull()
    {
        var sut = (string?)null;

        Assert.Null(sut.Left(5));
    }



    [Fact]
    public void Left_WhenPassedEmptyString_ReturnsEmptyString()
    {
        var sut = string.Empty;

        Assert.Equal(string.Empty, sut.Left(5));
    }

        

    [Fact]
    public void Left_WhenPassedStringShorterThanSpecifiedLength_ReturnEntireString()
    {
        var sut = "1234";

        Assert.Equal("1234", sut.Left(5));
    }



    [Fact]
    public void Left_WhenPassedStringIsEqualToLengthSpecified_ReturnEntireString()
    {
        var sut = "12345";

        Assert.Equal("12345", sut.Left(5));
    }



    [Theory]
    [InlineData("123456", 5, "12345")]
    [InlineData("bobcat", 3, "bob")]
    [InlineData("Sample Text", 4, "Samp")]
    public void Left_WhenPassedStringIsGreaterThanLengthSpecified_ReturnOnlyTheSpecifiedPortion(string sut, int length, string expectedResult)
    {
        Assert.Equal(expectedResult, sut.Left(length));
    }
}