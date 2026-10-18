// Install the repository's staged-token-limit hook without discarding other checks.
import { spawnSync } from "node:child_process";
import console from "node:console";
import {
  chmodSync,
  existsSync,
  mkdirSync,
  readFileSync,
  renameSync,
  writeFileSync,
} from "node:fs";
import { dirname, join, resolve } from "node:path";
import process from "node:process";
import { fileURLToPath } from "node:url";

const directory = dirname(fileURLToPath(import.meta.url));
const root = resolve(directory, "../..");

function git(args, optional = false) {
  const result = spawnSync("git", args, { cwd: root, encoding: "utf8" });
  if (
    result.error ||
    (result.status !== 0 && !(optional && result.status === 1))
  ) {
    throw new Error(result.error?.message || result.stderr.trim());
  }
  return result.stdout.trim();
}

function install() {
  const skip =
    (process.env.CI && process.env.CI !== "false") ||
    process.env.NODE_ENV === "production" ||
    process.env.npm_config_production === "true" ||
    process.env.npm_config_omit?.split(/\s+/).includes("dev");
  if (skip || !existsSync(join(root, ".git"))) return;

  const configured = git(["config", "--path", "--get", "core.hooksPath"], true);
  if (configured) {
    if (
      configured
        .replaceAll("\\", "/")
        .endsWith("/.config/git-token-limit/hooks")
    ) {
      console.log(
        "The shared staged-token-limit hook already covers this checkout.",
      );
      return;
    }
    throw new Error(
      `Custom core.hooksPath is active (${configured}). Preserve it and add node scripts/git-hooks/check-staged-file-size.mjs to its pre-commit hook; see scripts/git-hooks/README.md.`,
    );
  }

  const common = git([
    "rev-parse",
    "--path-format=absolute",
    "--git-common-dir",
  ]);
  const hooks = join(common, "hooks");
  const target = join(hooks, "pre-commit");
  const backup = join(hooks, "pre-commit.before-token-limit");
  const content = readFileSync(
    join(directory, "pre-commit"),
    "utf8",
  ).replaceAll("\r\n", "\n");
  mkdirSync(hooks, { recursive: true });
  if (existsSync(target)) {
    const previous = readFileSync(target, "utf8");
    if (previous === content) {
      chmodSync(target, 0o755);
      return;
    }
    if (previous.includes("staged-token-limit:") || existsSync(backup)) {
      throw new Error(
        "Existing hook or backup has changed; refusing to overwrite it. See scripts/git-hooks/README.md.",
      );
    }
    renameSync(target, backup);
  }
  writeFileSync(target, content, { flag: "wx", mode: 0o755 });
  chmodSync(target, 0o755);
  console.log(
    "Installed staged-token-limit pre-commit hook; existing checks are preserved.",
  );
}

try {
  install();
} catch (error) {
  console.error(error.message);
  process.exitCode = 1;
}
