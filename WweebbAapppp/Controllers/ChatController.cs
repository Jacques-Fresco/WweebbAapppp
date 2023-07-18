    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using NamespaceChat;
    using Newtonsoft.Json.Linq;
    using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using VehicleQuotes.ResourceModels;
    using WweebbAapppp.Hubs;
    using WweebbAapppp.Models;
    using WweebbAapppp.MyWebSocket;

namespace WweebbAapppp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/message")]
    public class MessageController : Controller
    {
        private readonly IHubContext<MessageHub> _messageHub;
        private readonly VehicleQuotesContext _vehicleQuotesContext;

        public MessageController([NotNull] IHubContext<MessageHub> messageHub, VehicleQuotesContext vehicleQuotesContext)
        {
            _messageHub = messageHub;
            _vehicleQuotesContext = vehicleQuotesContext;
        }

        [HttpPost("сreate-message")]
        public async Task<IActionResult> Create(MessageDto messageDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            var payload = jwtToken.Payload.SerializeToJson();
            //var claimValue = jwtToken.Payload["propertyName"];
            JObject payloadJson = JObject.Parse(payload);
            string userId = (string)payloadJson[ClaimTypes.NameIdentifier];

            var message = new Message
            {
                //Id = Guid.NewGuid().ToString(),
                Text = messageDto.message,
                SentAt = DateTime.Now,
                UserId = messageDto.senderid,
                ChatId = messageDto.chatid
            };

            try
            {
                _vehicleQuotesContext.Messages.Add(message);
                await _vehicleQuotesContext.SaveChangesAsync();

                var messages = _vehicleQuotesContext.Messages.ToList();

                await _messageHub.Clients.All.SendAsync("sendToReact", messages); //_vehicleQuotesContext.Messages);
            }
            catch (Exception ex)
            {
                // Обработка исключения или вывод сообщения об ошибке
            }
            
            return Ok();
        }

        /*[HttpGet("send-message")]
        public async Task<IActionResult> SendMessage([FromBody]MessageDto messageDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            var payload = jwtToken.Payload.SerializeToJson();
            //var claimValue = jwtToken.Payload["propertyName"];
            JObject payloadJson = JObject.Parse(payload);
            string userId = (string)payloadJson[ClaimTypes.NameIdentifier];

            var message = new Message
            {
                //Id = Guid.NewGuid().ToString(),
                Text = messageDto.text,
                SentAt = DateTime.Now,
                UserId = messageDto.senderid,
                ChatId = messageDto.chatid
            };

            try
            {
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Обработка исключения или вывод сообщения об ошибке
            }

            // Отправить сообщение всем клиентам через SignalR
            //await _hubContext.Clients.All.SendAsync("newMessage", message);

            //_webSocketManager.

            return Ok();
        }*/

        [HttpGet("fetch-messages/{id}")]
        public async Task<IActionResult> FetchDataChat(int id)
        {
            try
            {
                var chat = await _vehicleQuotesContext.Chat.FindAsync(id);
        

                if (chat == null)
                {
                    chat = new Chat
                    {
                        Name = "MYCHAT",
                        CreatedAt = DateTime.Now
                    };

                    try
                    {
                        _vehicleQuotesContext.Chat.Add(chat);
                        await _vehicleQuotesContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Обработка исключения или вывод сообщения об ошибке
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения или вывод сообщения об ошибке
            }

            var messages = await _vehicleQuotesContext.Messages
                .Where(m => m.ChatId == id)
                .ToListAsync();

            if (messages == null)
            {
                return Ok(new List<Message>()); // Пустой список сообщений
            }

            return Ok(new { messages });
        }
    }


}
