using System.Text.Json;

namespace Soaryoto.Contracts.Messages;

public record WebSocketMessage(
    string Module,
    string Event,
    object Payload
);

public static class WebSocketMessageExtensions
{
    public static string ToJson(this WebSocketMessage message)
    {
        return JsonSerializer.Serialize(message);
    }

    public static WebSocketMessage FromJson(string json)
    {
        return JsonSerializer.Deserialize<WebSocketMessage>(json) ?? new WebSocketMessage( "error", "Invalid message format", new { });
    }
}