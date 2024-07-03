
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class MonographNotificationHub : Hub<IMonographNotification>
    {
        public async Task SendMonographStillAvailableNotification(bool value)
        {
            await Clients.All.SendMonographStillAvailableNotification(value);
        }
    }
}
