using CommunityToolkit.Mvvm.Input;
using Soaryoto.Interfaces.Services;
using Soaryoto.ViewModels.Controls;
using Soaryoto.Models;
using Soaryoto.Infrastructure;

namespace Soaryoto.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private ILoggingService _loggingService;
    private ManagerControllableService _managerControllableService;
    private IProcessManagerService _processManagerService;
    public LogViewerViewModel LogsData { get; }
    public MainWindowViewModel(
        ILoggingService loggingService,
        ILoggingServiceReader loggingServiceReader,
        ManagerControllableService managerControllableService,
        IProcessManagerService processManagerService
        )
    {
        _loggingService = loggingService;
        LogsData = new LogViewerViewModel(loggingServiceReader);
        _managerControllableService = managerControllableService;
        _processManagerService = processManagerService;
    }

    [RelayCommand]
    private void StartServices()
    {
        _loggingService.WriteLine("Начало включение сервисов");
        _managerControllableService.StartService(ServicesName.WebSocketServer);
        _managerControllableService.StartService(ServicesName.MicrophoneListener);
        _processManagerService.StartProcess(ProcessTypeName.ReactAvatar);
    }

    [RelayCommand]
    private void StopServices()
    {
        _loggingService.WriteLine("Выключение сервисов");
        _managerControllableService.StartService(ServicesName.WebSocketServer);
        _managerControllableService.StartService(ServicesName.MicrophoneListener);
        _processManagerService.StopProcess(ProcessTypeName.ReactAvatar);
    }
    
    [RelayCommand]
    private void StopReact()
    {
        _processManagerService.StopProcess(ProcessTypeName.ReactAvatar);
    }
}
