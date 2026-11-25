# 0008. Take `System.Memory` on net462 and netstandard2.0, for spans inside the library

- **Date:** 2026-11-24
- **Status:** Accepted

## Context

The four identifier case conversions built their result in a temporary `char[]` and then copied it into a string, so every call allocated the array as well as the result (#384). For a 14-character identifier that was 136 bytes for a 56-byte result.

A span over stack memory removes the array. `Span<T>` is part of the framework on net8.0 and later. On .NET Framework 4.6.2 and .NET Standard 2.0 it comes from the `System.Memory` package, and until now the library had no runtime dependency on any target.

## Decision

The net462 and netstandard2.0 builds reference `System.Memory`. The net8.0 and net10.0 builds reference nothing.

Spans are used inside the library only. No span or memory type appears on the public surface, so a consumer never has to reference the package to call the library.

`JoinWords`, the single pass behind the conversions without options, builds its result in a 256-character buffer on the stack when the result is sure to fit (a string of up to 128 characters, since each character can gain one delimiter) and in a heap array otherwise.

## Consequences

- A short string costs one allocation, the result. Measured on .NET 10, bytes allocated per `ToSnakeCase` call: 14 characters, 136 before and 56 after; 31 characters, 240 and 88; 128 characters, 816 and 280. A 500-character string is unchanged at 3,048, because it still uses a heap array.
- Consumers on .NET Framework and .NET Standard 2.0 now restore `System.Memory` and, through it, `System.Buffers`, `System.Numerics.Vectors` and `System.Runtime.CompilerServices.Unsafe`. All four are MIT-licensed Microsoft packages. An application that already uses a different version of one of them may need a binding redirect.
- On those targets the span is the package's portable implementation, which is slower per access than the one built into .NET 8 and later.
- A unit test measures the allocation on .NET 5 and later, so a change that brings the temporary array back fails the build.
- The overloads that take `CaseConversionOptions` still build a word list and a `StringBuilder` (ADR 0007). `PadCenter` on the older targets still uses a `StringBuilder`; moving it to a span is possible now and is not part of this decision.

---

_Template based on [Michael Nygard's ADR format](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)._
