# Staged file token limit

Pre-commit rejects maintained files above **5,000 tokens**, estimated as **bytes / 4**.
Exactly 20,000 bytes passes; 20,001 fails. It measures complete staged Git blobs,
including partially staged files, without reading unstaged file contents.

## Setup

Requires Git and Node.js 18 or newer on PATH. Use the repository's higher Node
requirement if it has one. In repositories using Husky, the existing dependency
installation activates the integrated pre-commit check. Otherwise run:

```sh
node scripts/git-hooks/install.mjs
```

Each clone needs hook setup. The installer preserves an existing default hook as
`.git/hooks/pre-commit.before-token-limit` and runs it between the size checks.
It recognizes the shared `~/.config/git-token-limit/hooks` installation and leaves
it in place. Other custom `core.hooksPath` values are not overwritten: add
`node scripts/git-hooks/check-staged-file-size.mjs` before and after the existing
pre-commit checks, ensuring any failure exits nonzero. Installation is skipped in
CI, production-only installs, and source archives without a `.git` entry.

To undo the default installation, remove its managed `.git/hooks/pre-commit`
and restore `pre-commit.before-token-limit` if present. In linked worktrees,
these files live in the common Git directory shown by `git rev-parse --git-common-dir`.
For Husky, remove the added checker invocations from `.husky/pre-commit`.

## Exclusions

Git does not identify agent authorship, so all maintained files are checked.
Known machine output is excluded: common package-manager lockfiles, directories
named `node_modules`, `vendor`, `generated`, or `__generated__`, generated filename
patterns, protobuf output, minified JS/CSS, and JS/CSS source maps. Exact names
are listed in `check-staged-file-size.mjs`.

For other generated files, third-party code, or non-authored assets, add narrow
`.gitattributes` rules and stage that file too:

```gitattributes
api/client.ts linguist-generated=true
third-party/** linguist-vendored
assets/logo.png token-limit-ignore
```

Bare attributes and `=true` are recognized. Attributes come from the index;
Git's local/global attribute configuration also applies. Never exempt maintained
source merely to fit. Binary files are not automatically exempt.

Only changed staged regular files are checked. Deletions, symlinks, submodules,
unchanged files, and untracked files are skipped. Renames are checked at their
destination. The error lists every oversized file with byte and token counts,
then explains how to extract cohesive modules, update imports/callers, split
tests and documentation, test, stage the resulting files, and retry.

## Verification

```sh
node --test scripts/git-hooks/verify.mjs
```

The tests exercise real commits in isolated temporary repositories. They cover
the threshold, staged versus unstaged data, exclusions, failure instructions,
existing-hook preservation, formatter changes, and custom-hook protection.
The GitHub workflow runs this suite on Windows and Linux without installing
application dependencies. It verifies hook behavior; it does not scan unstaged
files or reject existing oversized files elsewhere in the repository.
