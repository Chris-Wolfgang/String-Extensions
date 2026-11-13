# Reproducible builds

The assemblies in the `Wolfgang.Extensions.String` package are built deterministically: the same source commit, built with the same .NET SDK on the same operating system, produces byte-identical DLLs and PDBs regardless of the machine or the checkout path. This relies on `Deterministic` and `ContinuousIntegrationBuild` (set when `CI=true`) in `Directory.Build.props`, which normalise source paths, plus SourceLink, which embeds the commit.

One input depends on the calendar: the package takes its copyright line from `Directory.Build.props` (`Copyright (c) <UtcNow.Year> Chris Wolfgang` on current `main`), so a rebuild in a later year changes the `AssemblyCopyright` attribute unless the original year is passed back in (see step 2). Releases up to and including 0.1.1 used a fixed `Copyright (c) Chris Wolfgang` from `Directory.Build.props`, so for those the year does not matter.

## What CI checks

The [Reproducible Build](../.github/workflows/reproducible-build.yaml) workflow, on each of `ubuntu-latest` and `windows-latest`, builds the commit twice from two different directories, hashes every `.dll` and `.pdb` the `src/` project produces, and fails when the two sets of hashes differ. The `.nupkg` itself is not compared, because a zip records entry timestamps; the files inside are what must match. Both builds run in the same job, so the check cannot detect the year dependence described above.

Linux and Windows output are **not** compared with each other and are not byte-identical. The SDK writes `obj/*.AssemblyInfo.cs` and `obj/*.AssemblyAttributes.cs` with the operating system's newline, and the PDB records those files' checksums, so PDBs (and the DLL debug-directory bytes derived from them) differ across operating systems. The compiled IL and metadata are the same. Verify on the same operating system as the build you are checking.

## Verify a release yourself

