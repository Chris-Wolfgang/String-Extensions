using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FsCheck;
using Xunit;

namespace Wolfgang.Extensions.String.Tests.Fuzz;

/// <summary>
/// Property-based fuzz tests for <see cref="StringExtensions"/>. Each property compares a method
/// with a small reference model written out by hand (a <see cref="string.Substring(int, int)"/> for
/// <c>Left</c> / <c>Right</c>, explicit padding arithmetic for <c>PadCenter</c>, a word list built
/// in two steps for the case conversions). A disagreement means the method
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



    // Random strings almost never hold a valid surrogate pair or an apostrophe next to a letter, so
    // these inputs are checked on every run as well.
    [Theory]
    [InlineData("")]
    [InlineData("UserLoginCount")]
    [InlineData("don't stop, O'Brien")]
    [InlineData("\uD801\uDC00\uD801\uDC28 word")]
    [InlineData("emoji \uD83D\uDE00 face")]
    [InlineData("cafe\u0301Bar SKUCode Line2Qty")]
    public void Case_conversions_when_passed_a_seed_input_match_the_reference_word_list(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var words = Words(text);

        Assert.Equal(words, text.SplitWords());
        Assert.Equal(string.Join("_", words.Select(LowerCase)), text.ToSnakeCase());
        Assert.Equal(string.Join("-", words.Select(LowerCase)), text.ToKebabCase());
        Assert.Equal(string.Concat(words.Select(Capitalize)), text.ToPascalCase());
        Assert.Equal(string.Concat(words.Select((word, index) => index == 0 ? LowerCase(word) : Capitalize(word))), text.ToCamelCase());
    }



    [FuzzProperty]
    public bool ToKebabCase_lower_cases_and_puts_one_dash_between_words(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals(text.ToKebabCase(), string.Join("-", Words(text).Select(LowerCase)), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool ToSnakeCase_lower_cases_and_puts_one_underscore_between_words(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals(text.ToSnakeCase(), string.Join("_", Words(text).Select(LowerCase)), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool ToPascalCase_capitalizes_every_word_and_drops_everything_between_words(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
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
    public bool ToCamelCase_lower_cases_the_first_word_capitalizes_the_rest_and_drops_everything_between_words(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return string.Equals
        (
            text.ToCamelCase(),
            string.Concat(Words(text).Select((word, index) => index == 0 ? LowerCase(word) : Capitalize(word))),
            StringComparison.Ordinal
        );
    }



    // The overloads build a word list and format it; the methods without options walk the string
    // once. With new options the two must agree on every input.
    [FuzzProperty]
    public bool Case_conversions_with_new_options_match_the_methods_without_options(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);
        var options = new CaseConversionOptions();

        return string.Equals(text.ToCamelCase(options), text.ToCamelCase(), StringComparison.Ordinal)
            && string.Equals(text.ToKebabCase(options), text.ToKebabCase(), StringComparison.Ordinal)
            && string.Equals(text.ToPascalCase(options), text.ToPascalCase(), StringComparison.Ordinal)
            && string.Equals(text.ToSnakeCase(options), text.ToSnakeCase(), StringComparison.Ordinal);
    }



    [FuzzProperty]
    public bool SplitWords_returns_the_reference_word_list(NonNull<string> first, NonNull<string>[] rest, byte[] separatorPicks)
    {
        var text = Compose(first, rest, separatorPicks);

        return text.SplitWords().SequenceEqual(Words(text), StringComparer.Ordinal);
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
    // What Compose puts between the generated parts: the Unicode separators, and the other characters
    // the case conversions have a rule for (their delimiters, an apostrophe, control characters, a
    // combining mark, and a letter and an emoji outside the Basic Multilingual Plane).
    private static readonly string[] Separators =
    [
        " ", "\u00A0", "\u2003", "\u2028", "\u2029",
        "_", "-", "'", "\u2019", "\t", "\n", ".", "e\u0301", "\uD801\uDC00", "\uD83D\uDE00",
    ];



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



    // An upper-case letter starts a word when a lower-case letter or a digit comes before it, or when
    // an upper-case letter comes before it and a lower-case letter after it.
    private static bool StartsWord(string s, int i)
    {
        if (i == 0 || !char.IsUpper(s[i]))
        {
            return false;
        }

        var lowerOrDigitBefore = char.IsLower(s[i - 1]) || char.IsDigit(s[i - 1]);
        var endsUpperRun = char.IsUpper(s[i - 1]) && i + 1 < s.Length && char.IsLower(s[i + 1]);

        return lowerOrDigitBefore || endsUpperRun;
    }



    // The oracle's word list, built a different way from the library's single pass: take the
    // apostrophes out, mark which characters belong to words, cut the text into the marked runs, and
    // cut each run at its case boundaries.
    private static List<string> Words(string s)
    {
        var text = new string(s.Where(c => c != '\'' && c != '\u2019').ToArray());
        var inWord = WordCharacterMask(text);
        var words = new List<string>();
        var start = -1;

        for (var i = 0; i <= text.Length; i++)
        {
            if (i < text.Length && inWord[i])
            {
                start = start < 0 ? i : start;
            }
            else if (start >= 0)
            {
                words.AddRange(IdentifierWords(text.Substring(start, i - start)));
                start = -1;
            }
        }

        return words;
    }



    // True at every position that is a letter, a digit or a combining mark; a surrogate pair is
    // judged as one character and a surrogate on its own belongs to no word.
    private static bool[] WordCharacterMask(string s)
    {
        var mask = new bool[s.Length];
        var i = 0;

        while (i < s.Length)
        {
            if (char.IsSurrogatePair(s, i))
            {
                mask[i] = char.IsLetterOrDigit(s, i);
                mask[i + 1] = mask[i];
                i += 2;
            }
            else
            {
                mask[i] = char.IsLetterOrDigit(s[i]) || IsMark(s[i]);
                i++;
            }
        }

        return mask;
    }



    private static bool IsMark(char c) =>
        char.GetUnicodeCategory(c) is UnicodeCategory.NonSpacingMark or UnicodeCategory.SpacingCombiningMark or UnicodeCategory.EnclosingMark;



    // The words inside one run of word characters: it is cut in front of every
    // character that starts a word inside an identifier.
    private static IEnumerable<string> IdentifierWords(string piece)
    {
        var start = 0;

        for (var i = 1; i < piece.Length; i++)
        {
            if (StartsWord(piece, i))
            {
                yield return piece.Substring(start, i - start);
                start = i;
            }
        }

        yield return piece.Substring(start);
    }



    private static string Capitalize(string word) =>
        char.ToUpperInvariant(word[0]) + LowerCase(word.Substring(1));



    private static string LowerCase(string word) =>
        new(word.Select(char.ToLowerInvariant).ToArray());
}
