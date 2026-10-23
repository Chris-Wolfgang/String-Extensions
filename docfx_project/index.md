---
_layout: landing
---

# Wolfgang.Extensions.String

A collection of extension methods for `string` targeting .NET Framework 4.6.2, .NET Standard 2.0, .NET 8.0, and .NET 10.0.

## Installation

```bash
dotnet add package Wolfgang.Extensions.String
```

## Methods at a glance

| Method | Purpose |
|---|---|
| `Left(int)` | Leftmost N characters |
| `Right(int)` | Rightmost N characters |
| `PadCenter(int)` | Center within a width, padding both sides with spaces |
| `PadCenter(int, char)` | Center within a width, padding both sides with the given character |
| `ToCamelCase()` | Convert to `camelCase` |
| `ToKebabCase()` | Convert to `kebab-case` |
| `ToPascalCase()` | Convert to `PascalCase` |
| `ToSnakeCase()` | Convert to `snake_case` |
| `ToTitleCase()` | Convert to `Title Case` (current culture) |

## Quick examples

```csharp
"A sample string".Left(8);         // "A sample"
"A sample string".Right(6);        // "string"
"Hello".PadCenter(11, '-');        // "---Hello---"
"hello world".ToPascalCase();      // "HelloWorld"
"Hello World".ToSnakeCase();       // "hello_world"
"hello world".ToTitleCase();       // "Hello World"
```

## Where to next

- **[Documentation](docs/introduction.html)** — concepts, setup, and getting-started walkthrough
- **[API Reference](api/index.html)** — full member listing generated from XML doc comments
- **[GitHub repository](https://github.com/Chris-Wolfgang/String-Extensions)** — source, issues, releases
