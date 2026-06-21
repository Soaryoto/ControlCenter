using Soaryoto.Messages;

namespace Soaryoto.Interfaces;

public interface IWebSocketServer : ILaunchedService
{
    void Broadcast(WebSocketMessage webSocketMessage);
}