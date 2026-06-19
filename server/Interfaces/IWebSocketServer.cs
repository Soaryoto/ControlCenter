namespace Soaryoto.Interfaces
{
    public interface IWebSocketServer : ILaunchedService
    {
        void Broadcast(string json);
    }
}