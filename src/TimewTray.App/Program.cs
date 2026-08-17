using Avalonia;

namespace TimewTray.App;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<TimewTrayApplication>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
