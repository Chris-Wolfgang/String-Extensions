# Introduction

**Wolfgang.Extensions.String** is a small library of extension methods for `string`:

- **Substring helpers:** `Left` and `Right` return the first or last N characters. They clamp the length (a string shorter than N is returned unchanged) and return `null` for a `null` input.
- **Center padding:** `PadCenter` centers a string within a total width, padding both sides with spaces or a character you choose. When the padding is uneven, the extra character goes on the right.
- **Case conversions:** `ToCamelCase`, `ToKebabCase`, `ToPascalCase`, `ToSnakeCase` and `ToTitleCase`.

The package is published on NuGet as [`Wolfgang.Extensions.String`](https://www.nuget.org/packages/Wolfgang.Extensions.String) and targets .NET Framework 4.6.2, .NET Standard 2.0, .NET 8.0 and .NET 10.0.

## Behaviour worth knowing

- `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` split the string into words by one shared rule and differ only in casing and in what goes between the words. A word is a run of letters and digits; every other character (white space, punctuation, symbols, control characters, `_` and `-`) ends the word and is left out, except an apostrophe, which is removed without ending the word. A word also starts at a case boundary inside an identifier, so `"SKUCode".ToSnakeCase()` is `sku_code`, `"Line2Qty"` is `line2_qty` and `"user_login_count".ToPascalCase()` is `UserLoginCount`. They use invariant casing, so the result does not depend on the current culture.
- Each of the four has an overload that takes a `CaseConversionOptions`: keep punctuation in the words (`Punctuation`), choose what divides words (`Boundaries`), leave acronyms in upper case (`Acronyms`), or supply your own word splitter (`WordSplitter`). A new `CaseConversionOptions` holds the default rules. `SplitWords` returns the words themselves.
- `ToTitleCase` delegates to `TextInfo.ToTitleCase` of the **current** culture, and leaves words that are entirely upper case unchanged.
- Every method counts UTF-16 code units (`char` values), not what is displayed. `Left` and `Right` can split a surrogate pair (such as an emoji) or a letter from its combining mark, and `PadCenter`'s width is not a display width.
- Invariant casing leaves letters with no invariant mapping, such as `İ` (U+0130) and `ı` (U+0131), unchanged. It follows the runtime's Unicode data, so a few letters (for example Georgian Mtavruli) convert differently on .NET Framework and on .NET.
- `PadCenter` and the case conversions throw `ArgumentNullException` for a `null` input. `Left`, `Right` and `PadCenter` throw `ArgumentOutOfRangeException` for a negative length or width.

## Next steps

- [Getting Started](getting-started.md): install the package and call each method.
- [Building from source](setup.md): build, test and contribute.
- [API Reference](xref:Wolfgang.Extensions.String.StringExtensions): every member, generated from the XML doc comments.
