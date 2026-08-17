# Copilot Customization Files

This repository uses GitHub Copilot customization for a small Avalonia desktop app.

- Repository-wide instructions: `.github/copilot-instructions.md`
- Path-specific instructions: `.github/instructions/*.instructions.md`
- Custom agents: `.github/agents/*.agent.md`

## Available Instruction Sets

| File | Applies To | Purpose |
| --- | --- | --- |
| `dotnet.instructions.md` | C# projects, solutions, props files | .NET structure, typing, async, tests |
| `avalonia.instructions.md` | Avalonia views, view models, UI files | Avalonia MVVM and tray UI rules |
| `timewarrior.instructions.md` | Timewarrior and process integration code | Safe `timew` execution and parsing |
| `github-actions.instructions.md` | `.github/workflows/**` | Cross-platform .NET CI |
| `documentation.instructions.md` | README and docs | Concise user/developer docs |

Path-specific instructions add to the repository-wide rules in `../copilot-instructions.md`.

## Custom Agents

Custom agents are intentionally minimal:

| Agent | Purpose |
| --- | --- |
| `Avalonia` | Implements Avalonia/.NET app features |
| `C# Backend` | Implements pure C# core services, Timewarrior adapters, and process-observation logic |
| `QA` | Reviews and tests app behavior |
| `Documentation` | Updates concise user/developer documentation |

Avoid reintroducing specialized agents for unrelated application stacks unless the project scope changes.
