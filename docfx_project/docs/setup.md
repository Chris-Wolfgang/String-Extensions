# Building from source

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download). The test project also targets older frameworks; tests for a framework run only when its runtime (or, for .NET Framework, Windows) is available.
- Optional: [PowerShell 7](https://github.com/PowerShell/PowerShell) for the scripts under `scripts/`.

## Repository layout

| Folder | Contents |
|---|---|
| `src/Wolfgang.Extensions.String/` | The library (`StringExtensions.cs`) and its `PublicAPI.*.txt` baseline |
| `tests/Wolfgang.Extensions.String.Tests.Unit/` | xUnit unit tests, run on every target framework |
| `tests/Wolfgang.Extensions.String.Tests.DocExamples/` | Compiles every code example in the XML doc comments against the library, so the examples cannot rot |
| `tests/Wolfgang.Extensions.String.Tests.Fuzz/` | FsCheck property tests against an independent oracle; a change to a method's output must change the oracle too |
| `tests/Wolfgang.Extensions.String.Tests.Snapshots/` | Verify snapshot tests; a change to a method's output must update the `.verified` files |
| `tests/Wolfgang.Extensions.String.Tests.Aot/` | The unit tests built as a Native AOT executable (xunit.v3.aot; not a `dotnet test` project) |
| `tests/Wolfgang.Extensions.String.Tests.AotSmoke/` | A small trimmed, Native AOT console consumer (not a `dotnet test` project) |
| `benchmarks/Wolfgang.Extensions.String.Benchmarks/` | BenchmarkDotNet benchmarks |
| `examples/` | A runnable console example |
| `docfx_project/` | This documentation site |
| `docs/` | Maintainer documentation and the architecture decision records (`docs/adr/`) |
| `changelog/unreleased/` | Changelog fragments, one per pull request that changes `src/` |

## Build and test

```bash
git clone https://github.com/Chris-Wolfgang/String-Extensions.git
cd String-Extensions
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

Release builds treat warnings as errors, and every public member needs an XML doc comment.

## Run the PR checks locally

```powershell
pwsh ./scripts/build-pr.ps1
```

This mirrors the Windows stage of the PR workflow: a Release build, the tests on every target framework, the coverage gate (production code and test code are both gated; see `CODECOV_MINIMUM` and `CODECOV_TEST_MINIMUM` in `.github/workflows/pr.yaml`), DevSkim and gitleaks.

## Conventions

- Test names follow `Method_when_condition_expected_result`, for example `Left_when_passed_null_returns_null`.
- Usings are explicit in every file; implicit and global usings are off.
- A PR that changes `src/` adds a fragment under `changelog/unreleased/` (see its `README.md`).
- See [CONTRIBUTING.md](https://github.com/Chris-Wolfgang/String-Extensions/blob/main/CONTRIBUTING.md) for the full guide.

## Building this site

```bash
dotnet tool restore
dotnet docfx docfx_project/docfx.json --serve
```
