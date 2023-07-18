using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NamespaceChat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using WweebbAapppp.Models;

//using Microsoft.AspNetCore.WebSockets;

namespace WweebbAapppp.MyWebSocket
{
    public class MyWebSocketManager
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IConfiguration _configuration;
        //private readonly VehicleQuotesContext _vehicleQuotesContext;

        public MyWebSocketManager(IConnectionManager connectionManager, IConfiguration configuration)//, VehicleQuotesContext vehicleQuotesContext)
        {
            _connectionManager = connectionManager;
            _configuration = configuration;
            //_vehicleQuotesContext = vehicleQuotesContext;
        }

        public async Task HandleWebSocketRequest(HttpContext context, VehicleQuotesContext _vehicleQuotesContext)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

            var buffer = new byte[1024];
            WebSocketReceiveResult result = null;

            try
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                if (ex.InnerException != null)
                {
                    // Выводим информацию о внутреннем исключении
                    string sadfasd = ex.InnerException.Message;
                }
                else
                {
                    string sadf = ex.Message;
                }
            }


            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var data = JsonConvert.DeserializeObject<SendMessage>(message);
                string token = data.token;
                string newMessage = data.message;

                var tokenHandler = new JwtSecurityTokenHandler();

                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, ValidationParameters.GetValidationParameters(_configuration), out _);
                }
                catch (Exception ex)
                {
                    return;
                }

                var modelMessage = new Message()
                {
                    Text = newMessage,
                    UserId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
                    ChatId = 1,
                    SentAt = DateTime.UtcNow,
                };

                _vehicleQuotesContext.Messages.Add(modelMessage);
                await _vehicleQuotesContext.SaveChangesAsync();

                var messagesData = _vehicleQuotesContext.Messages;/*.AsEnumerable();.Select(message => new
                {
                    id = message.Id,
                    text = message.Text,
                    sentAt = message.SentAt,
                    userId = message.UserId,
                });*/

                await _connectionManager.SendMessageToAllOrPersonAsync("newMessage", messagesData, "");
            }
            else
            {
                string connectionId = GenerateConnectionId();
                _connectionManager.AddConnection(connectionId, webSocket);

                try
                {
                    await ProcessWebSocketMessages(webSocket, connectionId); // Обработка входящих сообщений
                }
                finally
                {
                    _connectionManager.RemoveConnection(connectionId); // Удаление соединения из коллекции при отключении
                    webSocket.Dispose(); // Освобождение ресурсов WebSocket
                }
            }
        }

        private async Task ProcessWebSocketMessages(WebSocket webSocket, string connectionId)
        {
            var buffer = new byte[1024];
            WebSocketReceiveResult result;

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var cancellationToken = cancellationTokenSource.Token;

                // Запускаем таймер с заданным таймаутом
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(300), cancellationToken);

                do
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {

                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        IEnumerable<Message> messages = JsonConvert.DeserializeObject<IEnumerable<Message>>(message);
                        await HandleMessage(messages, connectionId); // Обработка входящего сообщения

                        // Сброс таймера при получении нового сообщения
                        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
                    }

                } while (!result.CloseStatus.HasValue && !cancellationToken.IsCancellationRequested);

                //var closeStatus = result.CloseStatus.Value;
                var closeDescription = result.CloseStatusDescription;

                //logger.LogInformation($"Соединение закрыто. Статус: {closeStatus}. Описание: {closeDescription}");

               _connectionManager.RemoveConnection(connectionId);
            }

            // Другие действия при закрытии WebSocket-соединения
        }

        private async Task HandleMessage(IEnumerable<Message> message, string connectionId = "")
        {
            // Обработка входящего сообщения
            await _connectionManager.SendMessageToAllOrPersonAsync("newMessage", message, connectionId);
        }


        private string GenerateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public static class WebSocketExtensions
    {
        public static IApplicationBuilder UseMyWebSocketManager(this IApplicationBuilder app, string path)
        {
            return app.UseMiddleware<MyWebSocketMiddleware>(path);
        }

        public static IServiceCollection AddMyWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddTransient<MyWebSocketManager>();
            services.AddTransient<MyWebSocketMiddleware>();
            return services;
        }
    }

    public interface IConnectionManager
    {
        void AddConnection(string connectionId, WebSocket webSocket);
        Task SendMessageToAllOrPersonAsync(string messageType, IEnumerable<Message> message, string connectionWebSocketId);
        void RemoveConnection(string connectionId);
    }

    public class ConnectionManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();

        public void AddConnection(string connectionId, WebSocket webSocket)
        {
            _connections.TryAdd(connectionId, webSocket);
        }

        public void RemoveConnection(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }

        public async Task SendMessageToAllOrPersonAsync(string messageType, IEnumerable<Message> messages, string connectionWebSocketId = "")
        {
            //var serializedMessage = JsonConvert.SerializeObject(message);

            var messageString = string.Join(",", messages.Select(m => m.Text));

            var buffer = Encoding.UTF8.GetBytes(messageString);

            if (!string.IsNullOrEmpty(connectionWebSocketId))
            {
                if (_connections.TryGetValue(connectionWebSocketId, out var webSocket))
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            else
            {
                foreach (var connection in _connections)
                {
                    if (connection.Value.State == WebSocketState.Open)
                    {
                        await connection.Value.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }            
        }

        // Другие методы
    }

    public class MyWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MyWebSocketManager _webSocketManager;

        public MyWebSocketMiddleware(RequestDelegate next, MyWebSocketManager webSocketManager, string s)
        {
            _next = next;
            _webSocketManager = webSocketManager;
        }

        public async Task InvokeAsync(HttpContext context, VehicleQuotesContext _vehicleQuotesContext)
        {
            string fullUrl = context.Request.GetDisplayUrl();

            if (context.WebSockets.IsWebSocketRequest)
            {
                /*if (context.User.Identity.IsAuthenticated)
                {*/
                    await _webSocketManager.HandleWebSocketRequest(context, _vehicleQuotesContext);
                /*}
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }*/
            }

            await _next(context);
        }
    }
}
