# 0003. Explicit `using` directives in multi-target projects; no implicit or global usings

- **Date:** 2026-10-23 (records the change made in #233)
- **Status:** Accepted

## Context

`<ImplicitUsings>enable</ImplicitUsings>` generates a different set of global usings per target framework. In a project that targets `net462;netstandard2.0;net8.0;net10.0`, a file can compile on the modern targets and fail on the old ones, or a tool that analyses one target can report a `using` as redundant that another target needs.

## Decision

Every multi-target project leaves implicit usings off, and every source file declares the `using` directives it needs on every target framework. Single-target helper projects (for example a net10.0-only test suite) may use implicit usings, because there is only one set to reason about.

## Consequences

- What a file depends on is visible in the file and is the same on every target.
- Files carry a few more lines at the top.
- A redundant-using report from a tool that analyses one target framework is checked against a full multi-target build before the using is removed.
