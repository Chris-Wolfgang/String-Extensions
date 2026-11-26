namespace Wolfgang.Extensions.String;

/// <summary>
/// How <c>ToCamelCase</c> and <c>ToPascalCase</c> case a word that is written entirely in upper case,
/// such as <c>SKU</c> in <c>SKUCode</c>.
/// </summary>
public enum AcronymHandling
{
    /// <summary>
    /// The acronym is cased like any other word: <c>SKUCode</c> becomes <c>SkuCode</c>. This is what
    /// the conversions do when no options are given.
    /// </summary>
    AsWord = 0,

    /// <summary>
    /// A word that has no lower-case letter is left as it is wherever a word is capitalized:
    /// <c>SKUCode</c> stays <c>SKUCode</c> in Pascal case. The first word of a camel-case result is
    /// still lower-cased: <c>skuCode</c>.
    /// </summary>
    Preserve = 1,
}
