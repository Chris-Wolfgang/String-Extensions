# Wolfgang.Extensions.String

A collection of extension methods for strings targeting .NET Framework 4.6.2, .NET Standard 2.0, .NET 8.0, and .NET 10.0.

## Installation

```bash
dotnet add package Wolfgang.Extensions.String
```

## Methods

### Left

Returns the leftmost characters from a string.

```csharp
"A sample string".Left(8);   // "A sample"
"Hello".Left(10);             // "Hello"
"Hello".Left(0);              // ""
```

### Right

Returns the rightmost characters from a string.

```csharp
"A sample string".Right(6);  // "string"
"Hello".Right(10);            // "Hello"
"Hello".Right(0);             // ""
```

### PadCenter

Centers a string within a specified width, padding both sides with spaces or a specified character.

```csharp
"Hello".PadCenter(11);        // "   Hello   "
"Hello".PadCenter(11, '-');   // "---Hello---"
"Hello".PadCenter(3);         // "Hello"
```

### ToCamelCase

Converts a string to camelCase.

```csharp
// Input                              → Output
"A sample string for processing"      → "aSampleStringForProcessing"
"hello world"                         → "helloWorld"
```

### ToKebabCase

Converts a string to kebab-case.

```csharp
// Input                              → Output
"A sample string for processing"      → "a-sample-string-for-processing"
"Hello World"                         → "hello-world"
```

### ToPascalCase

Converts a string to PascalCase.

```csharp
// Input                              → Output
"A sample string for processing"      → "ASampleStringForProcessing"
"hello world"                         → "HelloWorld"
```

### ToSnakeCase

Converts a string to snake_case.

```csharp
// Input                              → Output
"A sample string for processing"      → "a_sample_string_for_processing"
"Hello World"                         → "hello_world"
```

### ToTitleCase

Capitalizes the first letter of each word, preserving spaces and punctuation.

```csharp
// Input                              → Output
"a sample string for processing"      → "A Sample String For Processing"
"hello world"                         → "Hello World"
```

> **Note:** This is a general-purpose implementation that delegates to `TextInfo.ToTitleCase`. Proper names, addresses, and other domain-specific text often require specialized handling (e.g., "McDonald", "van der Berg", "O'Brien") that is not provided here.

## Usage Example

```csharp
using Wolfgang.Extensions.String;

// Substring helpers
var path = "/usr/local/bin/app";
var first5 = path.Left(5);   // "/usr/"
var last3 = path.Right(3);   // "app"

// Formatted table with centered headers
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

## Documentation

- [API Reference](https://chris-wolfgang.github.io/String-Extensions/api/)
- [Getting Started](https://chris-wolfgang.github.io/String-Extensions/docs/getting-started.html)
- [Introduction](https://chris-wolfgang.github.io/String-Extensions/docs/introduction.html)

## License

This project is licensed under the [MIT License](LICENSE).
