# 0009. Every method accepts `null`: a `null` string gives `null` back

- **Date:** 2026-11-27
- **Status:** Accepted (supersedes [ADR-0005](0005-left-right-null-tolerant.md))

## Context

ADR 0005 made `Left` and `Right` return `null` for a `null` input, while `PadCenter`, the case conversions and `SplitWords` threw `ArgumentNullException`. That left callers with two rules for one library: a maybe-null value could go through `Left` safely, but not through `ToSnakeCase`. Each of these methods turns one value into another. When the input is missing, the useful result is a missing output, not an exception. The .NET methods that transform values the same way, such as `Path.GetFileName`, `Path.ChangeExtension` and `WebUtility.HtmlEncode`, return `null` for `null` and carry `[return: NotNullIfNotNull]`. Empty strings were already consistent: `""` returns `""` from every method except `PadCenter`.

## Decision

The string receiver of every public method is `string?`.

- **`Left`, `Right` and the case conversions** (`ToCamelCase`, `ToKebabCase`, `ToPascalCase`, `ToSnakeCase`, `ToTitleCase`, with and without `CaseConversionOptions`) return `null` for a `null` input. Their return carries `[return: NotNullIfNotNull(nameof(s))]`, so a caller that passes a non-null string still gets a non-null result, with no new nullable warnings.
- **`PadCenter`** returns a new string of `totalWidth` padding characters for a `null` input: its contract is a string of the requested width.
- **`SplitWords`** returns an empty sequence for a `null` input: its contract is a sequence.
- **Other arguments keep their checks.** A `null` `CaseConversionOptions` still throws `ArgumentNullException`, even when the string is `null`. A negative length or width still throws `ArgumentOutOfRangeException`, and the width is checked before the `null` test.

net462 and netstandard2.0 do not ship `NotNullIfNotNullAttribute`, so the library declares an internal copy for those two targets.

## Consequences

- One rule covers the whole library: a maybe-null value goes through any method without a guard.
- This is a behaviour change from 0.2.0. A caller who relied on `ArgumentNullException` to catch a missing value now gets `null`, or the padded string or empty sequence described above. The public signatures change only in nullability, so the change is not binary-breaking.
