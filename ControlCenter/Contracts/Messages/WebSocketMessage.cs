namespace Soaryoto.Contracts.Messages;

public record WebSocketMessage(
    string Module,
    string Event,
    object Payload
);