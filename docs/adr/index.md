# Architecture Decision Records

Short, dated records of non-obvious choices in this library: the *why* behind decisions that would otherwise be re-derived, often poorly, months after the PR that introduced them.

The format is [Michael Nygard's ADR](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions) (see [TEMPLATE.md](TEMPLATE.md)). A new ADR lands in the same PR as the decision it records, is part of that PR's review, and takes the next four-digit number. An ADR is not edited to reverse it; a new ADR supersedes it, and the old one's status says so.

## Index

| # | Title | Status |
|---|---|---|
| [0001](0001-pin-assemblyversion-at-1-0-0-0.md) | Pin `AssemblyVersion` at `1.0.0.0`; bump only on a breaking API change | Accepted |
| [0002](0002-publicapianalyzers-opt-in-per-project.md) | `PublicApiAnalyzers` is opt-in per project through the presence of `PublicAPI.*.txt` | Accepted |
| [0003](0003-explicit-usings-in-multi-target-projects.md) | Explicit `using` directives in multi-target projects; no implicit or global usings | Accepted |
| [0004](0004-invariant-casing-except-totitlecase.md) | Invariant casing for the separator-based conversions; current culture for `ToTitleCase` | Accepted |
| [0005](0005-left-right-null-tolerant.md) | `Left` / `Right` accept `null`; `PadCenter` and the case conversions reject it | Accepted |
