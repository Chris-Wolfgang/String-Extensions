using BenchmarkDotNet.Attributes;
using Wolfgang.Extensions.String;

namespace Wolfgang.Extensions.String.Benchmarks;

/// <summary>
/// Measures the casing conversions (<see cref="StringExtensions.ToCamelCase"/>,
/// <see cref="StringExtensions.ToPascalCase"/>, <see cref="StringExtensions.ToKebabCase"/>,
/// <see cref="StringExtensions.ToSnakeCase"/>, <see cref="StringExtensions.ToTitleCase"/>)
/// over a representative multi-word input. <see cref="StringExtensions.ToTitleCase"/>
/// delegates to <see cref="System.Globalization.TextInfo.ToTitleCase"/> and serves as the
/// BCL-backed reference point.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
public class CasingBenchmarks
{
    private string _input = null!;

    [GlobalSetup]
    public void Setup() => _input = " A sample string for processing! ";

    [Benchmark(Baseline = true)]
    public string ToTitleCase() => _input.ToTitleCase();

    [Benchmark]
    public string ToCamelCase() => _input.ToCamelCase();

    [Benchmark]
    public string ToPascalCase() => _input.ToPascalCase();

    [Benchmark]
    public string ToKebabCase() => _input.ToKebabCase();

    [Benchmark]
    public string ToSnakeCase() => _input.ToSnakeCase();
}
