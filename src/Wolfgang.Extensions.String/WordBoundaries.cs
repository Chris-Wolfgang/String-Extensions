using System;

namespace Wolfgang.Extensions.String;

/// <summary>
/// The things that divide one word from the next when a case conversion splits a string.
/// </summary>
/// <remarks>
/// <see cref="Separators"/>, <see cref="Delimiters"/> and <see cref="ControlCharacters"/> only make a
/// difference together with <see cref="PunctuationHandling.Keep"/>: when punctuation is dropped, every
/// character that is not a letter or a digit ends the word whatever is chosen here.
/// </remarks>
[Flags]
public enum WordBoundaries
{
    /// <summary>Nothing divides words except what <see cref="PunctuationHandling.Drop"/> removes.</summary>
    None = 0,

    /// <summary>
    /// A character for which <see cref="char.IsSeparator(char)"/> is <see langword="true"/>: a space,
    /// a no-break space, a line or paragraph separator.
    /// </summary>
    Separators = 1,

    /// <summary>An underscore or a dash, the delimiters of snake case and kebab case.</summary>
    Delimiters = 2,

    /// <summary>
    /// A change of case inside an identifier: an upper-case letter after a lower-case letter or a digit,
    /// and the last upper-case letter of a run that a lower-case letter follows.
    /// </summary>
    CaseChanges = 4,

    /// <summary>
    /// A character for which <see cref="char.IsControl(char)"/> is <see langword="true"/>, such as a
    /// tab or a line feed.
    /// </summary>
    ControlCharacters = 8,

    /// <summary>All of the above. This is what the conversions use when no options are given.</summary>
    All = Separators | Delimiters | CaseChanges | ControlCharacters,
}
