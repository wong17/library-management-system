using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class BookNotificationHub : Hub<IBookNotification>
    {
        public async Task SendBookStillAvailableNotification(bool value)
        {
            await Clients.All.SendBookStillAvailableNotification(value);
        }
    }
}
