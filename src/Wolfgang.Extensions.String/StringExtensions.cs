using System;
using System.Globalization;
#if !NET8_0_OR_GREATER
using System.Text;
#endif

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
    /// The number of characters in the resulting string. If <paramref name="totalWidth"/> is less than the length of
    /// <paramref name="s"/>, the original string is returned unchanged.
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
    /// var unchanged = "Hello".PadCenter(3);     // "Hello" (already wider than totalWidth)
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
    /// The number of characters in the resulting string. If <paramref name="totalWidth"/> is less than the length of
    /// <paramref name="s"/>, the original string is returned unchanged.
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

        if (totalWidth < s.Length)
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
    /// Converts a string to camelCase: the first word is lower-cased, every later word is capitalized with the
    /// rest of its letters lower-cased, and all separator characters are removed.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in camel case.
    /// </returns>
    /// <remarks>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> are treated as word
    /// boundaries and removed from the output. Control characters and punctuation are preserved.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var camel = " A sample string for processing! ".ToCamelCase();
    /// // camel == "aSampleStringForProcessing!"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToCamelCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        var output = new char[s.Length];

        // A word starts after a separator, but only once something has been written: the first
        // word stays lower case even when the input starts with separators.
        var capitalizeNext = false;

        var outputIndex = 0;

        foreach (var chr in s)
        {
            if (char.IsSeparator(chr))
            {
                capitalizeNext = outputIndex > 0;
                // eat char
            }
            else
            {
                output[outputIndex++] = capitalizeNext
                    ? char.ToUpperInvariant(chr)
                    : char.ToLowerInvariant(chr);
                capitalizeNext = false;
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Converts a string to kebab case: every letter is lower-cased and exactly one dash is put between
    /// words, whether they were divided by separator characters, by dashes, or by the case boundaries of
    /// a PascalCase or camelCase identifier.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in kebab case.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> divide words, and so
    /// does a dash already in the string. A run of them becomes a single dash, and the result neither starts
    /// nor ends with one: <c>"  two  words  "</c> is <c>two-words</c>. Control characters and other punctuation
    /// are preserved.
    /// </para>
    /// <para>
    /// A word also starts at an upper-case letter that follows a lower-case letter or a digit, and at the
    /// last upper-case letter of a run that a lower-case letter follows. An acronym therefore stays
    /// together and a digit stays with the word before it: <c>SKUCode</c> is <c>sku-code</c> and
    /// <c>Line2Qty</c> is <c>line2-qty</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var kebab = "A sample string for processing!".ToKebabCase();
    /// // kebab == "a-sample-string-for-processing!"
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

        return ToDelimitedCase(s, '-');
    }



    /// <summary>
    /// Converts a string to PascalCase: every word, including the first one, is capitalized with the rest of its
    /// letters lower-cased, and all separator characters are removed.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in Pascal case.
    /// </returns>
    /// <remarks>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> are treated as word
    /// boundaries and removed from the output. Control characters and punctuation are preserved.
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var pascal = " A sample string for processing! ".ToPascalCase();
    /// // pascal == "ASampleStringForProcessing!"
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToPascalCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        var output = new char[s.Length];

        var isLastCharSeparator = true;

        var outputIndex = 0;

        foreach (var chr in s)
        {
            if (char.IsSeparator(chr))
            {
                isLastCharSeparator = true;
                // eat char
            }
            else if (isLastCharSeparator)
            {
                isLastCharSeparator = false;
                output[outputIndex++] = char.ToUpperInvariant(chr);
            }
            else
            {
                isLastCharSeparator = false;
                output[outputIndex++] = char.ToLowerInvariant(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Converts a string to snake case: every letter is lower-cased and exactly one underscore is put between
    /// words, whether they were divided by separator characters, by underscores, or by the case boundaries of
    /// a PascalCase or camelCase identifier.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in snake case.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> divide words, and so
    /// does a underscore already in the string. A run of them becomes a single underscore, and the result neither starts
    /// nor ends with one: <c>"  two  words  "</c> is <c>two_words</c>. Control characters and other punctuation
    /// are preserved.
    /// </para>
    /// <para>
    /// A word also starts at an upper-case letter that follows a lower-case letter or a digit, and at the
    /// last upper-case letter of a run that a lower-case letter follows. An acronym therefore stays
    /// together and a digit stays with the word before it: <c>SKUCode</c> is <c>sku_code</c> and
    /// <c>Line2Qty</c> is <c>line2_qty</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.String;
    ///
    /// var snake = "A sample string for processing!".ToSnakeCase();
    /// // snake == "a_sample_string_for_processing!"
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

        return ToDelimitedCase(s, '_');
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



    // Lower-cases s and puts exactly one delimiter between its words. Words are divided by separator
    // characters, by the delimiter itself, and by the case boundaries of a PascalCase or camelCase
    // identifier. Nothing is written for a run of dividers until the next word starts, so a run
    // becomes one delimiter and the result neither starts nor ends with one.
    private static string ToDelimitedCase(string s, char delimiter)
    {
        // Every character can gain at most one delimiter in front of it.
        var output = new char[s.Length * 2];

        var outputIndex = 0;

        var delimiterPending = false;

        for (var index = 0; index < s.Length; index++)
        {
            var chr = s[index];

            if (chr == delimiter || char.IsSeparator(chr))
            {
                delimiterPending = outputIndex > 0;
                continue;
            }

            if (delimiterPending || StartsWordInsideIdentifier(s, index))
            {
                output[outputIndex++] = delimiter;
            }

            delimiterPending = false;

            output[outputIndex++] = char.ToLowerInvariant(chr);
        }

        return new string(output, 0, outputIndex);
    }



    // True when the upper-case letter at index starts a new word without a separator before it:
    // it follows a lower-case letter or a digit ("userLogin", "Line2Qty"), or it is the last
    // letter of an upper-case run and a lower-case letter follows it ("SKUCode").
    private static bool StartsWordInsideIdentifier(string s, int index)
    {
        if (index == 0 || !char.IsUpper(s[index]))
        {
            return false;
        }

        var before = s[index - 1];

        if (char.IsLower(before) || char.IsDigit(before))
        {
            return true;
        }

        return char.IsUpper(before) && index + 1 < s.Length && char.IsLower(s[index + 1]);
    }
}
