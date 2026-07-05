using Soaryoto.Interfaces.Services;
using Soaryoto.Models;
using Soaryoto.Services;

namespace Soaryoto.Infrastructure;

sealed class ApplicationHost
{
    private readonly ServiceContainer _services;
    public ServiceContainer Services => _services;
    public ApplicationHost()
    {
        _services = new ServiceContainer();
        InitServices();
        InitProcesses();
        Build();
    }

    private void InitServices()
    {
        _services.RegisterSingleton<ILoggingService, LoggingService>();
        _services.RegisterAlias<ILoggingServiceReader, ILoggingService>();
        _services.RegisterSingleton<IWebSocketServerService, WebSocketServer>();
        _services.RegisterSingleton<IMicrophoneListener, MicrophoneListener>();
        _services.RegisterSingleton<IProcessManagerService, ProcessManagerService>();
    }

    private void InitProcesses()
    {
        IProcessManagerService processManagerService = _services.GetService<IProcessManagerService>();
        processManagerService.AddProcessInfo(new ManagedProcess()
        {
            Name = "ReactAvatar",
            ProcessTypeName = ProcessTypeName.ReactAvatar,
            Executable = "cmd.exe",
            Arguments = "/C npm run dev",
            WorkingDirectory = @"D:\Projects\Streaming\Soaryoto\client"
        });
        processManagerService.Build();
    }

    private void Build()
    {
        _services.Build();
    }
}