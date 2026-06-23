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
        //_serviceContainer.RegisterSingleton<IWebSocketServer, WebSocketServer>();
        //_serviceContainer.RegisterSingleton<IMicrophoneListener, MicrophoneListener>();
        //_serviceContainer.RegisterSingleton<IButtonInterceptor, ButtonInterceptor>();
        _services.Build();
    }
}