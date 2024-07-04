using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class PublisherNotificationHub : Hub<IPublisherNotification>
    {
        public async Task SendPublisherNotification(bool value)
        {
            await Clients.All.SendPublisherNotification(value);
        }
    }
}
