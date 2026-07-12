using Soaryoto.Interfaces.Services;
using Soaryoto.Services;

namespace Soaryoto.Infrastructure;

sealed class ApplicationHost
{
    private readonly ServiceContainer _services;
    public ServiceContainer Services => _services;

    private ManagerControllableService _managerControllableService;
    public ManagerControllableService ManagerControllableService => _managerControllableService;

    public ApplicationHost()
    {
        _services = new ServiceContainer();
        InitServices();
        InitProcesses();

        _managerControllableService = new ManagerControllableService(_services.GetServices<IControllableService>());
        
        Build();
    }

    private void InitServices()
    {
        _services.RegisterSingleton<IProjectEnvironmentService, ProjectEnvironmentService>();

        _services.RegisterSingleton<ILoggingService, LoggingService>();
        _services.RegisterAlias<ILoggingServiceReader, LoggingService>();

        _services.RegisterSingleton<IWebSocketServerService, WebSocketServer>();
        _services.RegisterAlias<IControllableService, WebSocketServer>();

        _services.RegisterSingleton<IMicrophoneListener, MicrophoneListener>();
        _services.RegisterAlias<IControllableService, MicrophoneListener>();

        _services.RegisterSingleton<IProcessManagerService, ProcessManagerService>();

        _services.RegisterSingleton<IReactService, ReactService>();
        _services.RegisterAlias<IControllableService, ReactService>();
    }

    private void InitProcesses()
    {
        IProcessManagerService processManagerService = _services.GetService<IProcessManagerService>();
        processManagerService.Build();
    }
    
    private void Build()
    {
        _services.Build();
    }
}