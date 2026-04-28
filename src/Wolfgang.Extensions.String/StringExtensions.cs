using System;
using System.Globalization;
using System.Text;

namespace Wolfgang.Extensions.String;

/// <summary>
/// A collection of extension methods for the <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{


    /// <summary>
    /// Returns the left most specified character from the specified string.
    /// </summary>
    /// <param name="s">The instance to retrieve from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins at index 0 and is the specified number of characters long.
    /// If the string is null, null will be returned. If the string contains fewer than the specified number of characters
    /// then the entire string is returned.
    /// </returns>
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

        return s.Length <= length ? s : s.Substring(0, length);
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
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="totalWidth"/> is less than zero.
    /// </exception>
    public static string PadCenter(this string? s, int totalWidth)
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
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="totalWidth"/> is less than zero.
    /// </exception>
    public static string PadCenter(this string? s, int totalWidth, char paddingChar)
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
        var rightPadding = totalWidth - s.Length - leftPadding;
        var buffer = new StringBuilder(totalWidth);
        buffer.Append(new string(paddingChar, leftPadding));
        buffer.Append(s);
        buffer.Append(new string(paddingChar, rightPadding));

        return buffer.ToString();
    }



    /// <summary>
    /// Returns the right most specified character from the specified string.
    /// </summary>
    /// <param name="s">The instance to retrieve from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins the specified number of characters before the end and continues to the last character.
    /// If the string is null, null will be returned. If the string contains fewer than the specified number of characters
    /// then the entire string is returned.
    /// </returns>
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

        if (s.Length <= length)
        {
            return s;
        }

        var startIndex = s.Length - length;

        return s.Substring(startIndex);
    }



    /// <summary>
    /// Converts a string to camelCase by lowering the first character, capitalizing the first letter
    /// of each subsequent word, and removing all separator characters.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in camel case.
    /// </returns>
    /// <remarks>
    /// <example>
    /// <code>
    ///     Original: " A sample string for processing! "
    /// Return value: aSampleStringForProcessing!
    /// </code>
    /// </example>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> are treated as word
    /// boundaries and removed from the output. Control characters and punctuation are preserved.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToCamelCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        var output = new char[s.Length];

        var isLastCharSeparator = false;
        var foundFirstChar = false;

        var outputIndex = 0;

        foreach (var chr in s)
        {
            if (char.IsSeparator(chr))
            {
                isLastCharSeparator = true;
                // eat char
            }
            else if (isLastCharSeparator && foundFirstChar)
            {
                isLastCharSeparator = false;
                foundFirstChar = true;
                output[outputIndex++] = char.ToUpperInvariant(chr);
            }
            else
            {
                isLastCharSeparator = false;
                foundFirstChar = true;
                output[outputIndex++] = char.ToLowerInvariant(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Converts all letters to lower case and replaces separator characters with dashes.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in kebab case.
    /// </returns>
    /// <remarks>
    /// <example>
    /// <code>
    ///     Original: "A sample string for processing!"
    /// Return value: a-sample-string-for-processing!
    /// </code>
    /// </example>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> are replaced with dashes.
    /// Control characters and punctuation are preserved.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToKebabCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        var output = new char[s.Length];

        var outputIndex = 0;

        foreach (var chr in s)
        {
            if (char.IsSeparator(chr))
            {
                output[outputIndex++] = '-';
            }
            else
            {
                output[outputIndex++] = char.ToLowerInvariant(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Capitalizes each word, including the first one, and removes all separator characters.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in Pascal case.
    /// </returns>
    /// <remarks>
    /// <example>
    /// <code>
    ///     Original: " A sample string for processing! "
    /// Return value: ASampleStringForProcessing!
    /// </code>
    /// </example>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> are treated as word
    /// boundaries and removed from the output. Control characters and punctuation are preserved.
    /// </remarks>
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
    /// Converts all letters to lower case and replaces separator characters with underscores.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in snake case.
    /// </returns>
    /// <remarks>
    /// <example>
    /// <code>
    ///     Original: "A sample string for processing!"
    /// Return value: a_sample_string_for_processing!
    /// </code>
    /// </example>
    /// Characters where <see cref="char.IsSeparator(char)"/> returns <see langword="true"/> are replaced with underscores.
    /// Control characters and punctuation are preserved.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToSnakeCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException(nameof(s));

        var output = new char[s.Length];

        var outputIndex = 0;

        foreach (var chr in s)
        {
            if (char.IsSeparator(chr))
            {
                output[outputIndex++] = '_';
            }
            else
            {
                output[outputIndex++] = char.ToLowerInvariant(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Capitalizes each word, leaving spaces and punctuation.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in title case.
    /// </returns>
    /// <remarks>
    /// <example>
    /// <code>
    ///     Original: "a sample string for processing!"
    /// Return value: A Sample String For Processing!
    /// </code>
    /// </example>
    /// Delegates to <see cref="TextInfo.ToTitleCase"/> using <see cref="CultureInfo.CurrentCulture"/>.
    /// Words that are entirely uppercase are left unchanged. This is a general-purpose implementation;
    /// proper names and addresses may require specialized handling.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="s"/> is <see langword="null"/>.
    /// </exception>
    public static string ToTitleCase(this string s)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
    }

}
