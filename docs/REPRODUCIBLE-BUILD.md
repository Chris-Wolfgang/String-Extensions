# Verifying a release of Wolfgang.Extensions.String

Anyone can check that a published release of `Wolfgang.Extensions.String` was built from the tagged source, without
trusting this repository's CI. This page is for a consumer who has never seen the repository.

## What "reproducible" means here

The same tag, built with the SDK recorded in the release's `BUILD-MANIFEST.txt`, gives:

- byte-identical `.dll` and `.pdb` files for every target framework (`net462`, `netstandard2.0`, `net8.0`, `net10.0`), and
- `.nupkg` / `.snupkg` packages whose **entries** match the published packages entry by entry.

The inputs that used to make a rebuild differ are now pinned:

- The copyright line is a literal year range (`Directory.Build.props`).
- The SDK-generated `*.AssemblyInfo.cs` / `*.AssemblyAttributes.cs` files are rewritten to LF before compilation (`Directory.Build.targets`).
- `net462` compiles against the `Microsoft.NETFramework.ReferenceAssemblies` package on every OS.
- `global.json` pins the SDK major.

`Deterministic` and `ContinuousIntegrationBuild` (set when `CI=true`) normalise source paths, and SourceLink embeds the commit.

What is expected to differ, and is never compared:

- **The package file itself** (`.nupkg` / `.snupkg` as a zip). NuGet stamps every zip entry with the pack time, so
  two packs of the same commit never give the same zip bytes, even on one machine. Compare the entries instead.
- **A build on another operating system.** Cross-OS identity is reported, not promised. The release packs on
  `windows-latest`. With the pieces above, a Linux rebuild should match it. The `Reproducible build (Windows report)`
  job of the [Reproducible Build workflow](../.github/workflows/reproducible-build.yaml) shows on every pull request
  whether it does. If your Linux rebuild differs only in the PDB-derived fields listed under "What a near-match looks
  like", rebuild on Windows.

Every pull request to `main` is gated the same way. The workflow packs the commit twice on Ubuntu, with a full
`git clean` in between, and fails unless both packs are identical by the rules above.

## What a release publishes

Each GitHub Release carries these assets next to the packages:

| Asset | Contents |
|---|---|
| `SHA256SUMS.txt` | `<sha256>  <tfm>/<file>` for every `.dll`/`.pdb`, and `<sha256>  <package-file>` for every `.nupkg`/`.snupkg` |
| `BUILD-MANIFEST.txt` | exact SDK version (`dotnet --version`), runner OS and image, commit SHA, tag, and the `CI` / `ContinuousIntegrationBuild` / `Deterministic` values |
| `reproducible-build-manifest.json` | the SHA-256 of each `.nupkg`/`.snupkg` and of every `.dll`/`.pdb` inside them (see "Per-package hash manifest") |

`SHA256SUMS.txt` and `BUILD-MANIFEST.txt` are subjects of the release's build-provenance attestation, next to the
`.nupkg`. Step 5 below therefore covers them as well as the packages.

## Verify a release

You need git, the GitHub CLI (`gh`), Python 3, PowerShell 7 (`pwsh`) and the .NET SDK the manifest names.

1. **Clone the tag.**

   ```bash
   TAG=v1.2.3   # the release you are checking
   git clone --branch "$TAG" --depth 1 https://github.com/Chris-Wolfgang/String-Extensions.git
   cd String-Extensions
   gh release download "$TAG" --repo Chris-Wolfgang/String-Extensions --dir ../release
   cat ../release/BUILD-MANIFEST.txt
   ```

2. **Install the recorded SDK.** `global.json` pins only the major version. Install the exact version that
   `BUILD-MANIFEST.txt` names (for example with `dotnet-install.sh --version <sdk>`), and check that `dotnet --version`
   prints it from inside the clone.

3. **Pack exactly as the workflow does.** Restore, render the third-party notices (the release packs
   `obj/THIRD-PARTY-NOTICES.md` at the package root, so a pack without it differs by that entry), then pack
   each packable project under `src/`:

   ```bash
   export CI=true
   dotnet restore
   pwsh ./scripts/third-party-notices.ps1
   for proj in $(git ls-files 'src/*.csproj' 'src/**/*.csproj'); do
     [ "$(dotnet msbuild "$proj" -getProperty:IsPackable | tr -d '[:space:]')" = "true" ] || continue
     dotnet pack "$proj" -c Release -p:ContinuousIntegrationBuild=true -p:Deterministic=true -o ../rebuilt
   done
   ```

4. **Compare.**

   ```bash
   # Assemblies and symbols, keyed <tfm>/<file> exactly as in SHA256SUMS.txt.
   for d in $(git ls-files 'src/*.csproj' 'src/**/*.csproj' | xargs -n1 dirname); do
     [ -d "$d/bin/Release" ] || continue
     (cd "$d/bin/Release" && find . -mindepth 2 -maxdepth 2 \( -name '*.dll' -o -name '*.pdb' \) \
       | sed 's|^\./||' | xargs sha256sum | sed 's/ \*/  /')   # Git Bash on Windows prints ' *' (binary mode)
   done | sort -u -k2 > ../rebuilt-assemblies.txt
   grep -vE '\.s?nupkg$' ../release/SHA256SUMS.txt | sort -k2 | diff - ../rebuilt-assemblies.txt \
     && echo "Assemblies and symbols: identical"

   # Packages, entry by entry.
   python3 - ../release ../rebuilt <<'PY'
   import hashlib, pathlib, sys, zipfile
   def entries(p):
       with zipfile.ZipFile(p) as z:
           return {e.filename: hashlib.sha256(z.read(e)).hexdigest() for e in z.infolist() if not e.is_dir()}
   ok = True
   for pub in sorted(pathlib.Path(sys.argv[1]).glob("*.*nupkg")):
       mine = pathlib.Path(sys.argv[2]) / pub.name
       same = mine.exists() and entries(pub) == entries(mine)
       ok &= same
       print(("identical entries: " if same else "DIFFERS: ") + pub.name)
   sys.exit(0 if ok else 1)
   PY
   ```

