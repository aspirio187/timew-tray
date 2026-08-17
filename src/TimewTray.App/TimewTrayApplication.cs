using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using TimewTray.Core;

namespace TimewTray.App;

public sealed class TimewTrayApplication : Application
{
    private MainWindow? _mainWindow;
    private TrayIcon? _trayIcon;

    public override void Initialize()
    {
        Styles.Add(new Avalonia.Themes.Fluent.FluentTheme());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            TimewarriorMonitor monitor = new();
            TimerListViewModel viewModel = new(monitor);
            _mainWindow = new MainWindow(viewModel);
            desktop.MainWindow = _mainWindow;
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _trayIcon = CreateTrayIcon(desktop, viewModel);
            TrayIcon.SetIcons(this, new TrayIcons { _trayIcon });
        }

        base.OnFrameworkInitializationCompleted();
    }

    private TrayIcon CreateTrayIcon(
        IClassicDesktopStyleApplicationLifetime desktop,
        TimerListViewModel viewModel)
    {
        NativeMenuItem openItem = new("Open Timew Tray");
        openItem.Click += (_, _) => ShowMainWindow();

        NativeMenuItem refreshItem = new("Refresh");
        refreshItem.Click += async (_, _) => await viewModel.RefreshAsync();

        NativeMenuItem quitItem = new("Quit");
        quitItem.Click += (_, _) => desktop.Shutdown();

        TrayIcon trayIcon = new()
        {
            ToolTipText = "Timew Tray",
            IsVisible = true,
            Icon = CreateTrayIconImage(),
            Menu = new NativeMenu { Items = { openItem, refreshItem, quitItem } }
        };

        trayIcon.Clicked += (_, _) => ShowMainWindow();
        return trayIcon;
    }

    private void ShowMainWindow()
    {
        if (_mainWindow is null)
        {
            return;
        }

        _mainWindow.Show();
        _mainWindow.WindowState = WindowState.Normal;
        _mainWindow.Activate();
    }

    private static WindowIcon CreateTrayIconImage()
    {
        RenderTargetBitmap bitmap = new(new PixelSize(32, 32), new Vector(96, 96));
        Grid surface = new()
        {
            Width = 32,
            Height = 32,
            Background = Brushes.Transparent,
            Children =
            {
                new Ellipse
                {
                    Width = 28,
                    Height = 28,
                    Fill = new SolidColorBrush(Color.FromRgb(42, 111, 219)),
                    Stroke = Brushes.White,
                    StrokeThickness = 2,
                    Margin = new Thickness(2)
                },
                new TextBlock
                {
                    Text = "T",
                    Foreground = Brushes.White,
                    FontWeight = FontWeight.Bold,
                    FontSize = 18,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                }
            }
        };

        surface.Measure(new Size(32, 32));
        surface.Arrange(new Rect(0, 0, 32, 32));
        bitmap.Render(surface);
        return new WindowIcon(bitmap);
    }
}
