using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

public class PadCenterTests
{

    [Fact]
    public void PadCenter_string_int_when_passed_null_string_throws_ArgumentNullException()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.PadCenter(10));
        Assert.Equal("s", exception.ParamName);
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_less_0_throws_ArgumentOutOfRangeException()
    {
        const string sut = "sample text";

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.PadCenter(-1));
        Assert.Equal("totalWidth", exception.ParamName);
    }



    [Fact]
    public void PadCenter_string_int_when_passed_null_string_and_totalWidth_less_than_0_throws_ArgumentNullException_on_first_argument()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.PadCenter(-1));
        Assert.Equal("s", exception.ParamName);
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_equals_0_returns_original_string()
    {
        const string sut = "sample text";

        Assert.Equal(sut, sut.PadCenter(0));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_less_than_string_length_returns_a_reference_to_existing_string()
    {
        const string sut = "sample text";

        Assert.Same(sut, sut.PadCenter(sut.Length - 1));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_equals_string_length_returns_value_equal_to_original_string()
    {
        const string sut = "sample text";

        Assert.Equal("sample text", sut.PadCenter(sut.Length));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_equals_string_length_returns_a_reference_to_a_new_string()
    {
        const string sut = "sample text";

        Assert.NotSame(sut, sut.PadCenter(sut.Length));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_1_greater_than_string_length_returns_space_on_end()
    {
        const string sut = "sample text";

        Assert.Equal("sample text ", sut.PadCenter(sut.Length + 1));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_2_greater_than_string_length_returns_space_on_each_end()
    {
        const string sut = "sample text";

        Assert.Equal(" sample text ", sut.PadCenter(sut.Length + 2));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_greater_than_string_length_returns_centers_text()
    {
        const string sut = "sample text";

        Assert.Equal("    sample text    ", sut.PadCenter(19));
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_less_than_0_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expectedException = Assert.Throws<ArgumentOutOfRangeException>(() => sut.PadRight(-1));
        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => sut.PadCenter(-1));

        Assert.Equal
        (
            expectedException.ParamName,
            actualException.ParamName
        );
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_equal_0_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expected = sut.PadRight(0);
        var actual = sut.PadCenter(0);

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_equal_string_length_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expected = sut.PadRight(sut.Length);
        var actual = sut.PadCenter(sut.Length);

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void PadCenter_string_int_when_passed_totalWidth_equal_string_length_plus_1_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expected = sut.PadRight(sut.Length + 1);
        var actual = sut.PadCenter(sut.Length + 1);

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_null_string_throws_ArgumentNullException()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.PadCenter(10, '-'));
        Assert.Equal("s", exception.ParamName);
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_less_0_throws_ArgumentOutOfRangeException()
    {
        const string sut = "sample text";

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.PadCenter(-1, '-'));
        Assert.Equal("totalWidth", exception.ParamName);
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_null_string_and_totalWidth_less_than_0_throws_ArgumentNullException_on_first_argument()
    {
        string? sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut!.PadCenter(-1, '-'));
        Assert.Equal("s", exception.ParamName);
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_equals_0_returns_original_string()
    {
        const string sut = "sample text";

        Assert.Equal(sut, sut.PadCenter(0, '-'));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_less_than_string_length_returns_a_reference_to_existing_string()
    {
        const string sut = "sample text";

        Assert.Same(sut, sut.PadCenter(sut.Length - 1, '-'));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_equals_string_length_returns_value_equal_to_original_string()
    {
        const string sut = "sample text";

        Assert.Equal("sample text", sut.PadCenter(sut.Length, '-'));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_equals_string_length_returns_a_reference_to_a_new_string()
    {
        const string sut = "sample text";

        Assert.NotSame(sut, sut.PadCenter(sut.Length, '-'));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_1_greater_than_string_length_returns_specified_char_on_end()
    {
        const string sut = "sample text";

        Assert.Equal("sample text-", sut.PadCenter(sut.Length + 1, '-'));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_2_greater_than_string_length_returns_specified_char_on_each_end()
    {
        const string sut = "sample text";

        Assert.Equal("-sample text-", sut.PadCenter(sut.Length + 2, '-'));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_greater_than_string_length_returns_centers_text()
    {
        const string sut = "sample text";

        Assert.Equal("----sample text----", sut.PadCenter(19, '-'));
    }



    [Theory]
    [InlineData('*')]
    [InlineData('-')]
    [InlineData('\t')]
    [InlineData('a')]
    [InlineData('ɕ')] // LATIN SMALL LETTER C WITH CURL, a non-ASCII padding char
    public void PadCenter_string_int_char_when_passed_centers_text_using_specified_character(char paddingChar)
    {
        const string sut = "sample text";
        var expected = string.Concat(new string(paddingChar, 4), sut, new string(paddingChar, 5));

        Assert.Equal(expected, sut.PadCenter(20, paddingChar));
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_less_than_0_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expectedException = Assert.Throws<ArgumentOutOfRangeException>(() => sut.PadRight(-1, '-'));
        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => sut.PadCenter(-1, '-'));

        Assert.Equal
        (
            expectedException.ParamName,
            actualException.ParamName
        );
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_equal_0_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expected = sut.PadRight(0, '-');
        var actual = sut.PadCenter(0, '-');

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_equal_string_length_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expected = sut.PadRight(sut.Length, '-');
        var actual = sut.PadCenter(sut.Length, '-');

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void PadCenter_string_int_char_when_passed_totalWidth_equal_string_length_plus_1_has_same_behavior_as_PadRight()
    {
        const string sut = "Sample Text";

        var expected = sut.PadRight(sut.Length + 1, '-');
        var actual = sut.PadCenter(sut.Length + 1, '-');

        Assert.Equal(expected, actual);
    }
}
