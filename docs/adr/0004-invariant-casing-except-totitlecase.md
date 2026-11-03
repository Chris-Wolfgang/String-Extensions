# 0004. Invariant casing for the separator-based conversions; current culture for `ToTitleCase`

- **Date:** 2026-10-23 (records the behaviour pinned by `CultureInvarianceTests`)
- **Status:** Accepted

## Context

`char.ToUpper` / `char.ToLower` and `string.ToUpper` / `string.ToLower` use the current culture. Under `tr-TR`, `'i'.ToUpper()` is `'İ'` (U+0130) and `'I'.ToLower()` is `'ı'` (U+0131), so an identifier-style conversion such as `"file id".ToPascalCase()` would produce a different string on a Turkish machine. `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` produce identifiers, file names and keys, where that difference is a bug.

`ToTitleCase` has the opposite purpose: it formats human-readable text, and title-casing rules are culture-specific. .NET only exposes them through `TextInfo.ToTitleCase` of a specific culture.

## Decision

`ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` use `char.ToUpperInvariant` / `char.ToLowerInvariant`. `ToTitleCase` delegates to `CultureInfo.CurrentCulture.TextInfo.ToTitleCase`, and its XML docs say so.

## Consequences

- The four identifier conversions give the same output under every culture; `CultureInvarianceTests` runs them under `tr-TR`, `de-DE` and `en-US` and pins that.
- `ToTitleCase` can differ between cultures, and leaves words that are entirely upper case unchanged (a `TextInfo` rule). A caller that needs a fixed result sets the culture or calls `TextInfo.ToTitleCase` on the culture it wants.
- Changing either choice is a behaviour change for consumers and needs a new ADR that supersedes this one.
