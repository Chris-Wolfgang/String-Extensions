#if NET5_0_OR_GREATER
using System;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Unit;

/// <summary>
/// Asserts that the call shapes documented as allocation-free stay allocation-free, and that
/// <see cref="StringExtensions.PadCenter(string, int, char)"/> allocates only its result, measured with
/// <see cref="GC.GetAllocatedBytesForCurrentThread"/> over many iterations after a warm-up pass.
/// </summary>
/// <remarks>
/// <para>
/// Documented zero-allocation call shapes (each returns an existing instance):
/// </para>
/// <list type="bullet">
/// <item><description><c>Left</c> / <c>Right</c> when <c>length</c> is at least the string's length (the
/// same instance), when <c>length</c> is 0 (<see cref="string.Empty"/>), and for a <see langword="null"/>
/// string.</description></item>
/// <item><description><c>PadCenter</c> (both overloads) when <c>totalWidth</c> is less than the string's
/// length (the same instance).</description></item>
/// </list>
/// <para>
/// One allocation: <c>PadCenter</c> that pads allocates exactly its result string on .NET 8 and later
/// (<c>string.Create</c>); the test compares it with a plain <c>new string(char, int)</c> of the same length.
/// <c>Left</c> / <c>Right</c> that cut allocate the substring, and the case conversions allocate a work
/// buffer plus the result; those are not asserted here.
/// </para>
/// <para>
/// Guarded on NET5_0_OR_GREATER: <see cref="GC.GetAllocatedBytesForCurrentThread"/> does not exist on the
/// .NET Framework targets, and net5.0-net7.0 run the netstandard2.0 asset, so the single-allocation test
/// is guarded on NET8_0_OR_GREATER.
/// </para>
/// </remarks>
public class ZeroAllocationTests
{
    private const int Iterations = 10_000;

    private const string Value = "Sample Text";



    [Fact]
    public void Left_when_length_covers_the_string_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => Value.Left(Value.Length + 5));
    }



    [Fact]
    public void Left_when_length_is_0_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => Value.Left(0));
    }



    [Fact]
    public void Left_when_passed_null_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => ((string?)null).Left(3));
    }



    [Fact]
    public void Right_when_length_covers_the_string_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => Value.Right(Value.Length));
    }



    [Fact]
    public void Right_when_length_is_0_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => Value.Right(0));
    }



    [Fact]
    public void Right_when_passed_null_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => ((string?)null).Right(3));
    }



    [Fact]
    public void PadCenter_string_int_when_totalWidth_is_less_than_length_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => Value.PadCenter(Value.Length - 1));
    }



    [Fact]
    public void PadCenter_string_int_char_when_totalWidth_is_less_than_length_allocates_zero_bytes()
    {
        AssertZeroAllocation(() => Value.PadCenter(3, '*'));
    }

#if NET8_0_OR_GREATER


    [Fact]
    public void PadCenter_string_int_char_when_padding_allocates_only_the_result_string()
    {
        const int width = 80;

        var expected = MeasureBytes(() => new string('*', width));
        var actual = MeasureBytes(() => Value.PadCenter(width, '*'));

        Assert.Equal(expected, actual);
    }
#endif



    private static void AssertZeroAllocation(Func<string?> call)
    {
        var allocated = MeasureBytes(call);

        Assert.True
        (
            allocated == 0,
            $"Expected 0 bytes allocated over {Iterations} calls, measured {allocated} bytes."
        );
    }



    private static long MeasureBytes(Func<string?> call)
    {
        // Warm-up with a full pass: JIT compilation, first-call type loading and the tiered-JIT
        // promotion that happens part-way through the first few thousand calls can allocate a few
        // bytes once. That is not the call's cost, so it stays out of the measured pass.
        for (var i = 0; i < Iterations; i++)
        {
            _ = call();
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < Iterations; i++)
        {
            _ = call();
        }

        return GC.GetAllocatedBytesForCurrentThread() - before;
    }
}
#endif
