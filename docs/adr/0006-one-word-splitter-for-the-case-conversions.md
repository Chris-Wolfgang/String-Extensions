# 0006. One word splitter for the four identifier case conversions

- **Date:** 2026-11-24
- **Status:** Accepted

## Context

`ToCamelCase`, `ToKebabCase`, `ToPascalCase` and `ToSnakeCase` each had their own loop and their own idea of where a word ends. Only characters for which `char.IsSeparator` is true divided words, so:

- an identifier came out as one word (`"UserLoginCount".ToSnakeCase()` was `userlogincount`);
- one format could not be converted to another (`"user_login_count".ToPascalCase()` was `User_login_count`);
- punctuation stayed in the result (`"hello, world!".ToSnakeCase()` was `hello,_world!`), which is not an identifier, a file name or a key;
- a word that began with punctuation was not capitalized (`"(hello) world".ToPascalCase()` was `(hello)World`);
- tab and newline were not boundaries, because they are control characters and not separators.

There is no formal standard for these formats. The written definitions closest to one, and the widely used conversion libraries, agree on the approach: find the words, then case and join them. Google's definition of camel case removes apostrophes and splits on spaces and remaining punctuation. This ADR does not adopt the Microsoft rule that keeps a two-letter acronym in upper case (`IOStream`).

## Decision

The four conversions share one private routine, `JoinWords`, and differ only in the casing of each word and in the delimiter between words.

| Method | Casing of each word | Between words |
|---|---|---|
| `ToSnakeCase` | lower | `_` |
| `ToKebabCase` | lower | `-` |
| `ToPascalCase` | first character upper, rest lower | nothing |
| `ToCamelCase` | as Pascal, but the first word all lower | nothing |

The words of a string are found by these rules, in this order:

1. **Apostrophes are removed first** (`'` U+0027 and `’` U+2019). They do not end a word: `don't` is the word `dont`.
2. **A word is a run of letters and digits.** A combining mark counts as part of its word, and so does a surrogate pair that is a letter or a digit.
3. **Every other character ends the word and is left out**: white space, punctuation, symbols, control characters, `_`, `-`, emoji, and a surrogate that has no partner.
4. **A word also starts at a case boundary**: at an upper-case letter that follows a lower-case letter or a digit (`userLogin`, `Line2Qty`), and at the last upper-case letter of a run that a lower-case letter follows (`SKUCode` is `SKU` and `Code`).

Consequences of the rules that are decisions in their own right:

- The result is the empty string when the input has no letters or digits.
- An acronym is one word and is cased like any other: `"SKUCode".ToPascalCase()` is `SkuCode`.
- A word that starts with a digit keeps it and is otherwise lower-cased: `"9lives cat".ToPascalCase()` is `9livesCat`. A digit has no upper-case form, and the letter after it is not the start of the word.
- Casing stays invariant and per `char` (ADR 0004). A letter outside the Basic Multilingual Plane is kept in its word but is neither cased nor seen as a case boundary.

`ToTitleCase` is not part of this decision. It formats prose, keeps the spaces and punctuation, and delegates to `TextInfo`.

## Consequences

- Any of the four formats converts to any other, because `_`, `-` and case boundaries are all word boundaries for every method.
- The rule lives in one place. A change to what a word is changes all four methods together, and the fuzz tests compare each method with one shared reference word list.
- This is a breaking change in behaviour for callers who passed text with punctuation, control characters or leading and trailing separators: those characters no longer appear in the result. It is recorded as a `breaking` changelog fragment. The public API surface is unchanged.
- Known limits that follow from reading only letter case, tracked in #381: `SKUcode` splits as `SK` and `Ucode`; `IDs` splits as `I` and `Ds`; two acronyms in a row (`HTTPSURL`) are one word; an accent written as a separate combining mark hides a case boundary after it.
- A later change may add an overload that takes options, including a caller-supplied word splitter. The no-argument methods keep these rules as their default.

---

_Template based on [Michael Nygard's ADR format](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)._
