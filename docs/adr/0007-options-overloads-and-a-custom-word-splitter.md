# 0007. Options overloads for the case conversions, with a caller-supplied word splitter

- **Date:** 2026-11-24
- **Status:** Accepted

## Context

ADR 0006 gave the four identifier conversions one rule for finding words and made it the only behaviour. Some callers need a different rule: punctuation kept in the result (what the library did before ADR 0006), an acronym left in upper case (`SKUCode`, the Microsoft naming style), a brand name such as `iPhone` not split at its capital, or words found by the caller's own code.

Three constraints shaped the answer:

- The methods without options are called in loops and walk the string once, without building a list of words. A design that routed them through an options object and a word list would allocate several objects per call.
- The library targets .NET Framework 4.6.2 and .NET Standard 2.0. A span cannot be a generic type argument or be returned with `yield return` on any target, so a splitter that yields spans cannot be written.
- A settings object can gain a property later without a new overload; a list of boolean parameters cannot.

## Decision

Each of `ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` gets one overload that takes a `CaseConversionOptions`. Two `SplitWords` methods return the words themselves.

`CaseConversionOptions` is a sealed class with settable properties. A new instance holds the rules of ADR 0006:

| Property | Type | Default |
|---|---|---|
| `Punctuation` | `PunctuationHandling` (`Drop`, `Keep`) | `Drop` |
| `Boundaries` | `WordBoundaries` flags (`Separators`, `Delimiters`, `CaseChanges`, `ControlCharacters`, `All`, `None`) | `All` |
| `Acronyms` | `AcronymHandling` (`AsWord`, `Preserve`) | `AsWord` |
| `WordSplitter` | `Func<string, IEnumerable<string>>?` | `null` |

The rules:

1. **`Punctuation = Drop`**: a character that is not a letter, a digit or a combining mark ends the word and is left out, and an apostrophe is removed first. Of `Boundaries`, only `CaseChanges` then makes a difference.
2. **`Punctuation = Keep`**: such a character stays in the word it touches, unless it is one of the `Boundaries` named, which still divide words and are left out. An apostrophe is kept like any other punctuation.
3. **`Acronyms = Preserve`**: where a word would be capitalized, a word with no lower-case letter is written as it is. The first word of a camel-case result is still lower-cased, and snake and kebab case are not affected.
4. **Capitalizing** upper-cases the first character of the word that is a letter, a digit or a mark, so a word kept with punctuation in front of it (`(hello`) becomes `(Hello`.
5. **`WordSplitter`**, when set, replaces rules 1 and 2. It receives the string unchanged. Its words are cased and joined as they are and in its order; its sequence is enumerated exactly once; a `null` or empty word is skipped; a `null` sequence is an `InvalidOperationException`.

The methods without options stay on the single pass of ADR 0006 (`JoinWords`). The overloads build a word list (`SplitWordsIterator`, or the caller's splitter) and format it (`FormatWords`). The two paths are held together by tests: a fixed corpus in the unit tests and a fuzz property both require that a conversion with new options equals the conversion without options.

The splitter's signature is `Func<string, IEnumerable<string>>`, so a caller can pass an iterator, a list, an array or `Regex.Split`.

## Consequences

- The public surface grows by one class, three enums, four overloads and two `SplitWords` methods. `PublicAPI.Unshipped.txt` lists them.
- The default rules now exist twice, as a single pass and as an iterator. A change to one must be made to the other; the equality tests fail if they drift.
- The overloads allocate a string per word and a `StringBuilder`. A caller who needs the fastest path uses the method without options.
- The zero value of `PunctuationHandling` and of `AcronymHandling` is the default rule (`Drop`, `AsWord`). `WordBoundaries.None` is zero and is not the default; the property initializer sets `All`.
- A `ReadOnlyMemory<char>`-based splitter overload can be added later without breaking this one.

---

_Template based on [Michael Nygard's ADR format](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)._
