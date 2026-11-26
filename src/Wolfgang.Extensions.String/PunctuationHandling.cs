namespace Wolfgang.Extensions.String;

/// <summary>
/// What a case conversion does with a character that is neither a letter nor a digit, such as a comma,
/// a bracket, a symbol or an emoji.
/// </summary>
public enum PunctuationHandling
{
    /// <summary>
    /// The character ends the word and is left out of the result. An apostrophe is removed without
    /// ending the word. This is what the conversions do when no options are given.
    /// </summary>
    Drop = 0,

    /// <summary>
    /// The character stays in the result as part of the word it touches, unless it is one of the
    /// <see cref="WordBoundaries"/> the options name.
    /// </summary>
    Keep = 1,
}
