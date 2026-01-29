# Getting Started

This guide will help you quickly get up and running with the String Extensions library in your .NET projects.

## Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8.0 SDK** or later
- A compatible IDE (Visual Studio 2022, VS Code, or Rider)
- Basic knowledge of C# and .NET

## Installation

### Via NuGet Package Manager

You can install String Extensions using the NuGet Package Manager in Visual Studio:

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages"
3. Search for "String-Extensions"
4. Click "Install"

### Via .NET CLI

Alternatively, you can install the package using the .NET CLI:

```bash
dotnet add package String-Extensions
```

### Via Package Manager Console

Or use the Package Manager Console in Visual Studio:

```powershell
Install-Package String-Extensions
```

## Basic Usage

Once installed, you can start using the extension methods immediately. Here's a simple example:

```csharp
using StringExtensions;

// Example usage
string text = "  Hello World  ";
string result = text.TrimAndLower(); // "hello world"

// Check if string is null or empty
if (text.IsNullOrEmpty())
{
    Console.WriteLine("String is empty");
}

// Truncate with ellipsis
string longText = "This is a very long string that needs to be truncated";
string truncated = longText.Truncate(20); // "This is a very long..."
```

## Common Use Cases

### String Validation

```csharp
string email = "user@example.com";
bool isValid = email.IsValidEmail();

string url = "https://example.com";
bool isValidUrl = url.IsValidUrl();
```

### String Transformation

```csharp
string text = "hello_world";
string camelCase = text.ToCamelCase(); // "helloWorld"
string pascalCase = text.ToPascalCase(); // "HelloWorld"
```

### String Parsing

```csharp
string number = "123.45";
double? value = number.ToDoubleOrNull(); // 123.45

string date = "2024-01-15";
DateTime? parsed = date.ToDateTimeOrNull(); // DateTime object or null
```

## Next Steps

Now that you have the library installed and understand basic usage, you can:

1. Explore the [API Documentation](../api/index.md) for a complete list of available methods
2. Check out the [Setup Guide](setup.md) for advanced configuration options
3. Review the [Examples](../examples/index.md) for more complex scenarios

## Getting Help

If you encounter any issues or have questions:

- Check the [API Documentation](../api/index.md)
- Visit the [GitHub Repository](https://github.com/Chris-Wolfgang/String-Extensions)
- Open an issue on GitHub for bug reports or feature requests

## What's Next?

Continue to the [Setup Guide](setup.md) to learn about advanced configuration and customization options.