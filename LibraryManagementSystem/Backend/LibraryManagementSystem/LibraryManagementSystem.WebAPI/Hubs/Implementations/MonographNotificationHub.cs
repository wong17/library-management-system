using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class MonographNotificationHub : Hub<IMonographNotification>
    {
        public async Task SendMonographNotification(bool value)
        {
            await Clients.All.SendMonographNotification(value);
        }
    }
}