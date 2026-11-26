using System;
using System.Collections.Generic;

namespace Wolfgang.Extensions.String;

/// <summary>
/// How <c>ToCamelCase</c>, <c>ToKebabCase</c>, <c>ToPascalCase</c>, <c>ToSnakeCase</c> and
/// <c>SplitWords</c> find the words of a string. A new instance holds the rules the conversions use
/// when no options are given.
/// </summary>
/// <example>
/// <code>
/// using Wolfgang.Extensions.String;
///
/// var options = new CaseConversionOptions
/// {
///     Punctuation = PunctuationHandling.Keep,
///     Boundaries = WordBoundaries.Separators,
/// };
///
/// var snake = "Hello, World!".ToSnakeCase(options);
/// // snake == "hello,_world!"
/// </code>
/// </example>
public sealed class CaseConversionOptions
{
    private PunctuationHandling _punctuation = PunctuationHandling.Drop;

    private WordBoundaries _boundaries = WordBoundaries.All;

    private AcronymHandling _acronyms = AcronymHandling.AsWord;



    /// <summary>
    /// What happens to a character that is neither a letter nor a digit. The default is
    /// <see cref="PunctuationHandling.Drop"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value is not a member of <see cref="PunctuationHandling"/>.
    /// </exception>
    public PunctuationHandling Punctuation
    {
        get => _punctuation;
        set
        {
            if (value != PunctuationHandling.Drop && value != PunctuationHandling.Keep)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The value is not a member of PunctuationHandling.");
            }

            _punctuation = value;
        }
    }



    /// <summary>
    /// What divides one word from the next. The default is <see cref="WordBoundaries.All"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value has a bit that is not a member of <see cref="WordBoundaries"/>.
    /// </exception>
    public WordBoundaries Boundaries
    {
        get => _boundaries;
        set
        {
            if ((value & ~WordBoundaries.All) != WordBoundaries.None)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The value has a bit that is not a member of WordBoundaries.");
            }

            _boundaries = value;
        }
    }



    /// <summary>
    /// How camel case and Pascal case treat a word written entirely in upper case. The default is
    /// <see cref="AcronymHandling.AsWord"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value is not a member of <see cref="AcronymHandling"/>.
    /// </exception>
    public AcronymHandling Acronyms
    {
        get => _acronyms;
        set
        {
            if (value != AcronymHandling.AsWord && value != AcronymHandling.Preserve)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The value is not a member of AcronymHandling.");
            }

            _acronyms = value;
        }
    }



    /// <summary>
    /// A function that finds the words of the string in place of the built-in rules, or
    /// <see langword="null"/> (the default) to use the built-in rules.
    /// </summary>
    /// <remarks>
    /// When it is set, <see cref="Punctuation"/> and <see cref="Boundaries"/> are not used: the
    /// conversion cases and joins exactly the words the function returns, in the order it returns
    /// them. The sequence is enumerated once. A <see langword="null"/> or empty word is skipped.
    /// </remarks>
    public Func<string, IEnumerable<string>>? WordSplitter { get; set; }
}
