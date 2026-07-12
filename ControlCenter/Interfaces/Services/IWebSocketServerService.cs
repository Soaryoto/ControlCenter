using Soaryoto.Contracts.Messages;

namespace Soaryoto.Interfaces.Services;

public interface IWebSocketServerService : IControllableService
{
    void Broadcast(WebSocketMessage webSocketMessage);
}