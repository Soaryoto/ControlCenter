using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Soaryoto.Interfaces.Services;
using Soaryoto.ViewModels.Controls;

namespace Soaryoto.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private ILoggingService _loggingService;
    private IWebSocketServerService _webSocketServerService;
    private IMicrophoneListener _microphoneListener;
    public LogViewerViewModel LogsData { get; }
    public MainWindowViewModel(
        ILoggingService loggingService,
        ILoggingServiceReader loggingServiceReader,
        IWebSocketServerService webSocketServerService,
        IMicrophoneListener microphoneListener
        )
    {
        _loggingService = loggingService;
        LogsData = new LogViewerViewModel(loggingServiceReader);
        _webSocketServerService = webSocketServerService;
        _microphoneListener = microphoneListener;
    }

    [RelayCommand]
    private void StartServices()
    {
        _loggingService.WriteLine("Начало включение сервисов");
        _webSocketServerService.StartService();
        _microphoneListener.StartService();
    }

    [RelayCommand]
    private void StopServices()
    {
        _loggingService.WriteLine("Выключение сервисов");
        _webSocketServerService.StopService();
        _microphoneListener.StopService();
    }
}
