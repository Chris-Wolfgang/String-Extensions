#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// The output is not <see langword="null"/> when the named parameter is not <see langword="null"/>.
/// net462 and netstandard2.0 do not ship this attribute; the compiler honours a copy declared in the
/// library, so the nullable annotations are the same on every target.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
internal sealed class NotNullIfNotNullAttribute : Attribute
{
    /// <summary>Initializes the attribute with the associated parameter name.</summary>
    /// <param name="parameterName">The parameter whose null state decides the output's.</param>
    public NotNullIfNotNullAttribute(string parameterName) => ParameterName = parameterName;



    /// <summary>Gets the associated parameter name.</summary>
    public string ParameterName { get; }
}
#endif
