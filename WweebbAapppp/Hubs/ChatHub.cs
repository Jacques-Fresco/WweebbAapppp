using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WweebbAapppp.Hubs
{
    /*[Authorize]*/
    public class MessageHub : Hub
    {
        /*public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("newMessage", message);
        }*/
    }
}