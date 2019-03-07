using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using vtortola.WebSockets;

namespace pc_remote_server.Server
{
    public abstract class Server
    {
        private readonly List<Task> _runningTasks = new List<Task>();
        private readonly CancellationTokenSource _token;

        protected Server(IPEndPoint endpoint)
        {
            var cancellation = new CancellationTokenSource();
            _token = cancellation;

            var server = new WebSocketListener(endpoint);
            var rfc6455 = new WebSocketFactoryRfc6455();
            server.Standards.RegisterStandard(rfc6455);
            server.StartAsync(cancellation.Token).ConfigureAwait(false);

            Debug.WriteLine("Echo Server started at " + endpoint);

            var task = Task.Run(() => AcceptWebSocketClientsAsync(server, cancellation.Token), cancellation.Token);
            _runningTasks.Add(task);
        }

        public void Stop()
        {
            Debug.WriteLine("Server stopping");
            _token.Cancel();
            _runningTasks.ForEach(t => t.Wait(_token.Token));
            _runningTasks.Clear();
        }

        private async Task AcceptWebSocketClientsAsync(WebSocketListener server, CancellationToken token)
        {
            Debug.WriteLine("Accepting clients");
            while (!token.IsCancellationRequested)
                try
                {
                    var ws = await server.AcceptWebSocketAsync(token).ConfigureAwait(false);
                    if (ws != null)
                        Task.Run(
                            () => HandleConnectionAsync(ws, token).ConfigureAwait(false), token
                        ).ConfigureAwait(false);
                }
                catch (Exception aex)
                {
                    Debug.WriteLine("Error Accepting clients: " + aex.GetBaseException().Message);
                }

            Debug.WriteLine("Server Stop accepting clients");
        }

        private async Task HandleConnectionAsync(WebSocket ws, CancellationToken cancellation)
        {
            try
            {
                while (ws.IsConnected && !cancellation.IsCancellationRequested)
                {
                    var msg = await ws.ReadStringAsync(cancellation).ConfigureAwait(false);
                    if (msg == null) continue;

                    var command = JsonConvert.DeserializeObject<Command>(msg);
                    var response = await HandleMessage(command);
                    ws.WriteString(JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception aex)
            {
                Debug.WriteLine("Error Handling connection: " + aex.GetBaseException().Message);
                try
                {
                    ws.Close();
                }
                catch
                {
                    // ignored
                }
            }
            finally
            {
                ws.Dispose();
            }
        }

        protected abstract Task<CommandResponse> HandleMessage(Command message);
    }
}