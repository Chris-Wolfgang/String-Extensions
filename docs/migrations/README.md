# Migration guides

One document per major-version transition. Written during release prep for the target major (not retroactively), and linked from that release's GitHub Release notes.

The template in [TEMPLATE-major-version-migration.md](TEMPLATE-major-version-migration.md) is the required shape: summary + exhaustive breaking-change inventory + before/after samples per break + deprecation timeline.

## When a new guide gets written

- The release PR that turns over the MAJOR version (0.x → 1.0, 1.x → 2.0, …) adds `vX-to-vN.md` to this folder in the same PR as the breaking code change.
- The guide is part of the release-prep checklist; a MAJOR bump does not merge without one.
- From 1.0 on, minor / patch releases do NOT get a migration guide — non-breaking by SemVer contract. Before 1.0 see below.

## Index

_No guides yet: the library is at 0.1.1 and has not shipped a breaking release. The first entry lands with the 0.x → 1.0 release PR._

## Before 1.0

Pre-1.0, a breaking change rides on a **minor** bump (`changelog/unreleased/README.md`: the minor version is the compatibility line until 1.0). A 0.x release whose fragments include `type: breaking` gets a guide here too, named for its versions (for example `v0.1-to-v0.2.md`).

## Release-PR checklist for a breaking release

- The guide is added in the same PR, from the template.
- `<AssemblyVersion>` is bumped in the same PR ([ADR-0001](../adr/0001-pin-assemblyversion-at-1-0-0-0.md)); a breaking change must not ship under the old binding identity.
- Each intentional binary break has a `CompatibilitySuppressions.xml` entry for PackageValidation, and `PublicAPI.Shipped.txt` is updated.
- The GitHub Release notes link the guide.
