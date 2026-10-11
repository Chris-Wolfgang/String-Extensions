window.BENCHMARK_DATA = {
  "lastUpdate": 1791698892398,
  "repoUrl": "https://github.com/Chris-Wolfgang/String-Extensions",
  "entries": {
    "Mutation score": [
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "99dea8f89a92adb311d5d7c6c47bd25ca448c889",
          "message": "chore: take the template's remaining .gitignore entries (#226)\n\nAdditive: the entries this repository did not already have are appended with\nthe template's own comments, and nothing it does have is touched. A wholesale\ncopy would drop repo-specific entries and could move a !negation relative to\nthe pattern it exempts.\n\n  _site/                     DocFX output at any depth, not just\n                             docfx_project/_site/\n  docfx_project/api/*.yml    metadata docfx generates per public type; the\n                             hand-written index.md and README.md stay tracked\n  *.template                 the sidecars scripts/upgrade.ps1 writes for files\n                             needing a manual merge - resolve and delete them,\n                             never commit them\n\nChecked before appending: no tracked file in this repository matches any of\nthe new patterns.\n\nCo-authored-by: Claude Opus 5 <noreply@anthropic.com>",
          "timestamp": "2026-09-22T22:21:35Z",
          "url": "https://github.com/Chris-Wolfgang/String-Extensions/commit/99dea8f89a92adb311d5d7c6c47bd25ca448c889"
        },
        "date": 1790489277555,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 90.62,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "Chris Wolfgang",
            "username": "Chris-Wolfgang",
            "email": "210299580+Chris-Wolfgang@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "99dea8f89a92adb311d5d7c6c47bd25ca448c889",
          "message": "chore: take the template's remaining .gitignore entries (#226)\n\nAdditive: the entries this repository did not already have are appended with\nthe template's own comments, and nothing it does have is touched. A wholesale\ncopy would drop repo-specific entries and could move a !negation relative to\nthe pattern it exempts.\n\n  _site/                     DocFX output at any depth, not just\n                             docfx_project/_site/\n  docfx_project/api/*.yml    metadata docfx generates per public type; the\n                             hand-written index.md and README.md stay tracked\n  *.template                 the sidecars scripts/upgrade.ps1 writes for files\n                             needing a manual merge - resolve and delete them,\n                             never commit them\n\nChecked before appending: no tracked file in this repository matches any of\nthe new patterns.\n\nCo-authored-by: Claude Opus 5 <noreply@anthropic.com>",
          "timestamp": "2026-09-22T22:21:35Z",
          "url": "https://github.com/Chris-Wolfgang/String-Extensions/commit/99dea8f89a92adb311d5d7c6c47bd25ca448c889"
        },
        "date": 1791100822523,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 90.62,
            "unit": "%"
          }
        ]
      },
      {
        "commit": {
          "author": {
            "name": "dependabot[bot]",
            "username": "dependabot[bot]",
            "email": "49699333+dependabot[bot]@users.noreply.github.com"
          },
          "committer": {
            "name": "GitHub",
            "username": "web-flow",
            "email": "noreply@github.com"
          },
          "id": "4601e16b697c71b1c278a22487c54d07273991c3",
          "message": "build(deps): bump the github-actions group with 4 updates (#229)\n\nBumps the github-actions group with 4 updates: [github/codeql-action/upload-sarif](https://github.com/github/codeql-action), [benchmark-action/github-action-benchmark](https://github.com/benchmark-action/github-action-benchmark), [github/codeql-action/init](https://github.com/github/codeql-action) and [github/codeql-action/analyze](https://github.com/github/codeql-action).\n\n\nUpdates `github/codeql-action/upload-sarif` from 4.38.0 to 4.38.1\n- [Release notes](https://github.com/github/codeql-action/releases)\n- [Changelog](https://github.com/github/codeql-action/blob/main/CHANGELOG.md)\n- [Commits](https://github.com/github/codeql-action/compare/b96794f015dfd88f77b49b1c93e0fa7110f94c63...1c5b675653bb5c22dbe9b12b556ec555138e09fd)\n\nUpdates `benchmark-action/github-action-benchmark` from 1.22.1 to 1.22.2\n- [Release notes](https://github.com/benchmark-action/github-action-benchmark/releases)\n- [Changelog](https://github.com/benchmark-action/github-action-benchmark/blob/master/CHANGELOG.md)\n- [Commits](https://github.com/benchmark-action/github-action-benchmark/compare/v1.22.1...4322e5726e6334590d251fc4f92bec0efafc45dc)\n\nUpdates `github/codeql-action/init` from 4.38.0 to 4.38.1\n- [Release notes](https://github.com/github/codeql-action/releases)\n- [Changelog](https://github.com/github/codeql-action/blob/main/CHANGELOG.md)\n- [Commits](https://github.com/github/codeql-action/compare/b96794f015dfd88f77b49b1c93e0fa7110f94c63...1c5b675653bb5c22dbe9b12b556ec555138e09fd)\n\nUpdates `github/codeql-action/analyze` from 4.38.0 to 4.38.1\n- [Release notes](https://github.com/github/codeql-action/releases)\n- [Changelog](https://github.com/github/codeql-action/blob/main/CHANGELOG.md)\n- [Commits](https://github.com/github/codeql-action/compare/b96794f015dfd88f77b49b1c93e0fa7110f94c63...1c5b675653bb5c22dbe9b12b556ec555138e09fd)\n\n---\nupdated-dependencies:\n- dependency-name: github/codeql-action/upload-sarif\n  dependency-version: 4.38.1\n  dependency-type: direct:production\n  update-type: version-update:semver-patch\n  dependency-group: github-actions\n- dependency-name: benchmark-action/github-action-benchmark\n  dependency-version: 1.22.2\n  dependency-type: direct:production\n  update-type: version-update:semver-patch\n  dependency-group: github-actions\n- dependency-name: github/codeql-action/init\n  dependency-version: 4.38.1\n  dependency-type: direct:production\n  update-type: version-update:semver-patch\n  dependency-group: github-actions\n- dependency-name: github/codeql-action/analyze\n  dependency-version: 4.38.1\n  dependency-type: direct:production\n  update-type: version-update:semver-patch\n  dependency-group: github-actions\n...\n\nSigned-off-by: dependabot[bot] <support@github.com>\nCo-authored-by: dependabot[bot] <49699333+dependabot[bot]@users.noreply.github.com>\nCo-authored-by: Chris Wolfgang <210299580+Chris-Wolfgang@users.noreply.github.com>",
          "timestamp": "2026-10-11T01:36:02Z",
          "url": "https://github.com/Chris-Wolfgang/String-Extensions/commit/4601e16b697c71b1c278a22487c54d07273991c3"
        },
        "date": 1791698891596,
        "tool": "customBiggerIsBetter",
        "benches": [
          {
            "name": "Mutation score",
            "value": 90.62,
            "unit": "%"
          }
        ]
      }
    ]
  }
}