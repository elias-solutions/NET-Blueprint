using Microsoft.AspNetCore.SignalR;

namespace NET.Backend.Blueprint.Api.SignalR
{
    public class StatusChangeHub : Hub
    {
        public async Task SendMessage(Guid id, string key, string value)
        {
            if (Clients == null)
            {
                return;
            }
            
            await Clients.All.SendAsync("StatusChange", id, key, value);
        }
    }
}
