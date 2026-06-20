using BenchmarkDotNet.Attributes;
using Wolfgang.Extensions.String;

namespace Wolfgang.Extensions.String.Benchmarks;

/// <summary>
/// Compares <see cref="StringExtensions.Left(string, int)"/> and
/// <see cref="StringExtensions.Right(string, int)"/> against the raw
/// <see cref="string.Substring(int, int)"/> calls they encapsulate. The
/// extensions add length-clamping and null-tolerance the BCL substring lacks,
/// so this measures the cost of that safety.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
public class LeftRightBenchmarks
{
    private string _input = null!;

    private const int Length = 8;

    [GlobalSetup]
    public void Setup() => _input = "The quick brown fox jumps over the lazy dog";

    [Benchmark(Baseline = true)]
    public string Substring_Left() => _input.Substring(0, Length);

    [Benchmark]
    public string Left() => _input.Left(Length)!;

    [Benchmark]
    public string Substring_Right() => _input.Substring(_input.Length - Length, Length);

    [Benchmark]
    public string Right() => _input.Right(Length)!;
}
