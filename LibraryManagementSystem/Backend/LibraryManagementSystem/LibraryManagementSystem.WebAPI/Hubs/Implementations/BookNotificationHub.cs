using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class BookNotificationHub : Hub<IBookNotification>
    {
        public async Task SendBookNotification(bool value)
        {
            await Clients.All.SendBookNotification(value);
        }
    }
}