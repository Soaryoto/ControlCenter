using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Soaryoto.ViewModels;
using Soaryoto.Views;
using Soaryoto.Infrastructure;
using Soaryoto.Interfaces.Services;

namespace Soaryoto;

public partial class App : Application
{
    private readonly ApplicationHost _applicationHost = new ();
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(
                    _applicationHost.Services.GetService<ILoggingService>(),
                    _applicationHost.Services.GetService<ILoggingServiceReader>(),
                    _applicationHost.Services.GetService<IWebSocketServerService>(),
                    _applicationHost.Services.GetService<IMicrophoneListener>()
                    ),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}