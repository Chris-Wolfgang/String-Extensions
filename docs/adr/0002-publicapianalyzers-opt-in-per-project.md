# 0002. `PublicApiAnalyzers` is opt-in per project through the presence of `PublicAPI.*.txt`

- **Date:** 2026-10-23
- **Status:** Accepted

## Context

`Directory.Build.props` is shared by every project in the repository: the library, the unit tests, the specialised test suites and the benchmarks. `Microsoft.CodeAnalysis.PublicApiAnalyzers` reports RS0016 / RS0017 for every public symbol that is not listed in `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt`. Test and benchmark projects have public classes (xunit and BenchmarkDotNet need them) that are not an API anyone consumes, so turning the analyzer on everywhere would flood them with findings.

## Decision

`Directory.Build.props` references the analyzer, and adds the two files as `AdditionalFiles`, only when `Exists('PublicAPI.Shipped.txt') or Exists('PublicAPI.Unshipped.txt')` holds for the project being built. The library under `src/` carries the files; nothing else does.

## Consequences

- The analyzer guards the shipped surface of the library and stays silent elsewhere.
- Deleting the two files from `src/` silently switches the guard off, so PR review has to catch that. PackageValidation (`EnablePackageValidation` against the last published version, added in #284) is the second, independent line of defence for binary compatibility.
- A new public member needs a `PublicAPI.Unshipped.txt` line in the same PR; at release time the unshipped lines move to `PublicAPI.Shipped.txt`.
