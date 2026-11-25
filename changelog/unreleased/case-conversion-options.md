type: feature

Add `CaseConversionOptions` and an overload of `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` that takes it: keep punctuation in the words, choose what divides words, leave acronyms in upper case, or supply your own word splitter. Add `SplitWords`, which returns the words of a string by the same rules.
