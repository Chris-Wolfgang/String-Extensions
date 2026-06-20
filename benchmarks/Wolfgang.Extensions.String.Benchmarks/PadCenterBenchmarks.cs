using BenchmarkDotNet.Attributes;
using Wolfgang.Extensions.String;

namespace Wolfgang.Extensions.String.Benchmarks;

/// <summary>
/// Compares <see cref="StringExtensions.PadCenter(string, int)"/> (and its
/// padding-char overload) against the canonical hand-rolled
/// <see cref="string.PadLeft(int)"/> + <see cref="string.PadRight(int)"/> idiom
/// callers reach for when no center-pad helper exists.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
public class PadCenterBenchmarks
{
    private string _input = null!;

    private const int TotalWidth = 80;

    [GlobalSetup]
    public void Setup() => _input = "centered";

    [Benchmark(Baseline = true)]
    public string Manual_PadLeftPadRight()
    {
        var leftPadding = (TotalWidth - _input.Length) / 2;
        return _input.PadLeft(_input.Length + leftPadding).PadRight(TotalWidth);
    }

    [Benchmark]
    public string PadCenter() => _input.PadCenter(TotalWidth);

    [Benchmark]
    public string PadCenter_WithChar() => _input.PadCenter(TotalWidth, '*');
}
