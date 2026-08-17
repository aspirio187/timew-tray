namespace TimewTray.Core;

public sealed record TimewarriorTimer(
    string Title,
    string Tags,
    string Duration,
    DateTimeOffset RefreshedAt)
{
    public string DisplayText =>
        string.IsNullOrWhiteSpace(Tags)
            ? $"{Title} - {Duration}"
            : $"{Title} - {Duration} - {Tags}";

    public override string ToString() => DisplayText;
}

public sealed record TimewarriorSnapshot(
    bool IsAvailable,
    string Status,
    IReadOnlyList<TimewarriorTimer> Timers,
    string? ErrorMessage)
{
    public static TimewarriorSnapshot Missing(string message) =>
        new(false, "Timewarrior unavailable", Array.Empty<TimewarriorTimer>(), message);

    public static TimewarriorSnapshot Failed(string message) =>
        new(true, "Unable to read timers", Array.Empty<TimewarriorTimer>(), message);

    public static TimewarriorSnapshot Inactive() =>
        new(true, "No active Timewarrior timer", Array.Empty<TimewarriorTimer>(), null);
}
