---
applyTo: '.github/workflows/**'
---

# GitHub Actions Instructions

Use these rules for CI and packaging workflows.

- Use cross-platform jobs for Linux, macOS, and Windows when validating app behavior.
- Keep CI focused on restore, build, test, and formatting/linting if configured.
- Do not add Docker image build/push workflows for this desktop app unless explicitly requested.
- Avoid secrets for normal build/test workflows.
- Make workflows tolerate the early repository state where no `.sln` or `.csproj` exists yet.
- Prefer PowerShell Core (`pwsh`) for cross-platform scripting in workflows.

