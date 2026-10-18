import { spawnSync } from "node:child_process";
import console from "node:console";
import process from "node:process";

const MAX_BYTES = 20_000;
const format = (value) =>
  value.toLocaleString("en-US", { maximumFractionDigits: 2 });
const LOCKFILES = new Set([
  "package-lock.json",
  "npm-shrinkwrap.json",
  "pnpm-lock.yaml",
  "yarn.lock",
  "bun.lock",
  "bun.lockb",
  "Cargo.lock",
  "Gemfile.lock",
  "composer.lock",
  "Pipfile.lock",
  "poetry.lock",
  "uv.lock",
  "pdm.lock",
  "go.sum",
  "packages.lock.json",
  "paket.lock",
  "Podfile.lock",
  "Package.resolved",
  "pubspec.lock",
  "mix.lock",
  "flake.lock",
  ".terraform.lock.hcl",
]);

function generatedPath(path) {
  const name = path.split("/").at(-1);
  return (
    LOCKFILES.has(name) ||
    /(^|\/)(node_modules|vendor|generated|__generated__)(\/|$)/.test(path) ||
    /\.(generated|g)\.[^/]+$/.test(name) ||
    /(?:\.pb\.(?:go|cc|h)|_pb2(?:_grpc)?\.py|\.min\.(?:js|css)|\.(?:js|css)\.map)$/.test(
      name,
    )
  );
}

function exemptPaths(files) {
  const attributes = git(
    [
      "check-attr",
      "--cached",
      "-z",
      "--stdin",
      "token-limit-ignore",
      "linguist-generated",
      "linguist-vendored",
    ],
    files.map((file) => file.path).join("\0") + "\0",
  ).split("\0");
  const exempt = new Set();
  for (let i = 0; i < attributes.length - 1; i += 3) {
    if (attributes[i + 2] === "set" || attributes[i + 2] === "true") {
      exempt.add(attributes[i]);
    }
  }
  return exempt;
}

function git(args, input) {
  const result = spawnSync("git", args, {
    input,
    encoding: "utf8",
    maxBuffer: 64 * 1024 * 1024,
  });
  if (result.error || result.status !== 0) {
    throw new Error(
      result.error?.message || result.stderr.trim() || "Git command failed",
    );
  }
  return result.stdout;
}

function check() {
  process.chdir(git(["rev-parse", "--show-toplevel"]).trim());
  const records = git([
    "diff",
    "--cached",
    "--raw",
    "--no-abbrev",
    "-z",
    "--no-renames",
    "--no-ext-diff",
    "--diff-filter=ACMRT",
    "--",
  ]).split("\0");
  let files = [];
  for (let i = 0; i < records.length - 1; i += 2) {
    const [, mode, , oid] = records[i].split(" ");
    // Only regular files: deleted files, symlinks and submodules have no source to check.
    if (
      (mode === "100644" || mode === "100755") &&
      !generatedPath(records[i + 1])
    ) {
      files.push({ path: records[i + 1], oid });
    }
  }
  if (!files.length) return;
  const exempt = exemptPaths(files);
  files = files.filter((file) => !exempt.has(file.path));
  if (!files.length) return;

  const sizes = git(
    ["cat-file", "--batch-check=%(objectsize)"],
    files.map((file) => file.oid).join("\n") + "\n",
  )
    .trim()
    .split("\n");
  const oversized = files.flatMap((file, i) => {
    if (!/^\d+$/.test(sizes[i] ?? ""))
      throw new Error("Cannot read staged blob size");
    const bytes = Number(sizes[i]);
    return bytes > MAX_BYTES ? [{ ...file, bytes }] : [];
  });
  if (!oversized.length) return;

  console.error(
    "Commit blocked: staged files exceed 5,000 tokens (20,000 bytes; tokens = bytes / 4).",
  );
  for (const file of oversized) {
    console.error(
      `  ${JSON.stringify(file.path)}: ${format(file.bytes)} bytes / ${format(file.bytes / 4)} tokens`,
    );
  }
  console.error(`
How to split these files:
  1. Group code by responsibility. Move cohesive components, services, helpers,
     or types into separate, clearly named modules with small public interfaces.
  2. Keep the original file focused on orchestration. Update imports/exports and
     callers; avoid circular imports. Split tests by behavior and docs by topic.
  3. Keep EVERY resulting file at or below 20,000 bytes (5,000 tokens).
     Do not minify code or remove useful comments merely to fit the limit.
  4. Run the relevant tests, then git add the changed and new files and retry
     the commit. Only staged content is checked; unstaged fixes do not count.

For actual generated or third-party files only: mark their exact paths with
token-limit-ignore in .gitattributes and stage that file too. Never exempt
maintained source just to avoid splitting it.`);
  process.exitCode = 1;
}

try {
  check();
} catch (error) {
  console.error(`Staged file size check failed: ${error.message}`);
  process.exitCode = 1;
}
