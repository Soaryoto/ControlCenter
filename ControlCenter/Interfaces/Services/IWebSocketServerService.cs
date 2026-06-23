using Soaryoto.Contracts.Messages;

namespace Soaryoto.Interfaces.Services;

public interface IWebSocketServerService : ILaunchedService
{
    void Broadcast(WebSocketMessage webSocketMessage);
}