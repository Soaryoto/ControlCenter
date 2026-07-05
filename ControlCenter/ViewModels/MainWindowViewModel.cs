using CommunityToolkit.Mvvm.Input;
using Soaryoto.Interfaces.Services;
using Soaryoto.ViewModels.Controls;
using Soaryoto.Models;

namespace Soaryoto.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private ILoggingService _loggingService;
    private IWebSocketServerService _webSocketServerService;
    private IMicrophoneListener _microphoneListener;
    private IProcessManagerService _processManagerService;
    public LogViewerViewModel LogsData { get; }
    public MainWindowViewModel(
        ILoggingService loggingService,
        ILoggingServiceReader loggingServiceReader,
        IWebSocketServerService webSocketServerService,
        IMicrophoneListener microphoneListener,
        IProcessManagerService processManagerService
        )
    {
        _loggingService = loggingService;
        LogsData = new LogViewerViewModel(loggingServiceReader);
        _webSocketServerService = webSocketServerService;
        _microphoneListener = microphoneListener;
        _processManagerService = processManagerService;
    }

    [RelayCommand]
    private void StartServices()
    {
        _loggingService.WriteLine("Начало включение сервисов");
        _webSocketServerService.StartService();
        _microphoneListener.StartService();
        _processManagerService.StartProcess(ProcessTypeName.ReactAvatar);
    }

    [RelayCommand]
    private void StopServices()
    {
        _loggingService.WriteLine("Выключение сервисов");
        _webSocketServerService.StopService();
        _microphoneListener.StopService();
        _processManagerService.StopProcess(ProcessTypeName.ReactAvatar);
    }
    
    [RelayCommand]
    private void StopReact()
    {
        _processManagerService.StopProcess(ProcessTypeName.ReactAvatar);
    }
}
