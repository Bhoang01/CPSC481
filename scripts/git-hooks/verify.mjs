import assert from "node:assert/strict";
import { spawnSync } from "node:child_process";
import {
  chmodSync,
  copyFileSync,
  mkdirSync,
  mkdtempSync,
  readFileSync,
  rmSync,
  writeFileSync,
} from "node:fs";
import { tmpdir } from "node:os";
import { delimiter, dirname, join } from "node:path";
import process from "node:process";
import test from "node:test";
import { fileURLToPath } from "node:url";

const source = dirname(fileURLToPath(import.meta.url));

function repository(t, previousHook) {
  const cwd = mkdtempSync(join(tmpdir(), "staged-token-limit-"));
  const env = {
    ...process.env,
    CI: "",
    GIT_CONFIG_NOSYSTEM: "1",
    GIT_CONFIG_GLOBAL: join(cwd, "empty-gitconfig"),
    PATH: dirname(process.execPath) + delimiter + process.env.PATH,
  };
  for (const key of [
    "GIT_DIR",
    "GIT_WORK_TREE",
    "GIT_INDEX_FILE",
    "GIT_COMMON_DIR",
    "GIT_CONFIG_COUNT",
  ])
    delete env[key];
  t.after(() => rmSync(cwd, { recursive: true, force: true }));
  function git(...args) {
    const result = spawnSync("git", args, { cwd, env, encoding: "utf8" });
    assert.equal(result.status, 0, result.stderr);
    return result.stdout;
  }
  function write(path, content) {
    mkdirSync(dirname(join(cwd, path)), { recursive: true });
    writeFileSync(join(cwd, path), content);
  }
  git("init", "--quiet");
  git("config", "user.email", "test@example.invalid");
  git("config", "user.name", "Hook Test");
  git("config", "commit.gpgsign", "false");
  git("config", "core.autocrlf", "false");
  for (const name of [
    "check-staged-file-size.mjs",
    "install.mjs",
    "pre-commit",
  ]) {
    const destination = join(cwd, "scripts/git-hooks", name);
    mkdirSync(dirname(destination), { recursive: true });
    copyFileSync(join(source, name), destination);
  }
  if (previousHook) {
    write(".git/hooks/pre-commit", "#!/bin/sh\n" + previousHook + "\n");
    chmodSync(join(cwd, ".git/hooks/pre-commit"), 0o755);
  }
  function install() {
    return spawnSync(process.execPath, ["scripts/git-hooks/install.mjs"], {
      cwd,
      env,
      encoding: "utf8",
    });
  }
  const installed = install();
  assert.equal(installed.status, 0, installed.stderr);
  function commit() {
    return spawnSync(
      "git",
      ["commit", "--quiet", "--allow-empty", "-m", "fixture"],
      { cwd, env, encoding: "utf8" },
    );
  }
  return { cwd, git, write, commit, install };
}

test("installed hook checks staged bytes and explains how to split", (t) => {
  const { git, write, commit } = repository(t);
  write("source with spaces.ts", "a".repeat(20_001));
  git("add", "--", "source with spaces.ts");
  write("source with spaces.ts", "small unstaged replacement");
  const blocked = commit();
  assert.notEqual(blocked.status, 0);
  assert.match(
    blocked.stderr,
    /source with spaces\.ts.*20,001 bytes.*5,000\.25/,
  );
  assert.match(blocked.stderr, /responsibility/);
  assert.match(blocked.stderr, /imports/);
  assert.match(blocked.stderr, /git add/);
  write("source with spaces.ts", "a".repeat(20_000));
  git("add", "--", "source with spaces.ts");
  write("source with spaces.ts", "a".repeat(40_000));
  write("untracked.ts", "a".repeat(40_000));
  const allowed = commit();
  assert.equal(allowed.status, 0, allowed.stderr);
});

test("exempts generated files using staged attributes without ignoring maintained source", (t) => {
  const { git, write, commit } = repository(t);
  const exempt = [
    "package-lock.json",
    "packages/app/pnpm-lock.yaml",
    "Cargo.lock",
    "go.sum",
    "src/__generated__/api.ts",
    "src/client.generated.ts",
    "api/service.pb.go",
    "vendor/library.js",
    "public/app.min.js",
    "public/app.js.map",
    "api/client.ts",
    "third-party/library.ts",
    "assets/image.bin",
  ];
  for (const path of exempt) write(path, "a".repeat(20_001));
  write(
    ".gitattributes",
    "api/client.ts linguist-generated=true\nthird-party/library.ts linguist-vendored\nassets/image.bin token-limit-ignore\n",
  );
  git("add", "--", ".gitattributes", ...exempt);
  const allowed = commit();
  assert.equal(allowed.status, 0, allowed.stderr);
  write("maintained.ts", "a".repeat(20_001));
  git("add", "--", "maintained.ts");
  write(".gitattributes", "maintained.ts token-limit-ignore\n");
  const blocked = commit();
  assert.notEqual(blocked.status, 0);
  assert.match(blocked.stderr, /maintained\.ts/);
});

test("preserves existing hooks and rejects oversized formatter output", (t) => {
  const { cwd, git, write, commit, install } = repository(
    t,
    "echo previous-failed >&2; exit 1",
  );
  assert.equal(install().status, 0);
  const failed = commit();
  assert.notEqual(failed.status, 0);
  assert.match(failed.stderr, /previous-failed/);
  write("small.ts", "small");
  write("formatted-output", "a".repeat(20_001));
  git("add", "--", "small.ts");
  write(
    ".git/hooks/pre-commit.before-token-limit",
    "#!/bin/sh\ncp formatted-output small.ts\ngit add -- small.ts\n",
  );
  const blocked = commit();
  assert.notEqual(blocked.status, 0);
  assert.match(blocked.stderr, /small\.ts.*20,001 bytes/);
  const original = readFileSync(join(cwd, ".git/hooks/pre-commit"), "utf8");
  git("config", "core.hooksPath", ".custom-hooks");
  const refused = install();
  assert.notEqual(refused.status, 0);
  assert.match(refused.stderr, /Custom core.hooksPath/);
  assert.equal(
    readFileSync(join(cwd, ".git/hooks/pre-commit"), "utf8"),
    original,
  );
});
