# Contributing to Timew Tray

Thanks for helping with Timew Tray, a small cross-platform Avalonia desktop app for observing local Timewarrior activity.

## Project Scope

Keep contributions focused on the desktop app:

- Avalonia UI and tray behavior
- Local Timewarrior status/process observation
- Cross-platform support for Linux, macOS, and Windows
- Simple documentation and packaging

Do not add server infrastructure, persistent storage layers, cloud sync, telemetry, AI features, or web dashboards unless the project scope is explicitly changed.

## Development Setup

Prerequisites:

- .NET SDK
- Timewarrior (`timew`) for manual integration testing
- Git

Typical commands once a solution exists:

```powershell
dotnet restore
dotnet build
dotnet test
```

If no solution or project exists yet, create the smallest Avalonia app structure needed for the task and document the command used.

## Coding Guidelines

- Use C# with nullable reference types enabled.
- Keep Timewarrior command execution behind an interface.
- Use `ProcessStartInfo.ArgumentList`; never concatenate shell commands.
- Keep UI logic in view models, not views.
- Keep polling cancellable and non-blocking.
- Surface Timewarrior errors clearly.
- Isolate platform-specific code behind interfaces.

## Testing

Test core behavior without requiring Timewarrior to be installed by mocking the process runner. Cover:

- Timewarrior missing
- No active timer
- Active timer with tags
- Non-zero command exit
- Parser edge cases
- View-model state transitions

Manual UI checks should mention the operating system used.

## Commit Conventions

Use conventional commits:

```text
feat: add tray status menu
fix: handle missing timew executable
docs: update setup instructions
test: cover active timer parsing
chore: update ci workflow
```

## Pull Requests

- Keep PRs focused.
- Fill out the PR template.
- Include validation commands or explain why validation is not available yet.
- Include platform notes for tray, packaging, or startup behavior.
