using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class MonographNotificationHub : Hub<IMonographNotification>
    {
        public async Task SendMonographNotification(bool value)
        {
            await Clients.All.SendMonographNotification(value);
        }
    }
}