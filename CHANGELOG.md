# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.0] - 2026-11-25

### Breaking changes

- `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` now split a string into words by one shared rule: a word is a run of letters and digits, and every other character (white space, punctuation, symbols, control characters, `_` and `-`) is a word boundary that no longer appears in the result. `"hello, world!".ToSnakeCase()` returns `hello_world`, not `hello,_world!`; `"user_login_count".ToPascalCase()` returns `UserLoginCount`; `"(hello) world".ToPascalCase()` returns `HelloWorld`. An apostrophe is removed without ending the word. (#377)

### Added

- The `net8.0` and `net10.0` assets are marked trimmable and Native-AOT compatible (`IsTrimmable` / `IsAotCompatible`), so trimmed and AOT-published apps use the package without trim warnings. (#285)
- Add `CaseConversionOptions` and an overload of `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` that takes it: keep punctuation in the words, choose what divides words, leave acronyms in upper case, or supply your own word splitter. Add `SplitWords`, which returns the words of a string by the same rules. Setting an option to a value that is not a member of its enum throws `ArgumentOutOfRangeException`. (#377)
- `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` no longer allocate a temporary array for a string of up to 128 characters: the only allocation is the result. The .NET Framework 4.6.2 and .NET Standard 2.0 builds now depend on the `System.Memory` package; the .NET 8 and .NET 10 builds still have no dependencies. (#377)

### Fixed

- `ToCamelCase` and `ToPascalCase` now keep the words of a PascalCase or camelCase identifier apart: `"userLoginCount".ToPascalCase()` returns `UserLoginCount`, not `Userlogincount`, and `"UserLoginCount".ToCamelCase()` returns `userLoginCount`. An acronym becomes one word (`SKUCode` is `SkuCode`). (#377)
- `ToSnakeCase` and `ToKebabCase` now split a PascalCase or camelCase identifier into its words and put exactly one delimiter between words: `"UserLoginCount".ToSnakeCase()` returns `user_login_count`, not `userlogincount`, and `"  two  words  "` returns `two_words`, not `__two__words__`. Acronyms stay together (`SKUCode` is `sku_code`) and a digit stays with the word before it (`Line2Qty` is `line2_qty`). (#377)
- The package no longer ships a 4.3 MB `icon.ico` as a content file and no longer embeds it in every assembly, cutting the package from about 8.7 MB to under 1 MB. (#363)
- The NuGet package no longer asks you to accept its MIT licence when you install it. (#315)
- `ToTitleCase` now throws `ArgumentNullException` with `ParamName` `s` (it used to come from `TextInfo` as `str`), and `PadCenter` declares its receiver as non-nullable `string`, so passing a maybe-null string gives a compiler warning instead of only a runtime exception. (#278)

### Documentation

- Every public method has a C# `<example>` in its XML docs, and a test project compiles each example against the library so the examples cannot drift from the API. (#288)
- The XML docs of `Left` / `Right` say they return the leftmost / rightmost `length` characters, the `ToCamelCase` / `ToPascalCase` summaries say the rest of every word is lower-cased, and each case conversion explains its separator handling before its example. (#280)

### Internal

- Turn off implicit and global usings in the multi-target library and test projects; every file now states its own `using` directives, so each one is needed on every target framework. (#233)
- Every pack validates the package against the last published version (0.1.1) with PackageValidation, so an unintended binary-breaking change fails the build instead of reaching NuGet. (#284)
- `PadCenter` allocates only its result string on .NET 8 and later, and no longer allocates two temporary padding strings on the older targets; the result is unchanged. (#279)
- The library project builds with the repository-wide `latestMajor` language version instead of a pinned `LangVersion` 14 (identical on the .NET 10 SDK, so the compiled package does not change). (#277)
- Add the missing nullability annotations to the declared public API baseline (`PublicAPI.Shipped.txt`); the public surface is unchanged. (#230)

## [0.1.1] - 2026-06-20

Canonical maintenance round + binding-stability fix. No public API or runtime
behavior change vs v0.1.0.

### Changed

- Pinned `AssemblyVersion` to `1.0.0.0` so consumers no longer need to recompile
  or add binding redirects on patch/minor updates. `FileVersion` and
  `InformationalVersion` continue to track the package `Version`.
- Internal: adopted the canonical analyzer set and `TreatWarningsAsErrors` Release
  gate via `Directory.Build.props`; added public-API surface tracking
  (`Microsoft.CodeAnalysis.PublicApiAnalyzers`) as a breaking-change gate.
- Internal: standardized the README to the canonical structure with status badges.

### Added

- BenchmarkDotNet baseline project and a curated `benchmarks/.editorconfig`.
- Stryker.NET mutation-testing configuration.
- DocFX documentation version picker wired into the docs site.

## [0.1.0] - 2026-04-28

Initial release.

### Added

- Substring helpers: `Left` and `Right` (length-clamping, null-tolerant).
- `PadCenter(totalWidth)` and `PadCenter(totalWidth, paddingChar)` — center a
  string within a width, padding both sides.
- Case conversions: `ToCamelCase`, `ToKebabCase`, `ToPascalCase`, `ToSnakeCase`,
  and `ToTitleCase`.
- Multi-targeting: .NET Framework 4.6.2, .NET Standard 2.0, .NET 8.0, .NET 10.0.

[Unreleased]: https://github.com/Chris-Wolfgang/String-Extensions/compare/v0.1.1...HEAD
[0.1.1]: https://github.com/Chris-Wolfgang/String-Extensions/compare/v0.1.0...v0.1.1
[0.1.0]: https://github.com/Chris-Wolfgang/String-Extensions/releases/tag/v0.1.0
