using Soaryoto.Messages;
using System.Text.Json;

namespace Soaryoto.Extensions
{
    
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
}