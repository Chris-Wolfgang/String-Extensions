# Setup Guide

This guide provides detailed instructions for setting up and configuring the String Extensions library in various project types and scenarios.

## Table of Contents

- [Basic Setup](#basic-setup)
- [Advanced Configuration](#advanced-configuration)
- [Project Types](#project-types)
- [Development Setup](#development-setup)
- [Troubleshooting](#troubleshooting)

## Basic Setup

### 1. Install the NuGet Package

First, install the String Extensions package in your project:

#### Using .NET CLI

```bash
dotnet add package String-Extensions
```

#### Using Visual Studio Package Manager

```powershell
Install-Package String-Extensions
```

#### Using Visual Studio UI

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages"
3. Click the "Browse" tab
4. Search for "String-Extensions"
5. Select the package and click "Install"

### 2. Add Using Statement

In your C# files, add the using directive:

```csharp
using StringExtensions;
```

### 3. Verify Installation

Create a simple test to verify the installation:

```csharp
using StringExtensions;

var test = "hello world";
var result = test.ToPascalCase();
Console.WriteLine(result); // Should output: HelloWorld
```

## Advanced Configuration

### Global Using Directives (C# 10+)

To avoid adding the using statement in every file, add a global using directive:

Create or edit `GlobalUsings.cs` in your project:

```csharp
global using StringExtensions;
```

### ImplicitUsings in .csproj

For .NET 6+ projects, you can add implicit usings to your project file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <Using Include="StringExtensions" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="String-Extensions" Version="*" />
  </ItemGroup>
</Project>
```

## Project Types

### ASP.NET Core Web Applications

For ASP.NET Core projects, add the package reference to your `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="String-Extensions" Version="*" />
</ItemGroup>
```

Use in controllers, views, or Razor pages:

```csharp
public class HomeController : Controller
{
    public IActionResult Index()
    {
        var title = "welcome_to_my_app".ToPascalCase();
        ViewBag.Title = title;
        return View();
    }
}
```

### Console Applications

In a console application's `Program.cs`:

```csharp
using StringExtensions;

Console.WriteLine("Enter text to transform:");
var input = Console.ReadLine();
var result = input?.ToPascalCase() ?? string.Empty;
Console.WriteLine($"Result: {result}");
```

### Class Libraries

For class library projects that will use String Extensions:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="String-Extensions" Version="*" />
  </ItemGroup>
</Project>
```

### Blazor Applications

String Extensions works seamlessly with Blazor:

```razor
@page "/example"
@using StringExtensions

<h3>String Transformation Example</h3>

<input @bind="inputText" />
<button @onclick="Transform">Transform</button>

<p>Result: @result</p>

@code {
    private string inputText = "";
    private string result = "";

    private void Transform()
    {
        result = inputText.ToPascalCase();
    }
}
```

### Unit Test Projects

In test projects (xUnit, NUnit, MSTest):

```csharp
using StringExtensions;
using Xunit;

public class StringExtensionTests
{
    [Fact]
    public void ToPascalCase_ConvertsCorrectly()
    {
        // Arrange
        var input = "hello_world";

        // Act
        var result = input.ToPascalCase();

        // Assert
        Assert.Equal("HelloWorld", result);
    }
}
```

## Development Setup

### Setting Up for Contributing

If you want to contribute to the String Extensions library:

#### 1. Clone the Repository

```bash
git clone https://github.com/Chris-Wolfgang/String-Extensions.git
cd String-Extensions
```

#### 2. Restore Dependencies

```bash
dotnet restore
```

#### 3. Build the Solution

```bash
dotnet build
```

#### 4. Run Tests

```bash
dotnet test
```

#### 5. Run Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Development Tools

Recommended tools for development:

- **Visual Studio 2022** (17.8 or later)
- **Visual Studio Code** with C# extension
- **JetBrains Rider** (2023.3 or later)

### Required .NET SDK

- **.NET 8.0 SDK** or later

Download from: https://dotnet.microsoft.com/download

## Troubleshooting

### Common Issues

#### Issue: Extension methods not appearing in IntelliSense

**Solution:**
1. Ensure you've added the `using StringExtensions;` directive
2. Rebuild the solution
3. Restart Visual Studio or VS Code
4. Check that the package is properly installed in your `.csproj` file

#### Issue: Version conflicts with other packages

**Solution:**
1. Check for package version conflicts:
   ```bash
   dotnet list package --include-transitive
   ```
2. Update all packages to compatible versions:
   ```bash
   dotnet add package String-Extensions --version [specific-version]
   ```

#### Issue: Build errors after installation

**Solution:**
1. Clean the solution:
   ```bash
   dotnet clean
   ```
2. Restore packages:
   ```bash
   dotnet restore
   ```
3. Rebuild:
   ```bash
   dotnet build
   ```

#### Issue: Package not found in NuGet

**Solution:**
1. Ensure your NuGet sources are configured correctly
2. Clear the NuGet cache:
   ```bash
   dotnet nuget locals all --clear
   ```
3. Restore packages:
   ```bash
   dotnet restore
   ```

### Getting Help

If you encounter issues not covered here:

1. Check the [API Documentation](../api/index.md)
2. Search [existing GitHub issues](https://github.com/Chris-Wolfgang/String-Extensions/issues)
3. Create a new issue with:
   - Your .NET version
   - Project type
   - Steps to reproduce
   - Error messages or logs

## Performance Considerations

### Best Practices

1. **Cache Results**: If you're calling the same extension method repeatedly with the same input, cache the result
2. **Use StringBuilder**: For multiple string operations, consider using StringBuilder with the extension methods
3. **Avoid in Loops**: Move string operations outside of tight loops when possible

### Memory Management

String Extensions is designed to minimize allocations:

```csharp
// Good - Single allocation
var result = input.ToPascalCase();

// Less efficient - Multiple allocations in a loop
for (int i = 0; i < 1000; i++)
{
    var temp = inputs[i].ToPascalCase(); // Consider caching if input repeats
}
```

## Configuration Files

### NuGet.config

For enterprise environments, you might need a custom NuGet configuration:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <!-- Add your private feed if needed -->
  </packageSources>
</configuration>
```

### Directory.Build.props

For consistent package versions across multiple projects:

```xml
<Project>
  <ItemGroup>
    <PackageReference Include="String-Extensions" Version="1.0.0" />
  </ItemGroup>
</Project>
```

## Next Steps

Now that you have String Extensions set up, explore:

- [Getting Started Guide](getting-started.md) - Basic usage examples
- [API Documentation](../api/index.md) - Complete method reference
- [README](readme.md) - Overview and features

---

**Need more help?** Visit the [GitHub repository](https://github.com/Chris-Wolfgang/String-Extensions) or open an issue.
