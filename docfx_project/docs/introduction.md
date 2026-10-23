# Introduction

**Wolfgang.Extensions.String** is a small library of extension methods for `string`:

- **Substring helpers:** `Left` and `Right` return the first or last N characters. They clamp the length (a string shorter than N is returned unchanged) and return `null` for a `null` input.
- **Center padding:** `PadCenter` centers a string within a total width, padding both sides with spaces or a character you choose. When the padding is uneven, the extra character goes on the right.
- **Case conversions:** `ToCamelCase`, `ToKebabCase`, `ToPascalCase`, `ToSnakeCase` and `ToTitleCase`.

The package is published on NuGet as [`Wolfgang.Extensions.String`](https://www.nuget.org/packages/Wolfgang.Extensions.String) and targets .NET Framework 4.6.2, .NET Standard 2.0, .NET 8.0 and .NET 10.0.

## Behaviour worth knowing

- `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` treat the characters for which `char.IsSeparator` returns `true` (spaces and other Unicode separators) as word boundaries. Punctuation and control characters are kept. They use invariant casing, so the result does not depend on the current culture.
- `ToTitleCase` delegates to `TextInfo.ToTitleCase` of the **current** culture, and leaves words that are entirely upper case unchanged.
- `PadCenter` and the case conversions throw `ArgumentNullException` for a `null` input. `Left`, `Right` and `PadCenter` throw `ArgumentOutOfRangeException` for a negative length or width.

## Next steps

- [Getting Started](getting-started.md): install the package and call each method.
- [Building from source](setup.md): build, test and contribute.
- [API Reference](xref:Wolfgang.Extensions.String.StringExtensions): every member, generated from the XML doc comments.
