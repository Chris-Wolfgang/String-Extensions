# Wolfgang.Extensions.String

A collection of extension methods for `string` — substring helpers (`Left` / `Right`), center-padding (`PadCenter`), and case conversions (`ToCamelCase` / `ToKebabCase` / `ToPascalCase` / `ToSnakeCase` / `ToTitleCase`).

[![NuGet](https://img.shields.io/nuget/v/Wolfgang.Extensions.String.svg?logo=nuget&label=NuGet)](https://www.nuget.org/packages/Wolfgang.Extensions.String)
[![NuGet downloads](https://img.shields.io/nuget/dt/Wolfgang.Extensions.String.svg?logo=nuget&label=downloads)](https://www.nuget.org/packages/Wolfgang.Extensions.String)
[![PR build](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/String-Extensions/pr.yaml?event=pull_request_target&label=PR%20build&logo=github)](https://github.com/Chris-Wolfgang/String-Extensions/actions/workflows/pr.yaml)
[![Release](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/String-Extensions/release.yaml?label=release&logo=github)](https://github.com/Chris-Wolfgang/String-Extensions/actions/workflows/release.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Multi--Targeted-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github)](https://github.com/Chris-Wolfgang/String-Extensions)

---

## 📦 Installation

```bash
dotnet add package Wolfgang.Extensions.String
```

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📚 Documentation

- **GitHub Repository:** [https://github.com/Chris-Wolfgang/String-Extensions](https://github.com/Chris-Wolfgang/String-Extensions)
- **API Documentation:** [https://Chris-Wolfgang.github.io/String-Extensions/](https://Chris-Wolfgang.github.io/String-Extensions/)
- **CHANGELOG:** [CHANGELOG.md](CHANGELOG.md)
- **Contributing Guide:** [CONTRIBUTING.md](CONTRIBUTING.md)
- **Formatting Guide:** [docs/README-FORMATTING.md](docs/README-FORMATTING.md)
- **Release Workflow Setup:** [docs/RELEASE-WORKFLOW-SETUP.md](docs/RELEASE-WORKFLOW-SETUP.md)
- **Workflow Security:** [docs/WORKFLOW_SECURITY.md](docs/WORKFLOW_SECURITY.md)

---

## 🚀 Quick Start

```csharp
using System;
using Wolfgang.Extensions.String;

// Substring helpers (null-tolerant, length-clamped)
var path = "/usr/local/bin/app";
var first5 = path.Left(5);    // "/usr/"
var last3 = path.Right(3);    // "app"

// Centered formatting
Console.WriteLine($"{"Name".PadCenter(10)}|{"Date".PadCenter(12)}");
Console.WriteLine("----------+------------");
Console.WriteLine($"{"Steve",-10}|{"04/13/2026".PadCenter(12)}");

// Case conversions
var input = "user login count";
Console.WriteLine(input.ToCamelCase());   // "userLoginCount"
Console.WriteLine(input.ToKebabCase());   // "user-login-count"
Console.WriteLine(input.ToPascalCase());  // "UserLoginCount"
Console.WriteLine(input.ToSnakeCase());   // "user_login_count"
Console.WriteLine(input.ToTitleCase());   // "User Login Count"
```

---

## ✨ Features

The table below is a snapshot of the public API at the time of writing. For the
authoritative list (kept in sync with source on every release), see the
[API documentation](https://Chris-Wolfgang.github.io/String-Extensions/api/Wolfgang.Extensions.String.StringExtensions.html).

### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Left(this string, int length)` | `string` | Returns the leftmost `length` characters. Returns the whole string when it is shorter than `length`; returns `null` for a `null` input. |
| `Right(this string, int length)` | `string` | Returns the rightmost `length` characters. Returns the whole string when it is shorter than `length`; returns `null` for a `null` input. |
| `PadCenter(this string, int totalWidth)` | `string` | Centers the string within `totalWidth`, padding both sides with spaces. When padding is uneven the extra character goes on the right. Returns the original string when it is already at least `totalWidth` long. |
| `PadCenter(this string, int totalWidth, char paddingChar)` | `string` | As above, padding with `paddingChar` instead of spaces. |
| `ToCamelCase(this string)` | `string` | Converts to `camelCase`. Separator characters are treated as word boundaries and removed. |
| `ToKebabCase(this string)` | `string` | Converts to `kebab-case`. Separator characters become dashes. |
| `ToPascalCase(this string)` | `string` | Converts to `PascalCase`. Separator characters are treated as word boundaries and removed. |
| `ToSnakeCase(this string)` | `string` | Converts to `snake_case`. Separator characters become underscores. |
| `ToTitleCase(this string)` | `string` | Converts to `Title Case`, preserving spaces and punctuation. Delegates to `TextInfo.ToTitleCase` using the current culture. |

`Left` / `Right` are length-clamping and null-tolerant. `PadCenter` and the `ToXxxCase` methods throw `ArgumentNullException` for a `null` input; `PadCenter` throws `ArgumentOutOfRangeException` for a negative width. Separator handling uses `char.IsSeparator`, so control characters and punctuation are preserved.

### Examples

```csharp
// Substring helpers
"A sample string".Left(8);    // "A sample"
"Hello".Left(10);             // "Hello"   (shorter than requested → unchanged)
"Hello".Left(0);              // ""

"A sample string".Right(6);   // "string"
"Hello".Right(10);            // "Hello"
"Hello".Right(0);             // ""

// Center padding
"Hello".PadCenter(11);        // "   Hello   "
"Hello".PadCenter(11, '-');   // "---Hello---"
"Hello".PadCenter(3);         // "Hello"   (already wide enough → unchanged)

// Case conversions
"A sample string for processing".ToCamelCase();   // "aSampleStringForProcessing"
"A sample string for processing".ToKebabCase();   // "a-sample-string-for-processing"
"A sample string for processing".ToPascalCase();  // "ASampleStringForProcessing"
"A sample string for processing".ToSnakeCase();   // "a_sample_string_for_processing"
"a sample string for processing".ToTitleCase();   // "A Sample String For Processing"
```

> **Note:** `ToTitleCase` is a general-purpose implementation that delegates to `TextInfo.ToTitleCase`. Proper names, addresses, and other domain-specific text often require specialized handling (e.g., "McDonald", "van der Berg", "O'Brien") that is not provided here.

---

## 🎯 Target Frameworks

| Framework | Versions |
|-----------|----------|
| .NET Framework | .NET Framework 4.6.2 |
| .NET Standard | .NET Standard 2.0 |
| .NET | .NET 8.0, .NET 10.0 |

---

## 🔍 Code Quality & Static Analysis

This project enforces **strict code quality standards** through **8 analyzer rule sets** (7 explicit `PackageReference`s plus the .NET SDK's built-in `Microsoft.CodeAnalysis.NetAnalyzers`), a `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` Release gate, and a banned-API ruleset:

### Analyzers in Use

1. **Microsoft.CodeAnalysis.NetAnalyzers** — Built-in .NET analyzers for correctness and performance
2. **Roslynator.Analyzers** — Advanced refactoring and code quality rules
3. **AsyncFixer** — Async/await best practices and anti-pattern detection
4. **Microsoft.VisualStudio.Threading.Analyzers** — Thread safety and async patterns
5. **Microsoft.CodeAnalysis.BannedApiAnalyzers** — Prevents usage of banned synchronous APIs (see `BannedSymbols.txt`)
6. **Meziantou.Analyzer** — Comprehensive code quality rules
7. **SonarAnalyzer.CSharp** — Industry-standard code analysis
8. **Microsoft.CodeAnalysis.PublicApiAnalyzers** — Tracks the public API surface via `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt`; surfaces additions/removals at compile time as a breaking-change review gate

---

## 🛠️ Building from Source

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) — required for the modern build / test / pack flow used by CI
- Optional: [PowerShell Core](https://github.com/PowerShell/PowerShell) for formatting scripts

### Build Steps

```bash
# Clone the repository
git clone https://github.com/Chris-Wolfgang/String-Extensions.git
cd String-Extensions

# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Run tests
dotnet test --configuration Release

# Run the benchmarks (optional)
dotnet run -c Release --project benchmarks/Wolfgang.Extensions.String.Benchmarks -- --filter '*'

# Run code formatting (PowerShell Core)
pwsh ./scripts/format.ps1
```

### Code Formatting

This project uses `.editorconfig` and `dotnet format`:

```bash
# Format code
dotnet format

# Verify formatting (as CI does)
dotnet format --verify-no-changes
```

See [docs/README-FORMATTING.md](docs/README-FORMATTING.md) for detailed formatting guidelines.

---

## 🤝 Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for:
- Code quality standards
- Build and test instructions
- Pull request guidelines
- Analyzer configuration details
