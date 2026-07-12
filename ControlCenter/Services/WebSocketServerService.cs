using Soaryoto.Contracts.Messages;
using Soaryoto.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Soaryoto.Models;

namespace Soaryoto.Services;

public sealed class WebSocketServer : BaseControllableService, IWebSocketServerService
{
    private readonly ILoggingService _loggingService;
    private readonly int _port = 8765;
    private readonly HttpListener _listener;
    private readonly List<WebSocket> _clients = new();
    private readonly object _lock = new();
    private CancellationTokenSource _cts = new();
    private void Start()
    {
        _cts = new CancellationTokenSource();
        Task.Run(() => StartAsync(_cts.Token));
    }

    private void Stop()
    {
        _cts.Cancel();
        StopAsync().GetAwaiter().GetResult();
    }

    private async Task StartAsync(CancellationToken ct)
    {
        _listener.Start();
        _loggingService.WriteLine($"[WS] Сервер запущен на ws://localhost:{_port}", LogLevel.Info);

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
                _loggingService.WriteLine($"[WS] Ошибка: {ex.Message}", LogLevel.Error);
            }
        }
    }

    private async Task StopAsync()
    {
        _listener.Stop();
        List<WebSocket> snapshot;
        lock (_lock) snapshot = _clients.ToList();

        var tasks = snapshot
            .Where(ws => ws.State == WebSocketState.Open)
            .Select(ws => ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None));

        await Task.WhenAll(tasks);
        _loggingService.WriteLine($"[WS] Сервер остановлен", LogLevel.Info);
    }
    
    private async Task HandleClientAsync(HttpListenerContext context, CancellationToken ct)
    {
        var wsContext = await context.AcceptWebSocketAsync(null);
        var ws = wsContext.WebSocket;

        lock (_lock) _clients.Add(ws);

        var ip = context.Request.RemoteEndPoint?.ToString() ?? "unknown";
        _loggingService.WriteLine($"[WS] Клиент подключился: {ip}  (всего: {_clients.Count})", LogLevel.Info);

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
            _loggingService.WriteLine($"[WS] Клиент отключился: {ip}  (всего: {_clients.Count})", LogLevel.Info);

            if (ws.State == WebSocketState.Open)
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

            ws.Dispose();
        }
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

    public WebSocketServer(ILoggingService loggingService)
    {
        Name = ServicesName.WebSocketServer;
        _loggingService = loggingService;
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{this._port}/");
    }

    public void Toggle()
    {
        if (State == ServicesState.Stopped)
        {
            Start();
            State = ServicesState.Started;
        }
        else
        {
            Stop();
            State = ServicesState.Stopped;
        }
    }

    public void Broadcast(WebSocketMessage webSocketMessage)
    {
        _ = BroadcastAsync(webSocketMessage.ToJson()).ContinueWith(t =>
        {
            if (t.Exception != null)
                _loggingService.WriteLine($"[WS] Ошибка при broadcast: {t.Exception.InnerException?.Message}", LogLevel.Error);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    public int ClientCount
    {
        get { lock (_lock) return _clients.Count; }
    }
}