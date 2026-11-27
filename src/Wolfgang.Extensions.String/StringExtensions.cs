using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Wolfgang.Extensions.String;

/// <summary>
/// A collection of extension methods for the <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns the leftmost <paramref name="length"/> characters of the string.
    /// </summary>
    /// <param name="s">The instance to retrieve from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins at index 0 and is the specified number of characters long.
    /// If the string is null, null will be returned. If the string contains fewer than the specified number of characters
    /// then the entire string is returned.
    /// </returns>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var prefix = "A sample string".Left(8);   // "A sample"
    /// var whole = "Hello".Left(10);             // "Hello" (shorter than length: unchanged)
    /// string? missing = null;
    /// var none = missing.Left(3);               // null
    /// </code>
    /// </example>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="length"/> is less than zero.
    /// </exception>
    public static string? Left(this string? s, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (s == null)
        {
            return null;
        }

        // Substring returns the same instance when it covers the whole string, so a string
        // no longer than length comes back unchanged without a separate length check.
        return s.Substring(0, Math.Min(length, s.Length));
    }



    /// <summary>
    /// Returns a new string of a specified length in which the current string is centered with spaces padding either side.
    /// </summary>
    /// <param name="s">The string to center.</param>
    /// <param name="totalWidth">
    /// The number of characters in the resulting string. If <paramref name="totalWidth"/> is less than or equal to the length of
    /// <paramref name="s"/>, the original string instance is returned unchanged.
    /// </param>
    /// <returns>
    /// A new string that is equivalent to <paramref name="s"/>, but center-aligned and padded on both sides with spaces.
    /// When the padding is uneven, the extra character is placed on the right.
    /// </returns>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var centered = "Hello".PadCenter(11);     // "   Hello   "
    /// var uneven = "Hello".PadCenter(8);        // " Hello  " (the extra space goes on the right)
    /// var unchanged = "Hello".PadCenter(3);     // "Hello" (already at least totalWidth wide: the same instance is returned)
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="totalWidth"/> is less than zero.
    /// </exception>
    public static string PadCenter(this string s, int totalWidth)
        => PadCenter(s, totalWidth, ' ');



    /// <summary>
    /// Returns a new string of a specified length in which the current string is centered with a specified character padding either side.
    /// </summary>
    /// <param name="s">The string to center.</param>
    /// <param name="totalWidth">
    /// The number of characters in the resulting string. If <paramref name="totalWidth"/> is less than or equal to the length of
    /// <paramref name="s"/>, the original string instance is returned unchanged.
    /// </param>
    /// <param name="paddingChar">A Unicode padding character.</param>
    /// <returns>
    /// A new string that is equivalent to <paramref name="s"/>, but center-aligned and padded on both sides with the
    /// <paramref name="paddingChar"/> character. When the padding is uneven, the extra character is placed on the right.
    /// </returns>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var centered = "Hello".PadCenter(11, '-'); // "---Hello---"
    /// var uneven = "Hello".PadCenter(8, '*');    // "*Hello**"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="totalWidth"/> is less than zero.
    /// </exception>
    public static string PadCenter(this string s, int totalWidth, char paddingChar)
    {
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }

        if (totalWidth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalWidth));
        }

        if (totalWidth <= s.Length)
        {
            return s;
        }

        var leftPadding = (totalWidth - s.Length) / 2;
#if NET8_0_OR_GREATER
        // One allocation: the result string is written in place.
        return string.Create
        (
            totalWidth,
            (Value: s, PaddingChar: paddingChar, LeftPadding: leftPadding),
            static (span, state) =>
            {
                span.Slice(0, state.LeftPadding).Fill(state.PaddingChar);
                state.Value.AsSpan().CopyTo(span.Slice(state.LeftPadding));
                span.Slice(state.LeftPadding + state.Value.Length).Fill(state.PaddingChar);
            }
        );
#else
        var rightPadding = totalWidth - s.Length - leftPadding;

        return new StringBuilder(totalWidth)
            .Append(paddingChar, leftPadding)
            .Append(s)
            .Append(paddingChar, rightPadding)
            .ToString();
