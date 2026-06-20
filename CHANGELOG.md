# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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

## [0.1.0] - 2026-04-27

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