1. Check out the release tag, on the same operating system as the release build (its `pack-and-validate` job runs on `windows-latest`), with the .NET SDK that job installed. The SDK version is printed in the release run's "Setup .NET" log, but GitHub deletes run logs after 90 days, so for an old release it may no longer be recoverable.
2. Set `CI=true` in the environment and build the library:
   `dotnet build src/Wolfgang.Extensions.String -c Release -p:ContinuousIntegrationBuild=true -p:NuGetAudit=false`.
   For a release built with the year-based copyright line, add `"-p:Copyright=Copyright (c) <year> Chris Wolfgang"`, where `<year>` is the year the release was built (the published DLL's `AssemblyCopyright` attribute shows it). Do **not** pass it for 0.1.1 or earlier: a global property overrides the value the tag's own files set and changes the DLL. `NuGetAudit=false` is needed for older tags: NuGet's vulnerability audit is time-dependent, so a tag that restored cleanly on release day can fail today with `NU1902` once an advisory is published against one of its build-time packages. It switches off the audit only and does not change the output.
3. Download the `.nupkg` from the GitHub Release (or nuget.org) and extract it; it is a zip. For each `lib/<tfm>/Wolfgang.Extensions.String.dll`, compare it with `src/Wolfgang.Extensions.String/bin/Release/<tfm>/Wolfgang.Extensions.String.dll`. `cmp -l published.dll rebuilt.dll | wc -l` prints the number of differing bytes (`0` means byte-identical). A `sha256sum` of each file gives the same yes/no answer.

The guarantee covers the library's own assembly. The PDB embeds the repository URL and commit, so a fork builds different PDBs.

### What a near-match looks like (v0.1.1, measured 2026-10-23)

Rebuilding the `v0.1.1` tag on Windows with SDK 10.0.400 (step 2, without a copyright override) and comparing it with the published 0.1.1 package gave **72 differing bytes** in each of the `net462`, `netstandard2.0`, `net8.0` and `net10.0` assemblies, with identical file sizes. For `net8.0` the differences sit in five ranges and nowhere else:

| File offsets (net8.0) | Bytes | Field |
|---|---|---|
| `0x88`–`0x8b` | 4 | PE header `TimeDateStamp` |
| `0xd70`–`0xd7f` | 16 | Module version id (MVID) in the metadata GUID heap |
| `0xfdc`–`0xfdf` | 4 | `TimeDateStamp` of the CodeView debug-directory entry |
| `0x1030`–`0x103f` | 16 | CodeView PDB id (GUID) |
| `0x109f`–`0x10be` | 32 | PDB checksum (SHA-256) |

These five fields are derived from a hash of the compilation, so a different compiler version or different generated source files change them while leaving the code unchanged; every other byte, including the IL, the metadata tables and the resources, was identical. The 0.1.1 release-run log has expired, so the SDK it used is unknown; a different SDK is consistent with the result but is not proven. A mismatch confined to these five ranges is the expected shape of "same source, different toolchain"; a mismatch anywhere else is a real discrepancy.

### Reporting a discrepancy

Open an issue and include: the release tag and commit, your operating system and `dotnet --info`, the exact build command, the `cmp -l` byte count per target framework and the offsets of the differing ranges (for example `cmp -l a.dll b.dll | awk '{printf "%x\n", $1-1}'`). Say whether you passed `-p:Copyright`. Ranges outside the five fields above are the interesting ones.

### Release hash manifest

Releases published after the manifest step was added to `release.yaml` carry a `reproducible-build-manifest.json` asset. It lists the tag, the commit, and the SHA-256 of each attached `.nupkg` and `.snupkg` and of every `.dll` and `.pdb` inside them. It describes the **published bytes**, which is what a rebuild is compared against; it is not signed (the build-provenance attestation covers the `.nupkg`, see [SECURITY.md](../SECURITY.md)). Because of the toolchain-dependent fields above, a rebuild is not expected to match every hash; compare with `cmp -l` as described and treat a mismatch confined to those five ranges as the expected near-match. Releases up to and including 0.1.1 have no manifest; compare against the `.nupkg` itself.

### Sharing your verification result

There is no third-party attestation service wired into this repository. To record that you rebuilt a release, open an issue titled `Verification: v<version>` containing the same information as a discrepancy report: tag and commit, operating system and `dotnet --info`, the exact build command, and the per-target-framework `cmp -l` byte count with the differing offsets. State plainly whether the result was byte-identical, a near-match confined to the five fields, or a mismatch elsewhere. A report is a claim by the person who ran it, not something this project signs or vouches for.

#### Signed verification attestation (optional)

An issue on its own is an unsigned claim. To make your result checkable by others, also attach a signed attestation, following the [Reproducible Builds](https://reproducible-builds.org/docs/) practice of independent rebuilders publishing signed results. The format is an in-toto statement with the SLSA [Verification Summary Attestation](https://slsa.dev/spec/v1.0/verification_summary) (VSA) predicate, signed keylessly with Sigstore [cosign](https://docs.sigstore.dev/cosign/) so it is bound to your own identity (GitHub, Google or Microsoft account) and recorded in the public Rekor transparency log:

1. Write `vsa.json` (the predicate only):

   ```json
   {
     "verifier": { "id": "https://github.com/<your-github-user>" },
     "timeVerified": "<UTC time, e.g. 2026-11-01T12:00:00Z>",
     "resourceUri": "pkg:nuget/Wolfgang.Extensions.String@<version>",
     "policy": { "uri": "https://github.com/Chris-Wolfgang/String-Extensions/blob/v<version>/docs/REPRODUCIBLE-BUILD.md" },
     "verificationResult": "PASSED",
     "verifiedLevels": ["REPRODUCIBLE_BUILD"],
     "slsaVersion": "1.0"
   }
   ```

   Use `"FAILED"` for a mismatch outside the five toolchain-dependent fields. Put the `cmp -l` byte counts in the issue, not in the predicate.
2. Sign it over the published `.nupkg` you compared against:

   ```bash
   cosign attest-blob Wolfgang.Extensions.String.<version>.nupkg \
     --predicate vsa.json \
     --type https://slsa.dev/verification_summary/v1 \
     --bundle verification.sigstore.json
   ```

   cosign opens a browser for the OIDC sign-in and prints the Rekor log index.
3. Attach `verification.sigstore.json` to your `Verification: v<version>` issue and paste the Rekor log index.

Anyone can then check the attestation against the same `.nupkg` and your identity:

```bash
cosign verify-blob-attestation Wolfgang.Extensions.String.<version>.nupkg \
  --bundle verification.sigstore.json \
  --type https://slsa.dev/verification_summary/v1 \
  --certificate-identity <the e-mail address or URI you signed with> \
  --certificate-oidc-issuer <https://github.com/login/oauth, https://accounts.google.com or https://login.microsoftonline.com>
```

The attestation says who rebuilt the package and what they found; this project does not countersign it. Hosted rebuilder services (for example [vouchsafe.io](https://vouchsafe.io/)) can publish the same kind of statement; none is wired into this repository.
