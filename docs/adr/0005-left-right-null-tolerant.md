# 0005. `Left` / `Right` accept `null`; `PadCenter` and the case conversions reject it

- **Date:** 2026-10-23 (records the behaviour shipped since 0.1.0)
- **Status:** Accepted

## Context

`Left` and `Right` mirror the VB `Left` / `Right` functions that many callers reach for: take "up to N characters" of a value that may be missing. A `null` input there usually means "no value", and the useful answer is `null`, not an exception. `PadCenter` and the case conversions transform a value into a new one; for them a `null` input has no meaningful result.

## Decision

`Left(this string? s, int length)` and `Right(this string? s, int length)` return `null` for a `null` input and clamp `length` to the string's length. Their signatures are `string?` in and out, so nullable flow analysis follows the value through. `PadCenter` and `ToCamelCase` / `ToKebabCase` / `ToPascalCase` / `ToSnakeCase` / `ToTitleCase` throw `ArgumentNullException` with `ParamName` `"s"`, and their receiver is a non-nullable `string`. (Two gaps against this decision are being fixed under #257: `PadCenter`'s receiver was annotated `string?`, and `ToTitleCase` let `TextInfo` throw with `ParamName` `"str"`.) `Left`, `Right` and `PadCenter` throw `ArgumentOutOfRangeException` for a negative length or width; `Left` and `Right` check the length before the `null` test, so a negative length is reported even for a `null` string.

## Consequences

- `value.Left(3)` is safe on a maybe-null value; the compiler warns when the result is used as non-null.
- Callers get a CS8604 warning, not a runtime surprise, when they pass a maybe-null string to `PadCenter` or a case conversion.
- Making `Left` / `Right` throw, or the others tolerate `null`, would be a breaking behaviour change.
