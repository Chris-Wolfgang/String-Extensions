# Getting Started

## Install

```bash
dotnet add package Wolfgang.Extensions.String
```

Or from the Package Manager Console:

```powershell
Install-Package Wolfgang.Extensions.String
```

In Visual Studio, search for `Wolfgang.Extensions.String` in **Manage NuGet Packages**.

## Use

Add the namespace, then call the methods on any `string`:

```csharp
using Wolfgang.Extensions.String;

// Substring helpers (null-tolerant, length-clamped)
"A sample string".Left(8);    // "A sample"
"Hello".Left(10);             // "Hello"
"Hello".Left(0);              // ""
"A sample string".Right(6);   // "string"

// Center padding
"Hello".PadCenter(11);        // "   Hello   "
"Hello".PadCenter(11, '-');   // "---Hello---"
"Hello".PadCenter(3);         // "Hello" (already wide enough)

// Case conversions
"A sample string for processing".ToCamelCase();   // "aSampleStringForProcessing"
"A sample string for processing".ToKebabCase();   // "a-sample-string-for-processing"
"A sample string for processing".ToPascalCase();  // "ASampleStringForProcessing"
"A sample string for processing".ToSnakeCase();   // "a_sample_string_for_processing"
"a sample string for processing".ToTitleCase();   // "A Sample String For Processing"
```

## Supported frameworks

| Target | Version |
|---|---|
| .NET Framework | 4.6.2 and later |
| .NET Standard | 2.0 |
| .NET | 8.0, 10.0 |

## Where to next

- The [API Reference](xref:Wolfgang.Extensions.String.StringExtensions) documents every overload, its exceptions and an example.
- The [examples project](https://github.com/Chris-Wolfgang/String-Extensions/tree/main/examples) is a runnable console app.
