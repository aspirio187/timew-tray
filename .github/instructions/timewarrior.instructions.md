---
applyTo: '**/*Timew*.cs,**/*Timewarrior*.cs,**/*Process*.cs,**/*Timer*.cs'
---

# Timewarrior Integration Instructions

Use these rules for code that observes Timewarrior or launches subprocesses.

## Command Execution

- Use `ProcessStartInfo` with `UseShellExecute = false`.
- Add arguments through `ArgumentList`; do not concatenate shell commands.
- Capture stdout, stderr, exit code, elapsed time, and cancellation.
- Apply a timeout to command execution.
- Surface missing executable and non-zero exit codes as explicit states.

## Data Source Rules

- Timewarrior is the source of truth.
- Prefer stable `timew get ...` queries for machine-readable values.
- Avoid parsing localized human output unless no stable query exists.
- Keep read-only observation separate from mutating commands.

## Process Observation

- Do not assume process APIs behave identically on Linux, macOS, and Windows.
- Avoid elevated privileges.
- Treat process lists as best-effort hints, not authoritative timer state.
- Prefer `timew` state for active timer status.

## Error Handling

- Do not swallow failures.
- Include the attempted executable, arguments, exit code, and stderr in diagnostic logs or typed errors.
- Keep user-facing messages concise and actionable.
