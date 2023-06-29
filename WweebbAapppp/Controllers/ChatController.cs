using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NamespaceChat;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VehicleQuotes.ResourceModels;
using WweebbAapppp.Hubs;
using WweebbAapppp.Models;

namespace WweebbAapppp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly VehicleQuotesContext _dbContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatController(IHubContext<ChatHub> hubContext, 
                            VehicleQuotesContext dbContext) 
                            //,IHttpContextAccessor httpContextAccessor)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
            //_httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("send-message")]
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
            await _hubContext.Clients.All.SendAsync("newMessage", message);

            return Ok();
        }

        [HttpGet("fetch-messages/{id}")]
        public async Task<IActionResult> FetchDataChat(int id)
        {
            try
            {
                var chat = await _dbContext.Chat.FindAsync(id);
            

                if (chat == null)
                {
                    chat = new Chat
                    {
                        Name = "MYCHAT",
                        CreatedAt = DateTime.Now
                    };

                    try
                    {
                        _dbContext.Chat.Add(chat);
                        await _dbContext.SaveChangesAsync();
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

            var messages = await _dbContext.Messages
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