#endif
    }



    /// <summary>
    /// Returns the rightmost <paramref name="length"/> characters of the string.
    /// </summary>
    /// <param name="s">The instance to retrieve from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins the specified number of characters before the end and continues to the last character.
    /// If the string is null, null will be returned. If the string contains fewer than the specified number of characters
    /// then the entire string is returned.
    /// </returns>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var suffix = "A sample string".Right(6);  // "string"
    /// var whole = "Hello".Right(10);            // "Hello" (shorter than length: unchanged)
    /// string? missing = null;
    /// var none = missing.Right(3);              // null
    /// </code>
    /// </example>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="length"/> is less than zero.
    /// </exception>
    public static string? Right(this string? s, int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (s == null)
        {
            return null;
        }

        // Substring returns the same instance when it covers the whole string, so a string
        // no longer than length comes back unchanged without a separate length check.
        return s.Substring(s.Length - Math.Min(length, s.Length));
    }



    /// <summary>
    /// Converts a string to camelCase: its words run together, the first in lower case and every later one
    /// with its first letter in upper case and the rest in lower case.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in camel case. It is empty when the string has no letters or digits.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The string is first split into words, by the rule every case conversion shares. A word is a run of
    /// letters and digits. Every other character (white space, punctuation, symbols, control characters,
    /// underscores and dashes) ends the word and is left out. An apostrophe is removed without ending the
    /// word, so <c>don't</c> is the one word <c>dont</c>.
    /// </para>
    /// <para>
    /// A word also starts at an upper-case letter that follows a lower-case letter or a digit, and at the
    /// last upper-case letter of a run that a lower-case letter follows. An acronym is therefore one word
    /// and a digit stays with the word before it: <c>SKUCode</c> is <c>SKU</c> and <c>Code</c>, and
    /// <c>Line2Qty</c> is <c>Line2</c> and <c>Qty</c>.
    /// </para>
    /// <para>
    /// Letters are cased one <see cref="char"/> at a time with the invariant culture, so the result does not
    /// depend on the current culture.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var camel = " A sample string, for processing! ".ToCamelCase();
    /// // camel == "aSampleStringForProcessing"
    ///
    /// var fromIdentifier = "UserLoginCount".ToCamelCase();
    /// // fromIdentifier == "userLoginCount"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToCamelCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        return JoinWords(s, NoDelimiter, WordCasing.CapitalizedAfterFirst);
    }



    /// <summary>
    /// Converts a string to camel case, finding its words by the rules in <paramref name="options"/>.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <param name="options">How the words of the string are found, and how an acronym is cased.</param>
    /// <returns>
    /// The specified string in camel case. It is empty when no word is found.
    /// </returns>
    /// <remarks>
    /// With a new <see cref="CaseConversionOptions"/> the result is the same as
    /// <see cref="ToCamelCase(string)"/> gives. When <see cref="CaseConversionOptions.WordSplitter"/> is
    /// set, the words it returns are cased and joined as they are.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var options = new CaseConversionOptions { Boundaries = WordBoundaries.All &amp; ~WordBoundaries.CaseChanges };
    ///
    /// var camel = "iPhone case".ToCamelCase(options);
    /// // camel == "iphoneCase"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="CaseConversionOptions.WordSplitter"/> returned <see langword="null"/>.
    /// </exception>
    public static string ToCamelCase(this string s, CaseConversionOptions options)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        return FormatWords(FindWords(s, options), NoDelimiter, WordCasing.CapitalizedAfterFirst, options.Acronyms);
    }



    /// <summary>
    /// Converts a string to kebab case: its words in lower case with one dash between them.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in kebab case. It is empty when the string has no letters or digits.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The string is first split into words, by the rule every case conversion shares. A word is a run of
    /// letters and digits. Every other character (white space, punctuation, symbols, control characters,
    /// underscores and dashes) ends the word and is left out. An apostrophe is removed without ending the
    /// word, so <c>don't</c> is the one word <c>dont</c>.
    /// </para>
    /// <para>
    /// A word also starts at an upper-case letter that follows a lower-case letter or a digit, and at the
    /// last upper-case letter of a run that a lower-case letter follows. An acronym is therefore one word
    /// and a digit stays with the word before it: <c>SKUCode</c> is <c>SKU</c> and <c>Code</c>, and
    /// <c>Line2Qty</c> is <c>Line2</c> and <c>Qty</c>.
    /// </para>
    /// <para>
    /// Letters are cased one <see cref="char"/> at a time with the invariant culture, so the result does not
    /// depend on the current culture.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var kebab = " A sample string, for processing! ".ToKebabCase();
    /// // kebab == "a-sample-string-for-processing"
    ///
    /// var fromIdentifier = "UserLoginCount".ToKebabCase();
    /// // fromIdentifier == "user-login-count"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToKebabCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        return JoinWords(s, '-', WordCasing.Lower);
    }



    /// <summary>
    /// Converts a string to kebab case, finding its words by the rules in <paramref name="options"/>.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <param name="options">How the words of the string are found, and how an acronym is cased.</param>
    /// <returns>
    /// The specified string in kebab case. It is empty when no word is found.
    /// </returns>
    /// <remarks>
    /// With a new <see cref="CaseConversionOptions"/> the result is the same as
    /// <see cref="ToKebabCase(string)"/> gives. When <see cref="CaseConversionOptions.WordSplitter"/> is
    /// set, the words it returns are cased and joined as they are.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var options = new CaseConversionOptions { Boundaries = WordBoundaries.All &amp; ~WordBoundaries.CaseChanges };
    ///
    /// var kebab = "iPhone case".ToKebabCase(options);
    /// // kebab == "iphone-case"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="CaseConversionOptions.WordSplitter"/> returned <see langword="null"/>.
    /// </exception>
    public static string ToKebabCase(this string s, CaseConversionOptions options)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        return FormatWords(FindWords(s, options), '-', WordCasing.Lower, options.Acronyms);
    }



    /// <summary>
    /// Converts a string to PascalCase: its words run together, each with its first letter in upper case and
    /// the rest in lower case.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in Pascal case. It is empty when the string has no letters or digits.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The string is first split into words, by the rule every case conversion shares. A word is a run of
    /// letters and digits. Every other character (white space, punctuation, symbols, control characters,
    /// underscores and dashes) ends the word and is left out. An apostrophe is removed without ending the
    /// word, so <c>don't</c> is the one word <c>dont</c>.
    /// </para>
    /// <para>
    /// A word also starts at an upper-case letter that follows a lower-case letter or a digit, and at the
    /// last upper-case letter of a run that a lower-case letter follows. An acronym is therefore one word
    /// and a digit stays with the word before it: <c>SKUCode</c> is <c>SKU</c> and <c>Code</c>, and
    /// <c>Line2Qty</c> is <c>Line2</c> and <c>Qty</c>.
    /// </para>
    /// <para>
    /// Letters are cased one <see cref="char"/> at a time with the invariant culture, so the result does not
    /// depend on the current culture.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var pascal = " A sample string, for processing! ".ToPascalCase();
    /// // pascal == "ASampleStringForProcessing"
    ///
    /// var fromIdentifier = "user_login_count".ToPascalCase();
    /// // fromIdentifier == "UserLoginCount"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToPascalCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        return JoinWords(s, NoDelimiter, WordCasing.Capitalized);
    }



    /// <summary>
    /// Converts a string to Pascal case, finding its words by the rules in <paramref name="options"/>.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <param name="options">How the words of the string are found, and how an acronym is cased.</param>
    /// <returns>
    /// The specified string in Pascal case. It is empty when no word is found.
    /// </returns>
    /// <remarks>
    /// With a new <see cref="CaseConversionOptions"/> the result is the same as
    /// <see cref="ToPascalCase(string)"/> gives. When <see cref="CaseConversionOptions.WordSplitter"/> is
    /// set, the words it returns are cased and joined as they are.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var options = new CaseConversionOptions { Boundaries = WordBoundaries.All &amp; ~WordBoundaries.CaseChanges };
    ///
    /// var pascal = "iPhone case".ToPascalCase(options);
    /// // pascal == "IphoneCase"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="CaseConversionOptions.WordSplitter"/> returned <see langword="null"/>.
    /// </exception>
    public static string ToPascalCase(this string s, CaseConversionOptions options)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        return FormatWords(FindWords(s, options), NoDelimiter, WordCasing.Capitalized, options.Acronyms);
    }



    /// <summary>
    /// Converts a string to snake case: its words in lower case with one underscore between them.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in snake case. It is empty when the string has no letters or digits.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The string is first split into words, by the rule every case conversion shares. A word is a run of
    /// letters and digits. Every other character (white space, punctuation, symbols, control characters,
    /// underscores and dashes) ends the word and is left out. An apostrophe is removed without ending the
    /// word, so <c>don't</c> is the one word <c>dont</c>.
    /// </para>
    /// <para>
    /// A word also starts at an upper-case letter that follows a lower-case letter or a digit, and at the
    /// last upper-case letter of a run that a lower-case letter follows. An acronym is therefore one word
    /// and a digit stays with the word before it: <c>SKUCode</c> is <c>SKU</c> and <c>Code</c>, and
    /// <c>Line2Qty</c> is <c>Line2</c> and <c>Qty</c>.
    /// </para>
    /// <para>
    /// Letters are cased one <see cref="char"/> at a time with the invariant culture, so the result does not
    /// depend on the current culture.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var snake = " A sample string, for processing! ".ToSnakeCase();
    /// // snake == "a_sample_string_for_processing"
    ///
    /// var fromIdentifier = "UserLoginCount".ToSnakeCase();
    /// // fromIdentifier == "user_login_count"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToSnakeCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        return JoinWords(s, '_', WordCasing.Lower);
    }



    /// <summary>
    /// Converts a string to snake case, finding its words by the rules in <paramref name="options"/>.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <param name="options">How the words of the string are found, and how an acronym is cased.</param>
    /// <returns>
    /// The specified string in snake case. It is empty when no word is found.
    /// </returns>
    /// <remarks>
    /// With a new <see cref="CaseConversionOptions"/> the result is the same as
    /// <see cref="ToSnakeCase(string)"/> gives. When <see cref="CaseConversionOptions.WordSplitter"/> is
    /// set, the words it returns are cased and joined as they are.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var options = new CaseConversionOptions { Boundaries = WordBoundaries.All &amp; ~WordBoundaries.CaseChanges };
    ///
    /// var snake = "iPhone case".ToSnakeCase(options);
    /// // snake == "iphone_case"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="CaseConversionOptions.WordSplitter"/> returned <see langword="null"/>.
    /// </exception>
    public static string ToSnakeCase(this string s, CaseConversionOptions options)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        return FormatWords(FindWords(s, options), '_', WordCasing.Lower, options.Acronyms);
    }



    /// <summary>
    /// Capitalizes each word, leaving spaces and punctuation.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in title case.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="TextInfo.ToTitleCase"/> using <see cref="CultureInfo.CurrentCulture"/>.
    /// Words that are entirely uppercase are left unchanged. This is a general-purpose implementation;
    /// proper names and addresses may require specialized handling.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var title = "a sample string for processing!".ToTitleCase();
    /// // title == "A Sample String For Processing!" (under en-US)
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToTitleCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
    }



    /// <summary>
    /// Splits a string into its words by the rules the case conversions use: a word is a run of letters
    /// and digits, every other character ends the word and is left out, an apostrophe is removed without
    /// ending the word, and a word also starts at a case boundary inside an identifier.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <returns>
    /// The words in the order they appear, with the casing they have in <paramref name="s"/>. The
    /// sequence is produced as it is enumerated.
    /// </returns>
    /// <example>
    /// <code>
    /// using System.Linq;
    /// using Wolfgang.Extensions.String;
    ///
    /// var words = "SKUCode for user_login!".SplitWords().ToArray();
    /// // words == ["SKU", "Code", "for", "user", "login"]
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static IEnumerable<string> SplitWords(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        return SplitWordsIterator(s, PunctuationHandling.Drop, WordBoundaries.All);
    }



    /// <summary>
    /// Splits a string into its words by the rules in <paramref name="options"/>.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="options">How the words of the string are found.</param>
    /// <returns>
    /// The words in the order they appear, with the casing they have in <paramref name="s"/>. When
    /// <see cref="CaseConversionOptions.WordSplitter"/> is set, the result is what that function returns,
    /// without its <see langword="null"/> and empty words.
    /// </returns>
    /// <example>
    /// <code>
    /// using System.Linq;
    /// using Wolfgang.Extensions.String;
    ///
    /// var options = new CaseConversionOptions { Punctuation = PunctuationHandling.Keep };
    ///
    /// var words = "Hello, big World!".SplitWords(options).ToArray();
    /// // words == ["Hello,", "big", "World!"]
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="CaseConversionOptions.WordSplitter"/> returned <see langword="null"/>.
    /// </exception>
    public static IEnumerable<string> SplitWords(this string s, CaseConversionOptions options)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        return WithoutEmptyWords(FindWords(s, options));
    }



    // What JoinWords does with the letters of each word.
    private enum WordCasing
    {
        // Every letter in lower case.
        Lower,

        // The first character in upper case, the rest in lower case.
        Capitalized,

        // The first word in lower case, every later word capitalized.
        CapitalizedAfterFirst,
    }



    // Passed to JoinWords as the delimiter when the words run together.
    private const char NoDelimiter = '\0';



    // The longest string whose result JoinWords builds on the stack. It covers identifiers and
    // most phrases, and its buffer (twice as many characters) is small next to a thread's stack.
    private const int LongestStringOnTheStack = 128;



    // The one place the case conversions split a string into words and put it back together.
    // The result is built on the stack when it is sure to fit, so a short string costs one
    // allocation: the result itself. A long one gets a buffer of its own.
    private static string JoinWords(string s, char delimiter, WordCasing casing)
    {
        // An apostrophe is not a boundary: "don't" is one word. Taking apostrophes out first keeps
        // the rules about what comes before and after a letter true of the word itself.
        var text = RemoveApostrophes(s);

        // Every character can gain at most one delimiter in front of it, so the buffer is twice
        // the string. The choice is made on the length of the string, not on the doubled length:
        // doubling a very long string's length overflows, and must not pick the stack.
        var output = text.Length <= LongestStringOnTheStack
            ? stackalloc char[text.Length * 2]
            : new char[text.Length * 2];

        var length = WriteWords(text, output, delimiter, casing);

        return output.Slice(0, length).ToString();
    }



    // Walks the text once and writes the result into the buffer; returns how much it wrote. A word
    // is a run of letters and digits, every other character ends the word and is left out, and a
    // word also starts at a case boundary inside an identifier. Each word is written in the casing
    // asked for, with the delimiter (if any) between words.
    private static int WriteWords(string text, Span<char> output, char delimiter, WordCasing casing)
    {
        var outputIndex = 0;

        var wordCount = 0;

        var inWord = false;

        for (var index = 0; index < text.Length; index++)
        {
            if (!IsWordCharacter(text, index))
            {
                inWord = false;
                continue;
            }

            var startsWord = !inWord || StartsWordInsideIdentifier(text, index);

            if (startsWord)
            {
                if (wordCount > 0 && delimiter != NoDelimiter)
                {
                    output[outputIndex++] = delimiter;
                }

                wordCount++;
            }

            inWord = true;

            output[outputIndex++] = startsWord && IsCapitalized(casing, wordCount)
                ? char.ToUpperInvariant(text[index])
                : char.ToLowerInvariant(text[index]);
        }

        return outputIndex;
    }



    // Whether the first character of the word with this number (counted from 1) is upper-cased.
    private static bool IsCapitalized(WordCasing casing, int wordNumber)
    {
        return casing == WordCasing.Capitalized
            || (casing == WordCasing.CapitalizedAfterFirst && wordNumber > 1);
    }



    // Takes out the typewriter apostrophe and the typographic one (RIGHT SINGLE QUOTATION MARK).
    // string.Replace compares ordinally and returns s itself when there is nothing to take out,
    // which is the usual case.
    private static string RemoveApostrophes(string s)
    {
        return s
            .Replace("'", string.Empty)
            .Replace("\u2019", string.Empty);
    }



    // True when the character at index belongs to a word: a letter, a number (a digit, or a
    // character such as a superscript two, a fraction or a Roman numeral), a combining mark (so an
    // accent written as its own character stays with its letter), or either half of a surrogate
    // pair that is a letter or a number.
    private static bool IsWordCharacter(string s, int index)
    {
        var chr = s[index];

        if (char.IsLetter(chr) || char.IsNumber(chr))
        {
            return true;
        }

        if (char.IsHighSurrogate(chr))
        {
            return index + 1 < s.Length && char.IsLowSurrogate(s[index + 1]) && IsLetterOrNumber(s, index);
        }

        if (char.IsLowSurrogate(chr))
        {
            return index > 0 && char.IsHighSurrogate(s[index - 1]) && IsLetterOrNumber(s, index - 1);
        }

        var category = char.GetUnicodeCategory(chr);

        return category == UnicodeCategory.NonSpacingMark
            || category == UnicodeCategory.SpacingCombiningMark
            || category == UnicodeCategory.EnclosingMark;
    }



    // Whether the surrogate pair that starts at index is a letter or a number.
    private static bool IsLetterOrNumber(string s, int index)
    {
        return char.IsLetter(s, index) || char.IsNumber(s, index);
    }



    // The words of s as the options ask for them: from the caller's splitter when there is one,
    // from the built-in rules otherwise. The sequence may hold null or empty words; FormatWords and
    // WithoutEmptyWords skip them.
    private static IEnumerable<string> FindWords(string s, CaseConversionOptions options)
    {
        var splitter = options.WordSplitter;

        if (splitter == null)
        {
            return SplitWordsIterator(s, options.Punctuation, options.Boundaries);
        }

        return splitter(s)
            ?? throw new InvalidOperationException("The word splitter in the options returned null. Return an empty sequence for a string that has no words.");
    }



    private static IEnumerable<string> WithoutEmptyWords(IEnumerable<string> words)
    {
        return words.Where(word => !string.IsNullOrEmpty(word));
    }



    // The built-in rules as a sequence of words. JoinWords applies the same rules in one pass
    // without building the words; a test compares the two over the same inputs.
    private static IEnumerable<string> SplitWordsIterator(string s, PunctuationHandling punctuation, WordBoundaries boundaries)
    {
        var keepPunctuation = punctuation == PunctuationHandling.Keep;

        // A kept apostrophe is punctuation like any other. A dropped one goes first, so that it does
        // not end the word it is in.
        var text = keepPunctuation ? s : RemoveApostrophes(s);

        var splitAtCaseChanges = (boundaries & WordBoundaries.CaseChanges) != WordBoundaries.None;

        var start = -1;

        for (var index = 0; index < text.Length; index++)
        {
            var inWord = IsWordCharacter(text, index) || (keepPunctuation && !IsBoundary(text[index], boundaries));

            if (!inWord)
            {
                if (start >= 0)
                {
                    yield return text.Substring(start, index - start);
                }

                start = -1;
            }
            else if (start < 0)
            {
                start = index;
            }
            else if (splitAtCaseChanges && StartsWordInsideIdentifier(text, index))
            {
                yield return text.Substring(start, index - start);

                start = index;
            }
        }

        if (start >= 0)
        {
            yield return text.Substring(start);
        }
    }



    // Whether a character that is not part of a word divides words when punctuation is kept.
    private static bool IsBoundary(char chr, WordBoundaries boundaries)
    {
        if ((boundaries & WordBoundaries.Separators) != WordBoundaries.None && char.IsSeparator(chr))
        {
            return true;
        }

        if ((boundaries & WordBoundaries.Delimiters) != WordBoundaries.None && (chr == '_' || chr == '-'))
        {
            return true;
        }

        return (boundaries & WordBoundaries.ControlCharacters) != WordBoundaries.None && char.IsControl(chr);
    }



    // Cases each word and joins the words with the delimiter, if there is one.
    private static string FormatWords(IEnumerable<string> words, char delimiter, WordCasing casing, AcronymHandling acronyms)
    {
        var output = new StringBuilder();

        var wordCount = 0;

        foreach (var word in words)
        {
            if (string.IsNullOrEmpty(word))
            {
                continue;
            }

            if (wordCount > 0 && delimiter != NoDelimiter)
            {
                output.Append(delimiter);
            }

            wordCount++;

            AppendWord(output, word, IsCapitalized(casing, wordCount), acronyms);
        }

        return output.ToString();
    }



    private static void AppendWord(StringBuilder output, string word, bool capitalize, AcronymHandling acronyms)
    {
        if (capitalize && acronyms == AcronymHandling.Preserve && IsAcronym(word))
        {
            output.Append(word);

            return;
        }

        // The character that is upper-cased is the first one that belongs to a word, so a word kept
        // with punctuation in front of it ("(hello") is still capitalized. word.Length is past the
        // end, so it marks "no character".
        var capitalizeAt = capitalize ? FirstWordCharacter(word) : word.Length;

        for (var index = 0; index < word.Length; index++)
        {
            output.Append
            (
                index == capitalizeAt
                    ? char.ToUpperInvariant(word[index])
                    : char.ToLowerInvariant(word[index])
            );
        }
    }



    // The index of the first letter, digit or mark of the word, or its length when it has none.
    private static int FirstWordCharacter(string word)
    {
        var index = 0;

        while (index < word.Length && !IsWordCharacter(word, index))
        {
            index++;
        }

        return index;
    }



    // An acronym is a word with no lower-case letter. A word of digits, or one in a script that has
    // no case, counts as well, and comes out the same either way.
    private static bool IsAcronym(string word)
    {
        return !word.Any(char.IsLower);
    }



    // True when the upper-case letter at index starts a new word without a boundary before it:
    // it follows a lower-case letter or a number ("userLogin", "Line2Qty"), or it is the last
    // letter of an upper-case run and a lower-case letter follows it ("SKUCode").
    private static bool StartsWordInsideIdentifier(string s, int index)
    {
        if (index == 0 || !char.IsUpper(s[index]))
        {
            return false;
        }

        var before = s[index - 1];

        if (char.IsLower(before) || char.IsNumber(before))
        {
            return true;
        }

        return char.IsUpper(before) && index + 1 < s.Length && char.IsLower(s[index + 1]);
    }
}
