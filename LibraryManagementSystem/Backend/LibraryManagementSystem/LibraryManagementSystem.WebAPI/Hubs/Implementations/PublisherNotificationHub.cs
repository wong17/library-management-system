using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class PublisherNotificationHub : Hub<IPublisherNotification>
    {
        public async Task SendPublisherNotification(bool value)
        {
            await Clients.All.SendPublisherNotification(value);
        }
    }
}