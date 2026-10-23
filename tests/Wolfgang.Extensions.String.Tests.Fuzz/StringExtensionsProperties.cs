using System;
using System.Globalization;
using System.Linq;
using FsCheck;

namespace Wolfgang.Extensions.String.Tests.Fuzz;

/// <summary>
/// Property-based fuzz tests for <see cref="StringExtensions"/>. Each property compares a method
/// with a small reference model written out by hand (a <see cref="string.Substring(int, int)"/> for
/// <c>Left</c> / <c>Right</c>, explicit padding arithmetic for <c>PadCenter</c>, a word split on
/// <see cref="char.IsSeparator(char)"/> for the case conversions). A disagreement means the method
/// gained or lost a behaviour that a hand-picked unit test did not pin down.
/// </summary>
/// <remarks>
/// Inputs are generated as <see cref="NonNull{T}"/> strings (the null paths are covered by the unit
/// tests) and non-negative lengths. Each property runs <see cref="FuzzPropertyAttribute.DefaultMaxTest"/>
/// (100) cases by default; the weekly <c>fuzz.yaml</c> run sets <c>FSCHECK_MAX_TESTS</c> to 100,000 (#100).
/// The reference models lower- and upper-case one <see cref="char"/> at a time with the invariant
/// culture, exactly as the library documents, so surrogate pairs and special casings agree.
/// </remarks>
public class StringExtensionsProperties
{
    // Widths are kept modest so a 100,000-case run stays fast; the arithmetic does not depend on size.
    private const int MaxWidth = 512;



    [FuzzProperty]
    public bool Left_returns_the_first_min_length_characters(NonNull<string> input, NonNegativeInt length)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(length);

        var s = input.Get;
        var n = length.Get;

        return string.Equals(s.Left(n), s.Substring(0, Math.Min(n, s.Length)), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool Right_returns_the_last_min_length_characters(NonNull<string> input, NonNegativeInt length)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(length);

        var s = input.Get;
        var n = length.Get;
        var take = Math.Min(n, s.Length);

        return string.Equals(s.Right(n), s.Substring(s.Length - take, take), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool Left_and_Right_split_the_string_without_losing_characters(NonNull<string> input, NonNegativeInt cut)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(cut);

        var s = input.Get;
        var k = cut.Get % (s.Length + 1);

        return string.Equals(s.Left(k) + s.Right(s.Length - k), s, StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool Left_and_Right_return_the_same_instance_when_length_covers_the_string(NonNull<string> input, NonNegativeInt extra)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(extra);

        var s = input.Get;
        var n = s.Length + (extra.Get % MaxWidth);

        return ReferenceEquals(s, s.Left(n)) && ReferenceEquals(s, s.Right(n));
    }



    [FuzzProperty]
    public bool PadCenter_centers_with_the_extra_character_on_the_right(NonNull<string> input, NonNegativeInt width, char paddingChar)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(width);

        var s = input.Get;
        var w = width.Get % MaxWidth;
        var total = Math.Max(w, s.Length);
        var left = (total - s.Length) / 2;
        var expected = new string(paddingChar, left) + s + new string(paddingChar, total - s.Length - left);

        return string.Equals(s.PadCenter(w, paddingChar), expected, StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool PadCenter_without_a_padding_char_pads_with_spaces(NonNull<string> input, NonNegativeInt width)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(width);

        var s = input.Get;
        var w = width.Get % MaxWidth;

        return string.Equals(s.PadCenter(w), s.PadCenter(w, ' '), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool ToKebabCase_lower_cases_and_replaces_separators_with_dashes(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals(text.ToKebabCase(), MapEachChar(text, '-'), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool ToSnakeCase_lower_cases_and_replaces_separators_with_underscores(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals(text.ToSnakeCase(), MapEachChar(text, '_'), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool ToPascalCase_capitalizes_every_word_and_drops_separators(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals
        (
            text.ToPascalCase(),
            string.Concat(Words(text).Select(Capitalize)),
            StringComparison.Ordinal
        );
    }



    [FuzzProperty]
    public bool ToCamelCase_lower_cases_the_first_word_and_capitalizes_the_rest(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals
        (
            text.ToCamelCase(),
            string.Concat(Words(text).Select((word, index) => index == 0 ? LowerCase(word) : Capitalize(word))),
            StringComparison.Ordinal
        );
    }



    [FuzzProperty]
    public bool ToTitleCase_matches_TextInfo_of_the_current_culture(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals
        (
            text.ToTitleCase(),
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text),
            StringComparison.Ordinal
        );
    }



    // Random strings rarely contain a separator, so the case-conversion properties build their input
    // from generated parts joined by real separators: ASCII space, NO-BREAK SPACE, EM SPACE, LINE
    // SEPARATOR and PARAGRAPH SEPARATOR (Zs, Zl and Zp). An empty first part gives leading separators,
    // empty later parts give runs of them.
    private static readonly char[] Separators = [' ', '\u00A0', '\u2003', '\u2028', '\u2029'];



    private static string Compose(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(rest);
        ArgumentNullException.ThrowIfNull(separatorPicks);

        return first.Get + string.Concat
        (
            rest.Select((part, index) => Separators[Pick(separatorPicks, index)] + part.Get)
        );
    }



    private static int Pick(byte[] picks, int index) =>
        picks.Length == 0 ? 0 : picks[index % picks.Length] % Separators.Length;



    private static string MapEachChar(string s, char separatorReplacement) =>
        new(s.Select(c => char.IsSeparator(c) ? separatorReplacement : char.ToLowerInvariant(c)).ToArray());



    // Splits on every char.IsSeparator character. U+2028 (itself a separator) is always in the list,
    // so the array is never empty: string.Split with an empty array would split on white space instead.
    private static string[] Words(string s) =>
        s.Split
        (
            s.Where(char.IsSeparator).Append('\u2028').Distinct().ToArray(),
            StringSplitOptions.RemoveEmptyEntries
        );



    private static string Capitalize(string word) =>
        char.ToUpperInvariant(word[0]) + LowerCase(word.Substring(1));



    private static string LowerCase(string word) =>
        new(word.Select(char.ToLowerInvariant).ToArray());
}
