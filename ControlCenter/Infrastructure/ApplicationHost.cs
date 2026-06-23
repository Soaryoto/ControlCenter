using Soaryoto.Interfaces.Services;
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
    }

    private void InitServices()
    {
        _services.RegisterSingleton<ILoggingService, LoggingService>();
        _services.RegisterAlias<ILoggingServiceReader, ILoggingService>();
        _services.RegisterSingleton<IWebSocketServerService, WebSocketServer>();
        _services.RegisterSingleton<IMicrophoneListener, MicrophoneListener>();
        //_services.RegisterSingleton<IButtonInterceptor, ButtonInterceptor>();
        _services.Build();
    }
}