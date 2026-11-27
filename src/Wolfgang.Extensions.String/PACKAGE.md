# Wolfgang.Extensions.String

A collection of extension methods for `string`: substring helpers (`Left` / `Right`), center-padding (`PadCenter`), word splitting (`SplitWords`) and case conversions (`ToCamelCase` / `ToKebabCase` / `ToPascalCase` / `ToSnakeCase` / `ToTitleCase`).

[![NuGet](https://img.shields.io/nuget/v/Wolfgang.Extensions.String.svg?logo=nuget&label=NuGet)](https://www.nuget.org/packages/Wolfgang.Extensions.String)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Chris-Wolfgang/String-Extensions/blob/main/LICENSE)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github)](https://github.com/Chris-Wolfgang/String-Extensions)

## Installation

```bash
dotnet add package Wolfgang.Extensions.String
```

## Quick start

```csharp
using System;
using Wolfgang.Extensions.String;

// Substring helpers (null-tolerant, length-clamped)
var path = "/usr/local/bin/app";
var first5 = path.Left(5);    // "/usr/"
var last3 = path.Right(3);    // "app"

// Centered formatting
Console.WriteLine($"{"Name".PadCenter(10)}|{"Date".PadCenter(12)}");

// Case conversions
var input = "user login count";
Console.WriteLine(input.ToCamelCase());   // "userLoginCount"
Console.WriteLine(input.ToKebabCase());   // "user-login-count"
Console.WriteLine(input.ToPascalCase());  // "UserLoginCount"
Console.WriteLine(input.ToSnakeCase());   // "user_login_count"
Console.WriteLine(input.ToTitleCase());   // "User Login Count"
```

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Left(this string? s, int length)` | `string?` | The leftmost `length` characters; the whole string when it is shorter; `null` for a `null` input. |
| `Right(this string? s, int length)` | `string?` | The rightmost `length` characters; the whole string when it is shorter; `null` for a `null` input. |
| `PadCenter(this string, int totalWidth)` | `string` | Centers the string within `totalWidth`, padding both sides with spaces; the extra character goes on the right. |
| `PadCenter(this string, int totalWidth, char paddingChar)` | `string` | As above, padding with `paddingChar`. |
| `ToCamelCase(this string)` | `string` | `camelCase`: `"User login-count"` is `userLoginCount`. |
| `ToKebabCase(this string)` | `string` | `kebab-case`: `"UserLoginCount"` is `user-login-count`. |
| `ToPascalCase(this string)` | `string` | `PascalCase`: `"user_login_count"` is `UserLoginCount`. |
| `ToSnakeCase(this string)` | `string` | `snake_case`: `"UserLoginCount"` is `user_login_count`. |
| `ToTitleCase(this string)` | `string` | `Title Case` with the current culture's `TextInfo.ToTitleCase`. |
| `ToCamelCase` / `ToKebabCase` / `ToPascalCase` / `ToSnakeCase(this string, CaseConversionOptions)` | `string` | The same conversion with the word rules taken from the options. |
| `SplitWords(this string)` / `SplitWords(this string, CaseConversionOptions)` | `IEnumerable<string>` | The words of the string by the rules the conversions use. |

`Left`, `Right` and `PadCenter` throw `ArgumentOutOfRangeException` for a negative length or width. `PadCenter` and the `ToXxxCase` methods throw `ArgumentNullException` for a `null` input.

## Learn more

- [API documentation](https://Chris-Wolfgang.github.io/String-Extensions/)
- [README, with the word rules and the analyzers used](https://github.com/Chris-Wolfgang/String-Extensions#readme)
- [CHANGELOG](https://github.com/Chris-Wolfgang/String-Extensions/blob/main/CHANGELOG.md)
- [Verifying a release (SBOM, provenance, reproducible build)](https://github.com/Chris-Wolfgang/String-Extensions/blob/main/SECURITY.md#verifying-a-release)
- [License (MIT)](https://github.com/Chris-Wolfgang/String-Extensions/blob/main/LICENSE)
