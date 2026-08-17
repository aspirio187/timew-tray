# Timew Tray

Timew Tray is a small cross-platform Avalonia desktop app for observing local Timewarrior activity from the system tray.

## Current Behavior

- Runs as a desktop app with a tray icon.
- Clicking the tray icon opens the main window.
- Closing the window hides it; the tray menu can reopen it or quit the app.
- Polls Timewarrior every 5 seconds.
- Lists the active Timewarrior timer with its tags and duration.
- Shows a clear message when `timew` is unavailable, inactive, or returns an error.

Timewarrior remains the source of truth. The app is currently read-only and does not start, stop, pause, or edit timers.

## Requirements

- .NET 10 SDK
- Timewarrior available as `timew` on `PATH` for live timer status

## Commands

From the repository root:

```powershell
dotnet restore TimewTray.slnx
dotnet build TimewTray.slnx
dotnet run --project src\TimewTray.App\TimewTray.App.csproj
```

Run tests:

```powershell
dotnet test TimewTray.slnx
```

## Platform Notes

Avalonia supports Linux, macOS, and Windows. Tray behavior depends on the desktop environment; if a platform does not expose tray support consistently, the main window still shows the same Timewarrior status.
