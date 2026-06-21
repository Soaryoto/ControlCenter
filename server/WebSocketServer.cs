using Soaryoto.Interfaces;
using Soaryoto.Utils;
using Soaryoto.Messages;
using Soaryoto.Extensions;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace Soaryoto;

public sealed class WebSocketServer : IWebSocketServer
{
    private readonly int _port = 8765;
    private readonly HttpListener _listener;
    private readonly List<WebSocket> _clients = new();
    private readonly object _lock = new();
    private CancellationTokenSource _cts = new();

    public WebSocketServer()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{this._port}/");
    }

    public void StartService()
    {
        _cts = new CancellationTokenSource();
        Task.Run(() => StartAsync(_cts.Token));
    }

    public void StopService()
    {
        _cts.Cancel();
        StopAsync().GetAwaiter().GetResult();
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _listener.Start();
        ConsoleWriter.WriteLine($"[WS] Сервер запущен на ws://localhost:{_port}", ConsoleColor.Green);

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var context = await _listener.GetContextAsync().WaitAsync(ct);

                if (context.Request.IsWebSocketRequest)
                {
                    _ = HandleClientAsync(context, ct);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                ConsoleWriter.WriteLine($"[WS] Ошибка: {ex.Message}", ConsoleColor.Red);
            }
        }
    }

    public async Task StopAsync()
    {
        _listener.Stop();
        List<WebSocket> snapshot;
        lock (_lock) snapshot = _clients.ToList();

        var tasks = snapshot
            .Where(ws => ws.State == WebSocketState.Open)
            .Select(ws => ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None));

        await Task.WhenAll(tasks);
        ConsoleWriter.WriteLine($"[WS] Сервер остановлен", ConsoleColor.Red);
    }
    
    private async Task HandleClientAsync(HttpListenerContext context, CancellationToken ct)
    {
        var wsContext = await context.AcceptWebSocketAsync(null);
        var ws = wsContext.WebSocket;

        lock (_lock) _clients.Add(ws);

        var ip = context.Request.RemoteEndPoint?.ToString() ?? "unknown";
        ConsoleWriter.WriteLine($"[WS] Клиент подключился: {ip}  (всего: {_clients.Count})", ConsoleColor.Cyan);

        var buffer = new byte[1024];

        try
        {
            while (ws.State == WebSocketState.Open && !ct.IsCancellationRequested)
            {
                var result = await ws.ReceiveAsync(buffer, ct);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;
            }
        }
        catch { /* клиент отвалился */ }
        finally
        {
            lock (_lock) _clients.Remove(ws);
            ConsoleWriter.WriteLine($"[WS] Клиент отключился: {ip}  (всего: {_clients.Count})", ConsoleColor.DarkYellow);

            if (ws.State == WebSocketState.Open)
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

            ws.Dispose();
        }
    }

    public void Broadcast(WebSocketMessage webSocketMessage)
    {
        _ = BroadcastAsync(webSocketMessage.ToJson()).ContinueWith(t =>
        {
            if (t.Exception != null)
                ConsoleWriter.WriteLine($"[WS] Ошибка при broadcast: {t.Exception.InnerException?.Message}", ConsoleColor.Red);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    private async Task BroadcastAsync(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        var segment = new ArraySegment<byte>(bytes);

        List<WebSocket> snapshot;
        lock (_lock) snapshot = _clients.ToList();

        var tasks = snapshot
            .Where(ws => ws.State == WebSocketState.Open)
            .Select(ws => ws.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None));

        await Task.WhenAll(tasks);
    }

    public int ClientCount
    {
        get { lock (_lock) return _clients.Count; }
    }
}