namespace Soaryoto {
    internal class Program
    {
        private static Task serverTask = null!;
        
        static void Main(string[] args)
        {
            ButtonInterceptor.Start();
            MicrophoneListener.StartListening();

            WebSocketServer wsServer = new WebSocketServer();
            CancellationTokenSource cts = new CancellationTokenSource();

            serverTask = Task.Run(() => wsServer.StartAsync(cts.Token));

            Application.Run();
            
            cts.Cancel();
            Task.Run(() => wsServer.StopAsync()).Wait();
            
            ButtonInterceptor.Stop();
            MicrophoneListener.StopListening();
        }
    }
}