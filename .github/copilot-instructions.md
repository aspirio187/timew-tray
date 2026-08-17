# GitHub Copilot Instructions

Repository-wide instructions for the Timew Tray project.

## Project Overview

Timew Tray is a simple cross-platform Avalonia desktop application for Linux, macOS, and Windows.

Its purpose is intentionally narrow: observe Timewarrior activity and Timewarrior subprocesses on the local computer, then present that status in a small tray/desktop UI. Do not turn this into a web app, service platform, database-backed product, cloud sync tool, or general project-management system unless the user explicitly changes the scope.

## Core Product Rules

- Track Timewarrior state through the local `timew` command and local process observation.
- Detect relevant `timew` subprocesses visible to the current user on Linux, macOS, and Windows. If OS permissions hide some processes, surface that limitation rather than requesting elevated privileges.
- Show useful local status: whether Timewarrior is available, whether a timer is active, active tags, elapsed duration, and recent relevant activity when available.
- Keep behavior read-only by default. Starting, stopping, pausing, or editing timers must be an explicit feature request and must be clearly visible to the user.
- Never silently replace `timew` behavior with custom storage. Timewarrior remains the source of truth.
- If Timewarrior is missing, unavailable, or returns an error, surface that state in the UI with an actionable message.
- Avoid privileged OS APIs. The app should work as a normal user process.
- Keep the app local-first. Do not add telemetry, remote APIs, accounts, analytics, or network calls unless explicitly requested.

## Expected Technology

- **Runtime:** .NET
- **UI:** Avalonia UI
- **Language:** C#
- **Pattern:** MVVM with testable services
- **Platforms:** Linux, macOS, Windows
- **External integration:** Timewarrior CLI (`timew`)

Prefer a small structure until the codebase needs more:

```text
src/
  TimewTray.App/          # Avalonia application, views, view models
  TimewTray.Core/         # Timewarrior/process abstractions and domain logic
tests/
  TimewTray.Tests/        # Unit tests for core behavior
```

## Timewarrior Integration

- Wrap all `timew` calls behind a dedicated service interface.
- Use `ProcessStartInfo.ArgumentList`; do not build shell command strings.
- Capture stdout, stderr, exit code, timeout, and cancellation explicitly.
- Treat command execution as fallible. Return typed results or throw project-specific exceptions with context.
- Do not parse localized human output when a stable `timew get ...` query is available.
- Keep polling intervals modest and cancellable. Avoid busy loops.
- Do not assume Linux process layouts on macOS or Windows.

## Avalonia UI Rules

- Keep UI logic in view models and services, not in code-behind.
- Use bindings and commands for UI interactions.
- Keep tray/status UI minimal and responsive.
- Do not block the UI thread with process execution or polling.
- Model platform-specific tray or notification behavior behind interfaces.
- Prefer plain, accessible text over decorative UI complexity.

## C# and .NET Rules

- Enable and respect nullable reference types.
- Avoid `dynamic`, reflection, and `object` as escape hatches.
- Prefer explicit small types for domain concepts such as active timer state, command result, and Timewarrior availability.
- Use dependency injection where it keeps services testable, but do not over-engineer.
- Pass `CancellationToken` through async polling and process execution paths.
- Keep exceptions meaningful; do not swallow errors or return success-shaped fallbacks.
- Prefer deterministic unit tests for parsers, command wrappers, and view models.

## Cross-Platform Rules

- Use .NET and Avalonia cross-platform APIs first.
- Isolate OS-specific code behind interfaces and guard it with platform checks.
- Do not introduce Windows-only dependencies for core behavior.
- Validate path handling on Linux, macOS, and Windows.
- Assume `timew` may be absent from `PATH`; allow future configuration of the executable path.

## Time Tracking for AI Work

When a repository-level `timer.sh` exists, agents must use it exactly as the human work-session timer:

- At the first active work on this repository for the day/session, run `./timer.sh status`; if no timer is active, run `./timer.sh start` from the repository root.
- Leave the timer running across AI responses, research phases, approvals, waits for the user, idle time between prompts, and completed subtasks.
- Run `./timer.sh pause` or `./timer.sh stop` only when the user explicitly instructs it, or when the user explicitly says the human work session on this project is finished.
- When the user signals the end of the workday in any language, write an accurate summary of at most 10 words from the day's context and run `./timer.sh close-day "<summary>"`.
- `./timer.sh status` is read-only. If the script fails, inform the user; do not silently replace it with raw `timew` commands.
- Optional tags are supported by the script, for example `./timer.sh start development`.

## Documentation Requirements

Update documentation when behavior, setup, packaging, commands, or user-facing workflows change. Keep documentation short and practical:

- What changed
- How to run or use it
- Platform-specific notes, if any
- Known limitations, if any

## Pull Request Expectations

- Keep changes focused and small.
- Include tests for core parsing, process execution behavior, and view models when practical.
- For UI changes, describe the manual check performed on each relevant platform.
- Do not leave stale references to unrelated application stacks.
