using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace TimewTray.Core;

public sealed class TimewarriorMonitor
{
    private static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(3);
    private readonly string _timewarriorExecutable;

    public TimewarriorMonitor(string timewarriorExecutable = "timew")
    {
        _timewarriorExecutable = timewarriorExecutable;
    }

    public async Task<TimewarriorSnapshot> GetSnapshotAsync(CancellationToken cancellationToken)
    {
        try
        {
            CommandResult active = await RunTimewarriorAsync(["get", "dom.active"], cancellationToken);
            if (!active.IsSuccess)
            {
                return TimewarriorSnapshot.Failed(active.GetErrorText(_timewarriorExecutable));
            }

            if (!string.Equals(active.Output.Trim(), "1", StringComparison.Ordinal))
            {
                return TimewarriorSnapshot.Inactive();
            }

            IReadOnlyList<string> tags = await ReadActiveTagsAsync(cancellationToken);
            string duration = await ReadValueOrFallbackAsync("dom.active.duration", "running", cancellationToken);
            string title = tags.Count == 0 ? "Active Timewarrior timer" : string.Join(" ", tags);
            TimewarriorTimer timer = new(title, string.Join(", ", tags), duration, DateTimeOffset.Now);

            return new TimewarriorSnapshot(true, "1 active Timewarrior timer", [timer], null);
        }
        catch (Win32Exception error)
        {
            return TimewarriorSnapshot.Missing($"Could not start '{_timewarriorExecutable}': {error.Message}");
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return TimewarriorSnapshot.Failed($"'{_timewarriorExecutable}' did not respond within {CommandTimeout.TotalSeconds:0} seconds.");
        }
    }

    private async Task<IReadOnlyList<string>> ReadActiveTagsAsync(CancellationToken cancellationToken)
    {
        string tagCountText = await ReadValueOrFallbackAsync("dom.active.tag.count", "0", cancellationToken);
        if (!int.TryParse(tagCountText.Trim(), out int tagCount) || tagCount <= 0)
        {
            return Array.Empty<string>();
        }

        List<string> tags = [];
        for (int index = 1; index <= tagCount; index++)
        {
            string tag = await ReadValueOrFallbackAsync($"dom.active.tag.{index}", string.Empty, cancellationToken);
            if (!string.IsNullOrWhiteSpace(tag))
            {
                tags.Add(tag.Trim());
            }
        }

        return tags;
    }

    private async Task<string> ReadValueOrFallbackAsync(string key, string fallback, CancellationToken cancellationToken)
    {
        CommandResult result = await RunTimewarriorAsync(["get", key], cancellationToken);
        return result.IsSuccess ? result.Output.Trim() : fallback;
    }

    private async Task<CommandResult> RunTimewarriorAsync(
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        using CancellationTokenSource timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(CommandTimeout);

        ProcessStartInfo startInfo = new()
        {
            FileName = _timewarriorExecutable,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using Process process = new() { StartInfo = startInfo };
        process.Start();

        Task<string> outputTask = process.StandardOutput.ReadToEndAsync(timeout.Token);
        Task<string> errorTask = process.StandardError.ReadToEndAsync(timeout.Token);
        await process.WaitForExitAsync(timeout.Token);

        string output = await outputTask;
        string error = await errorTask;
        return new CommandResult(process.ExitCode, output, error);
    }

    private sealed record CommandResult(int ExitCode, string Output, string Error)
    {
        public bool IsSuccess => ExitCode == 0;

        public string GetErrorText(string executable)
        {
            StringBuilder message = new();
            message.Append($"'{executable}' exited with code {ExitCode}.");
            if (!string.IsNullOrWhiteSpace(Error))
            {
                message.Append(' ');
                message.Append(Error.Trim());
            }

            return message.ToString();
        }
    }
}
