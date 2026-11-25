type: feature

`ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` no longer allocate a temporary array for a string of up to 128 characters: the only allocation is the result. The .NET Framework 4.6.2 and .NET Standard 2.0 builds now depend on the `System.Memory` package; the .NET 8 and .NET 10 builds still have no dependencies.
