# Setup Guide

This guide provides instructions for setting up the String Extensions library development environment.

## Development Setup

### Prerequisites

Before you begin development, ensure you have:

- **.NET SDK** - Supporting .NET Framework 4.6.2 through .NET 10
- A compatible IDE (Visual Studio 2022, Visual Studio 2019, VS Code, or Rider)
- Git for version control
- Basic knowledge of C# and .NET

### Required .NET SDK

The library will support a wide range of .NET versions:

- **.NET Framework 4.6.2** through **.NET Framework 4.8.1**
- **.NET (Core) 2.0** through **.NET 10**

Download the appropriate SDK from: https://dotnet.microsoft.com/download

### Development Tools

Recommended tools for development:

- **Visual Studio 2022** (17.8 or later) or **Visual Studio 2019** (for .NET Framework 4.6.2+ support)
- **Visual Studio Code** with C# extension
- **JetBrains Rider** (2023.3 or later)

## Setting Up the Repository

### 1. Clone the Repository

```bash
git clone https://github.com/Chris-Wolfgang/String-Extensions.git
cd String-Extensions
```

### 2. Explore the Structure

The repository follows a standard .NET project structure:

```
root/
├── src/                        # Source code (to be created)
├── tests/                      # Unit tests (to be created)
├── benchmarks/                 # Performance benchmarks (to be created)
├── examples/                   # Example projects (to be created)
├── docs/                       # Documentation
├── docfx_project/             # DocFX documentation
└── .github/                   # GitHub workflows and configurations
```

### 3. Create Projects

When adding implementation, follow these steps:

#### Create the Main Library Project

```bash
cd src
dotnet new classlib -n StringExtensions -f net462
# Add additional target frameworks as needed
```

#### Create Test Projects

```bash
cd ../tests
dotnet new xunit -n StringExtensions.Tests
```

#### Create Solution File

```bash
cd ..
dotnet new sln -n StringExtensions
dotnet sln add src/StringExtensions/StringExtensions.csproj
dotnet sln add tests/StringExtensions.Tests/StringExtensions.Tests.csproj
```

### 4. Multi-Targeting Configuration

For supporting multiple .NET versions, configure the `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net462;netstandard2.0;net6.0;net8.0;net10.0</TargetFrameworks>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
</Project>
```

## Building the Project

Once projects are created:

### Restore Dependencies

```bash
dotnet restore
```

### Build the Solution

```bash
dotnet build --configuration Release
```

### Run Tests

```bash
dotnet test
```

### Run Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Project Configuration

### Global Using Directives (C# 10+)

Create `GlobalUsings.cs` in the main project:

```csharp
global using System;
global using System.Collections.Generic;
global using System.Linq;
```

### EditorConfig

The repository includes an `.editorconfig` file with coding standards. Ensure your IDE respects these settings.

## Contributing Workflow

### 1. Create a Feature Branch

```bash
git checkout -b feature/my-new-feature
```

### 2. Make Changes

Implement your feature or bug fix following the coding standards.

### 3. Write Tests

Ensure all new code has appropriate test coverage.

### 4. Run Tests Locally

```bash
dotnet test
```

### 5. Commit and Push

```bash
git add .
git commit -m "Description of changes"
git push origin feature/my-new-feature
```

### 6. Create Pull Request

Open a pull request on GitHub for review.

## Documentation

### Building Documentation

The project uses DocFX for documentation generation.

Install DocFX:

```bash
dotnet tool install -g docfx
```

Build documentation:

```bash
cd docfx_project
docfx build
```

Serve documentation locally:

```bash
docfx serve _site
```

## Troubleshooting

### Common Issues

#### Issue: Build fails with framework errors

**Solution:**
Ensure you have all required .NET SDKs installed. Check with:
```bash
dotnet --list-sdks
```

#### Issue: Tests not discovered

**Solution:**
1. Ensure test project references xUnit or your chosen test framework
2. Rebuild the test project:
   ```bash
   dotnet build tests/StringExtensions.Tests
   ```

#### Issue: Multi-targeting issues

**Solution:**
1. Verify all target frameworks are installed
2. Check the `.csproj` `<TargetFrameworks>` element
3. Ensure conditional compilation is used for framework-specific code

## Code Style and Standards

### Naming Conventions

- Use PascalCase for public members
- Use camelCase for private fields with `_` prefix
- Use meaningful, descriptive names

### Extension Methods

Extension methods should:
- Be in static classes
- Use `this` parameter modifier
- Have clear, descriptive names
- Handle null inputs gracefully
- Include XML documentation comments

Example:

```csharp
/// <summary>
/// Performs an operation on the string.
/// </summary>
/// <param name="value">The input string.</param>
/// <returns>The result.</returns>
public static string SomeOperation(this string value)
{
    if (value == null)
        throw new ArgumentNullException(nameof(value));
    
    // Implementation
    return value;
}
```

## Performance Considerations

When implementing extension methods:

1. Minimize allocations
2. Use `Span<T>` and `ReadOnlySpan<T>` where appropriate
3. Consider using `StringBuilder` for multiple string operations
4. Benchmark performance-critical code

## Testing Guidelines

### Test Structure

Use AAA pattern (Arrange, Act, Assert):

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var input = "test";
    
    // Act
    var result = input.SomeOperation();
    
    // Assert
    Assert.Equal("expected", result);
}
```

### Test Coverage

- Aim for high code coverage (80%+)
- Test edge cases and error conditions
- Include null/empty string tests
- Test with different cultures when relevant

## Next Steps

- Review [CONTRIBUTING.md](../../CONTRIBUTING.md) for detailed contribution guidelines
- Check existing issues for tasks to work on
- Join discussions about feature planning

## Getting Help

If you encounter issues during setup:

1. Check [existing GitHub issues](https://github.com/Chris-Wolfgang/String-Extensions/issues)
2. Review the [README](readme.md)
3. Open a new issue with:
   - Your .NET version (`dotnet --version`)
   - Operating system
   - Steps to reproduce
   - Error messages or logs

---

**Ready to contribute?** Check the [Contributing Guide](../../CONTRIBUTING.md) for more details.
