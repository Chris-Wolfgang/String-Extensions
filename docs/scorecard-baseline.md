# OpenSSF Scorecard baseline

The repository is scored weekly, on every push to `main` and on branch-protection changes by
`.github/workflows/scorecard.yaml`. Findings go to the Security tab (Code Scanning), and the score is published to
[scorecard.dev](https://scorecard.dev/viewer/?uri=github.com/Chris-Wolfgang/String-Extensions) and shown by the
badge in the README.

## First published score: 7.5 (2026-11-13, `main` at `01cdaec`, Scorecard v5.6.0)

This is the floor (see [Floor](#floor)). Read from
`https://api.securityscorecards.dev/projects/github.com/Chris-Wolfgang/String-Extensions`.

| Check | Score | Scorecard's reason, and what is done about it |
|---|---:|---|
| Binary-Artifacts | 10 | No binaries found in the repo. |
| Branch-Protection | 4 | Not maximal: `main` requires no approvers, no code-owner review and no last-push approval. A ruleset change, not a code change. Tracked in #195 (`blocked`). |
| CI-Tests | 10 | 30 of 30 merged PRs checked by a CI test. |
| CII-Best-Practices | 0 | No bestpractices.dev badge. An external registration that CI cannot earn; suppressed in Code Scanning. |
| Code-Review | 0 | 0 of 26 changesets approved. Single-maintainer repository; suppressed in Code Scanning (see below). |
| Contributors | 0 | No contributing companies or organizations. Not actionable. |
| Dangerous-Workflow | 10 | No dangerous workflow patterns. |
| Dependency-Update-Tool | 10 | Dependabot detected. |
| Fuzzing | 10 | Project is fuzzed. |
| License | 10 | License file detected. |
| Maintained | 10 | 30 commits and 17 issue events in the last 90 days. |
| Packaging | 10 | Packaging workflow detected. |
| Pinned-Dependencies | 7 | Dependencies not pinned by hash; the NuGet `restore` findings are suppressed in Code Scanning (see below). |
| SAST | 10 | SAST runs on all commits. |
| Security-Policy | 10 | Security policy file detected. |
| Signed-Releases | 0 | The assets of v0.1.0 and v0.1.1 include no signature or provenance file. `release.yaml` now records a SLSA build-provenance attestation on GitHub (`gh attestation verify`), but Scorecard counts only signature and provenance files uploaded as release assets (for example `*.intoto.jsonl`, `*.sigstore.json`). NuGet package signing is tracked in #169 (`blocked` on a code-signing certificate). |
| Token-Permissions | 10 | Workflow tokens follow least privilege. |
| Vulnerabilities | 10 | No known vulnerabilities. |

## State on 2026-10-23 (`main` at `925fc4c`)

**No score is published yet.** `https://api.scorecard.dev/projects/github.com/Chris-Wolfgang/String-Extensions`
returns 404, so the README badge reads "invalid repo path". scorecard.dev rejected every publish because the
analysis job contained a `run:` step (#247); PR #267 moves the SARIF filter to its own job. The first push to
`main` after that fix publishes the first score.

What the Code Scanning results show in the meantime (the same checks, after the suppressions listed below):

| Check | Score | Why, and what is done about it |
|---|---:|---|
| Branch-Protection | 4 | "Not maximal": `main` requires no approvers, no code-owner review and no last-push approval. Improving it is a repository settings (ruleset) change, not a code change. Tracked in #195 (`blocked`). |
| Fuzzing | 0 | "No fuzzer integrations found": Scorecard has no detector for .NET property-based testing, so FsCheck property tests and a weekly fuzz workflow (#100) do not count. Tracked in #193. |
| Security-Policy | 4 | "Security policy file detected", no linked content; the alert was closed 2026-10-12 after `SECURITY.md` took the template's disclosure process (#227). |

Suppressed before the SARIF reaches Code Scanning, with the reason in `scorecard.yaml` (the published score is
unaffected): Code-Review (single-maintainer repository; a required second approver would force an administrator
bypass on every merge), CII-Best-Practices (an external bestpractices.dev registration CI cannot earn), and
Pinned-Dependencies findings for NuGet `restore` commands only (exact versions are pinned in the project files and
nuget.org versions are immutable).

## Floor

The first published score, in the table above, is the floor: a change that would lower the published score or a
check's score below it says why in its pull request. There is no automated
enforcement; the workflow publishes the score but does not compare it with a threshold, and the alerts it raises
are tracked as issues by the security-alerts workflow. Re-measure after each release, since the score depends on
the day's repository state (for example the share of recent changesets that were reviewed).
