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
    /// Returns the left most specified character from the specified string
    /// </summary>
    /// <param name="s">The instance to retrieve from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins at index 0 and is the specified number of characters long.
    /// If the string is null, null will be returned. If the string contains fewer than the specified number of character
    /// then the entire string is returned.
    /// </returns>
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
    /// Returns a new string of a specified length in which the current string is centered with spaces padding either side
    /// </summary>
    /// <param name="s"></param>
    /// <param name="totalWidth"></param>
    /// <returns></returns>
    public static string? PadCenter(this string? s, int totalWidth)
        => PadCenter(s, totalWidth, ' ');


    /// <summary>
    /// Returns a new string of a specified length in which the current string is centered with specified character padding either side
    /// </summary>
    /// <param name="s"></param>
    /// <param name="totalWidth"></param>
    /// <param name="paddingChar"></param>
    /// <returns></returns>
    public static string? PadCenter(this string? s, int totalWidth, char paddingChar)
    {
        if (s == null) throw new NullReferenceException(nameof(s));

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
    /// Returns the right most specified character from the specified string
    /// </summary>
    /// <param name="s">The instance to retrieve from.</param>
    /// <param name="length">The number of characters to retrieve.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins the specified number of characters before the end continue to the last character.
    /// If the string is null, null will be returned. If the string contains fewer than the specified number of character
    /// then the entire string is returned.
    /// </returns>
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
    /// Makes the first letter lower case, then capitalizes each word, including the first
    /// one and removes all spaces.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in camel case
    /// </returns>
    /// <remarks>
    ///     Original: A sample string for processing!
    /// Return value: aSampleStringForProcessing
    /// </remarks>
    public static string? ToCamelCase(this string? s)
    {
        _ = s ?? throw new ArgumentNullException();

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
                output[outputIndex++] = char.ToUpper(chr);
            }
            else
            {
                isLastCharSeparator = false;
                foundFirstChar = true;
                output[outputIndex++] = char.ToLower(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Converts all letter to lower case and replaces spaces with dashes.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in title case
    /// </returns>
    /// <remarks>
    ///     Original: A sample string for processing!
    /// Return value: a-sample-string-for-processing
    /// </remarks>
    public static string? ToKebabCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException();

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
                output[outputIndex++] = char.ToLower(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Capitalizes each word, including the first one and removes all spaces
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in pascal case
    /// </returns>
    /// <remarks>
    ///     Original: A sample string for processing!
    /// Return value: ASampleStringForProcessing
    /// </remarks>
    public static string? ToPascalCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException();

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
                output[outputIndex++] = char.ToUpper(chr);
            }
            else
            {
                isLastCharSeparator = false;
                output[outputIndex++] = char.ToLower(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Converts all letter to lower case and replaces spaces with underscores.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in title case
    /// </returns>
    /// <remarks>
    ///     Original: A sample string for processing!
    /// Return value: a_sample_string_for_processing
    /// </remarks>
    public static string? ToSnakeCase(this string s)
    {
        _ = s ?? throw new ArgumentNullException();

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
                output[outputIndex++] = char.ToLower(chr);
            }
        }

        return new string(output, 0, outputIndex);
    }



    /// <summary>
    /// Capitalizes each word, leaving spaces and punctuation.
    /// </summary>
    /// <param name="s">The string to convert.</param>
    /// <returns>
    /// The specified string in title case
    /// </returns>
    /// <remarks>
    ///     Original: A sample string for processing!
    /// Return value: A Sample String For Processing!
    /// </remarks>
    public static string? ToTitleCase(this string s)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
    }

}