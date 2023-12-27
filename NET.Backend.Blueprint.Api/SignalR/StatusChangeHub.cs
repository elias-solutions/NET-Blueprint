using Microsoft.AspNetCore.SignalR;

namespace NET.Backend.Blueprint.Api.SignalR
{
    public class StatusChangeHub : Hub
    {
        public async Task SendMessage(Guid id, string key, string value)
        {
            await Clients.All.SendAsync("StatusChange", id, key, value);
        }
    }
}
