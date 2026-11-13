# 0001. Pin `AssemblyVersion` at `1.0.0.0`; bump only on a breaking API change

- **Date:** 2026-10-23 (records a decision already in the csproj)
- **Status:** Accepted

## Context

MSBuild defaults `AssemblyVersion` to `$(Version)`. On .NET Framework, and for any consumer that binds strong-named assemblies, a change to `AssemblyVersion` needs a binding redirect in the consumer's `app.config` / `web.config`. This package ships a `net462` asset, so minor and patch releases that bumped `AssemblyVersion` would push that mechanical cost onto Framework consumers for no benefit.

## Decision

We pin `<AssemblyVersion>1.0.0.0</AssemblyVersion>` in `src/Wolfgang.Extensions.String/Wolfgang.Extensions.String.csproj` and bump it only with a deliberate breaking API change (a new MAJOR version). `FileVersion` (the release version with any prerelease suffix stripped, plus `.0`) and `InformationalVersion` carry the real version; neither takes part in binding. The comment next to the property in the csproj says why, so it is not deleted as cleanup.

## Consequences

- Consumers need no binding redirects for minor or patch updates.
- A MAJOR release must bump `AssemblyVersion` in the same PR as the break, or a breaking change ships under the old identity. The migration guide template (`docs/migrations/`) and the release PR are where this is checked.
- Stack traces and package explorers show `InformationalVersion`, so diagnosing which build a consumer runs is unaffected.
