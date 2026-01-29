# String Extensions Library

A comprehensive collection of extension methods for .NET string manipulation that makes working with strings more intuitive and efficient.

## Overview

String Extensions is a powerful .NET library designed to simplify common string operations through extension methods. Built for .NET 8.0 and beyond, it provides developers with a rich set of tools for string manipulation, validation, transformation, and parsing.

## Features

### String Validation
- Email validation
- URL validation
- Phone number validation
- Credit card number validation
- Empty/null checks with semantic methods

### String Transformation
- Case conversions (camelCase, PascalCase, snake_case, kebab-case)
- Truncation with ellipsis
- Padding and alignment
- Remove extra whitespace
- Reverse strings
- String encoding conversions

### String Parsing
- Safe parsing to numeric types with null fallback
- DateTime parsing with culture support
- Boolean parsing with flexible input
- Custom type parsing

### String Formatting
- Template-based formatting
- Number formatting
- Currency formatting
- Date/time formatting
- Custom format providers

### String Manipulation
- Substring operations with safety checks
- String replacement with multiple options
- Character removal and filtering
- String splitting with advanced options
- String joining with separators

## Installation

Install the String Extensions library via NuGet:

```bash
dotnet add package String-Extensions
```

Or via the Package Manager Console:

```powershell
Install-Package String-Extensions
```

## Quick Start

Add the using directive to your C# file:

```csharp
using StringExtensions;
```

Start using the extension methods:

```csharp
// Validation
if ("user@example.com".IsValidEmail())
{
    Console.WriteLine("Valid email!");
}

// Transformation
string result = "hello_world".ToPascalCase(); // "HelloWorld"

// Parsing
int? number = "123".ToIntOrNull(); // 123

// Truncation
string text = "Long text here".Truncate(10); // "Long text..."
```

## Documentation Structure

This documentation is organized into the following sections:

### [Introduction](introduction.md)
Learn about the String Extensions library, its features, and why you should use it.

### [Getting Started](getting-started.md)
Quick start guide to install and begin using the library in your projects.

### [Setup](setup.md)
Detailed setup and configuration instructions for advanced scenarios.

### [API Reference](../api/index.md)
Complete API documentation for all available extension methods.

## Requirements

- .NET 8.0 or later
- C# 12.0 or later

## Supported Platforms

String Extensions supports all platforms that .NET 8.0 supports:

- Windows
- Linux
- macOS
- Docker containers
- Cloud platforms (Azure, AWS, GCP)

## Performance

String Extensions is designed with performance in mind:

- Minimal allocations where possible
- Optimized algorithms for common operations
- Benchmarked against standard .NET methods
- No reflection usage in hot paths

## Contributing

We welcome contributions! Please see our [Contributing Guide](../../CONTRIBUTING.md) for details on:

- Code of conduct
- Development workflow
- Submitting pull requests
- Coding standards
- Testing requirements

## License

String Extensions is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.

## Support

- **GitHub Issues**: [Report bugs or request features](https://github.com/Chris-Wolfgang/String-Extensions/issues)
- **Discussions**: [Ask questions and share ideas](https://github.com/Chris-Wolfgang/String-Extensions/discussions)
- **Documentation**: You're reading it!

## Roadmap

Planned features for future releases:

- Regular expression extensions
- String comparison utilities
- Localization helpers
- Advanced pattern matching
- String encryption/decryption helpers

## Acknowledgments

String Extensions is built and maintained by the community. Special thanks to all contributors who have helped make this library better.

## Related Projects

- [Microsoft String Extensions](https://www.nuget.org/packages/Microsoft.Extensions.Primitives/)
- [Humanizer](https://github.com/Humanizr/Humanizer)
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)

## Stay Connected

- Star the repository on [GitHub](https://github.com/Chris-Wolfgang/String-Extensions)
- Follow for updates
- Share with your developer community

---

**Ready to get started?** Head over to the [Getting Started](getting-started.md) guide!