5. **Check the provenance** of the downloaded packages and hash files against this repository:

   ```bash
   for f in ../release/*.nupkg ../release/SHA256SUMS.txt ../release/BUILD-MANIFEST.txt; do
     gh attestation verify "$f" --repo Chris-Wolfgang/String-Extensions
   done
   ```

## Releases cut before the standard

Releases up to and including 0.1.1 have no `SHA256SUMS.txt`, `BUILD-MANIFEST.txt` or `reproducible-build-manifest.json`.
The SDK they used is no longer recorded, because GitHub deletes run logs after 90 days. Rebuild them as follows:

- Do **not** pass `-p:Copyright`. These releases took a fixed `Copyright (c) Chris Wolfgang` line, and a global property
  overrides the value the tag's own files set, which changes the DLL.
- Pass `-p:NuGetAudit=false`. NuGet's vulnerability audit depends on the date: a tag that restored cleanly on release
  day can fail today with `NU1902`. The flag switches off only the audit and does not change the output.
- Compare each `lib/<tfm>/Wolfgang.Extensions.String.dll` in the published `.nupkg` with
  `src/Wolfgang.Extensions.String/bin/Release/<tfm>/Wolfgang.Extensions.String.dll`. Run
  `cmp -l published.dll rebuilt.dll | wc -l`, where `0` means byte-identical.
- Expect a near-match, described in the next section.

The guarantee covers the library's own assembly. The PDB embeds the repository URL and commit, so a fork builds different PDBs.

## What a near-match looks like (v0.1.1, measured 2026-10-23)

Rebuilding the `v0.1.1` tag on Windows with SDK 10.0.400 (step 2, without a copyright override) and comparing it with the published 0.1.1 package gave **72 differing bytes** in each of the `net462`, `netstandard2.0`, `net8.0` and `net10.0` assemblies, with identical file sizes. For `net8.0` the differences sit in five ranges and nowhere else:

| File offsets (net8.0) | Bytes | Field |
|---|---|---|
| `0x88`–`0x8b` | 4 | PE header `TimeDateStamp` |
| `0xd70`–`0xd7f` | 16 | Module version id (MVID) in the metadata GUID heap |
| `0xfdc`–`0xfdf` | 4 | `TimeDateStamp` of the CodeView debug-directory entry |
| `0x1030`–`0x103f` | 16 | CodeView PDB id (GUID) |
| `0x109f`–`0x10be` | 32 | PDB checksum (SHA-256) |

These five fields are derived from a hash of the compilation, so a different compiler version or different generated source files change them while leaving the code unchanged; every other byte, including the IL, the metadata tables and the resources, was identical. The 0.1.1 release-run log has expired, so the SDK it used is unknown; a different SDK is consistent with the result but is not proven. A mismatch confined to these five ranges is the expected shape of "same source, different toolchain"; a mismatch anywhere else is a real discrepancy.

## Reporting a discrepancy

Open an issue and include: the release tag and commit, your operating system and `dotnet --info`, the exact build command, the `cmp -l` byte count per target framework and the offsets of the differing ranges (for example `cmp -l a.dll b.dll | awk '{printf "%x\n", $1-1}'`). Say whether you passed `-p:Copyright`. Ranges outside the five fields above are the interesting ones.

## Per-package hash manifest

Releases published after the manifest step was added to `release.yaml` also carry a `reproducible-build-manifest.json` asset, next to `SHA256SUMS.txt`. It lists the tag, the commit, and the SHA-256 of each attached `.nupkg` and `.snupkg` and of every `.dll` and `.pdb` inside them. It describes the **published bytes**, which is what a rebuild is compared against; it is not signed (the build-provenance attestation covers the `.nupkg`, see [SECURITY.md](../SECURITY.md)). Use its two kinds of hash differently. The package-level hashes (`.nupkg` / `.snupkg`) cover the whole zip, so they identify the **downloaded** files: check a package you fetched against them. A rebuilt package never matches them, because its zip entries carry the pack time (see "What is expected to differ"). For a rebuild, compare the `.dll` and `.pdb` hashes, and compare packages entry by entry as in step 4. For a release that has `BUILD-MANIFEST.txt`, rebuild with the SDK it names and expect every `.dll` and `.pdb` hash to match. For an older release the SDK is unknown, so compare with `cmp -l` and treat a mismatch confined to those five ranges as the expected near-match. Releases up to and including 0.1.1 have no manifest; compare against the `.nupkg` itself.

## Sharing your verification result

There is no third-party attestation service wired into this repository. To record that you rebuilt a release, open an issue titled `Verification: v<version>` containing the same information as a discrepancy report: tag and commit, operating system and `dotnet --info`, the exact build command, and the per-target-framework `cmp -l` byte count with the differing offsets. State plainly whether the result was byte-identical, a near-match confined to the five fields, or a mismatch elsewhere. A report is a claim by the person who ran it, not something this project signs or vouches for.

### Signed verification attestation (optional)

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
