using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using TimewTray.Core;

namespace TimewTray.App;

public sealed class MainWindow : Window
{
    public MainWindow(TimerListViewModel viewModel)
    {
        DataContext = viewModel;
        Title = "Timew Tray";
        Width = 520;
        Height = 360;
        MinWidth = 420;
        MinHeight = 260;
        Content = CreateContent();

        Opened += async (_, _) => await viewModel.StartAsync();
        Closing += (sender, args) =>
        {
            args.Cancel = true;
            ((Window)sender!).Hide();
        };
    }

    private static Control CreateContent()
    {
        TextBlock status = new() { FontSize = 16, FontWeight = Avalonia.Media.FontWeight.SemiBold };
        status.Bind(TextBlock.TextProperty, new Avalonia.Data.Binding(nameof(TimerListViewModel.Status)));

        TextBlock error = new() { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
        error.Bind(TextBlock.TextProperty, new Avalonia.Data.Binding(nameof(TimerListViewModel.ErrorMessage)));

        Button refresh = new() { Content = "Refresh", HorizontalAlignment = HorizontalAlignment.Left };
        refresh.Click += async (_, _) =>
        {
            if (refresh.DataContext is TimerListViewModel viewModel)
            {
                await viewModel.RefreshAsync();
            }
        };

        ListBox timers = new() { MinHeight = 180 };
        timers.Bind(ItemsControl.ItemsSourceProperty, new Avalonia.Data.Binding(nameof(TimerListViewModel.Timers)));

        return new StackPanel
        {
            Margin = new Avalonia.Thickness(16),
            Spacing = 12,
            Children =
            {
                status,
                error,
                refresh,
                timers,
                new TextBlock
                {
                    Text = "Close hides the window; use the tray menu to reopen or quit.",
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap
                }
            }
        };
    }
}

public sealed class TimerListViewModel : INotifyPropertyChanged
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(5);
    private readonly TimewarriorMonitor _monitor;
    private CancellationTokenSource? _pollingCancellation;
    private string _status = "Loading Timewarrior timers...";
    private string? _errorMessage;

    public TimerListViewModel(TimewarriorMonitor monitor)
    {
        _monitor = monitor;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<TimewarriorTimer> Timers { get; } = [];

    public string Status
    {
        get => _status;
        private set => SetField(ref _status, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set => SetField(ref _errorMessage, value);
    }

    public async Task StartAsync()
    {
        if (_pollingCancellation is not null)
        {
            return;
        }

        _pollingCancellation = new CancellationTokenSource();
        await RefreshAsync();
        _ = PollAsync(_pollingCancellation.Token);
    }

    public async Task RefreshAsync()
    {
        TimewarriorSnapshot snapshot = await _monitor.GetSnapshotAsync(CancellationToken.None);
        await Dispatcher.UIThread.InvokeAsync(() => ApplySnapshot(snapshot));
    }

    private async Task PollAsync(CancellationToken cancellationToken)
    {
        using PeriodicTimer timer = new(RefreshInterval);
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            TimewarriorSnapshot snapshot = await _monitor.GetSnapshotAsync(cancellationToken);
            await Dispatcher.UIThread.InvokeAsync(() => ApplySnapshot(snapshot), DispatcherPriority.Background);
        }
    }

    private void ApplySnapshot(TimewarriorSnapshot snapshot)
    {
        Status = snapshot.Status;
        ErrorMessage = snapshot.ErrorMessage;
        Timers.Clear();

        foreach (TimewarriorTimer timer in snapshot.Timers)
        {
            Timers.Add(timer);
        }
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
