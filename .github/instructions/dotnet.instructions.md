---
applyTo: '**/*.cs,**/*.csproj,**/*.sln,**/Directory.Build.props,**/Directory.Packages.props,**/global.json'
---

# .NET Instructions

Use these rules for C# and .NET project files.

## Project Shape

- Keep the solution small and understandable.
- Put UI concerns in `TimewTray.App`.
- Put Timewarrior/process/domain logic in `TimewTray.Core`.
- Put tests in `TimewTray.Tests`.
- Add new projects only when there is a clear separation of responsibility.

## C# Rules

- Enable nullable reference types and treat warnings as meaningful.
- Prefer immutable records for simple state snapshots.
- Prefer interfaces for external boundaries such as Timewarrior execution, clock, platform services, and notifications.
- Use `async`/`await` for process execution and polling.
- Pass `CancellationToken` through long-running, polling, or process-spawning APIs.
- Avoid `dynamic`, broad `object` payloads, and reflection unless there is a specific platform need.
- Keep public APIs small and named after the domain, not implementation details.

## Testing

- Unit test parsing and state transitions without requiring Timewarrior to be installed.
- Mock process execution through an interface.
- Keep platform-specific behavior isolated so it can be tested with small fakes.
- Prefer deterministic tests over sleeps; inject time or polling abstractions when needed.

## Build Commands

Use the smallest command that covers the change:

```powershell
dotnet test
dotnet build --configuration Release
```

If the repository does not yet contain a solution or project file, document that validation is not available yet.
