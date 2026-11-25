#if NET5_0_OR_GREATER
using System;
#endif
using System.Linq;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

/// <summary>
/// The buffer the case conversions build their result in: on the stack for a string of up to 128
/// characters, on the heap for a longer one. The result must be the same either way.
/// </summary>
public class CaseConversionBufferTests
{
    // "aBaB...": every pair is a word boundary, so the result is as long as a result can get.
    private static string Pairs(int count)
    {
        return string.Concat(Enumerable.Repeat("aB", count));
    }



    [Theory]
    [InlineData(1)]
    [InlineData(63)]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(200)]
    [InlineData(5000)]
    public void ToSnakeCase_when_the_result_is_as_long_as_it_can_get_returns_all_of_it(int pairs)
    {
        var expected = "a" + string.Concat(Enumerable.Repeat("_ba", pairs - 1)) + "_b";

        Assert.Equal(expected, Pairs(pairs).ToSnakeCase());
    }



    [Theory]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(200)]
    public void ToKebabCase_when_the_result_is_as_long_as_it_can_get_returns_all_of_it(int pairs)
    {
        var expected = "a" + string.Concat(Enumerable.Repeat("-ba", pairs - 1)) + "-b";

        Assert.Equal(expected, Pairs(pairs).ToKebabCase());
    }



    [Theory]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(200)]
    public void ToPascalCase_when_the_string_is_long_returns_all_of_it(int pairs)
    {
        var expected = "A" + string.Concat(Enumerable.Repeat("Ba", pairs - 1)) + "B";

        Assert.Equal(expected, Pairs(pairs).ToPascalCase());
    }



    [Theory]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(200)]
    public void ToCamelCase_when_the_string_is_long_returns_all_of_it(int pairs)
    {
        var expected = "a" + string.Concat(Enumerable.Repeat("Ba", pairs - 1)) + "B";

        Assert.Equal(expected, Pairs(pairs).ToCamelCase());
    }



    [Fact]
    public void ToSnakeCase_when_passed_a_129_character_string_uses_a_buffer_that_holds_the_result()
    {
        var input = Pairs(64) + "a";
        var expected = "a" + string.Concat(Enumerable.Repeat("_ba", 64));

        Assert.Equal(129, input.Length);
        Assert.Equal(expected, input.ToSnakeCase());
    }



#if NET5_0_OR_GREATER
    // 128 characters is the longest string whose result is built on the stack. If a temporary array
    // were allocated for it, each call would cost several hundred bytes more than the result does.
    [Theory]
    [InlineData(14)]
    [InlineData(128)]
    public void ToSnakeCase_when_the_string_fits_the_stack_buffer_allocates_no_more_than_the_result(int length)
    {
        const int Calls = 1000;
        var input = new string('a', length);

        // The first calls pay for one-time work that is not the method's own.
        Assert.Equal(input, input.ToSnakeCase());
        Assert.Equal(length, new string('b', length).Length);

        var start = GC.GetAllocatedBytesForCurrentThread();

        for (var call = 0; call < Calls; call++)
        {
            _ = new string('b', length);
        }

        var resultOnly = GC.GetAllocatedBytesForCurrentThread() - start;
        start = GC.GetAllocatedBytesForCurrentThread();

        for (var call = 0; call < Calls; call++)
        {
            _ = input.ToSnakeCase();
        }

        var conversion = GC.GetAllocatedBytesForCurrentThread() - start;

        // A temporary array of 256 characters is 536 bytes; 64 bytes a call leaves room for noise
        // from the test host and none for the array.
        Assert.InRange(conversion, resultOnly, resultOnly + (Calls * 64));
    }
#endif
}
