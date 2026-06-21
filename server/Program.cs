using Soaryoto.Interfaces;

namespace Soaryoto;
sealed class Program
{
    private static ServiceContainer _serviceContainer = new ServiceContainer();
    
    static void Main(string[] args)
    {
        FillServiceContainer();

        _serviceContainer.StartAll();

        Application.Run();

        _serviceContainer.StopAll();
    }

    static void FillServiceContainer()
    {
        _serviceContainer.RegisterSingleton<IWebSocketServer, WebSocketServer>();
        _serviceContainer.RegisterSingleton<IMicrophoneListener, MicrophoneListener>();
        //_serviceContainer.RegisterSingleton<IButtonInterceptor, ButtonInterceptor>();
    }
}