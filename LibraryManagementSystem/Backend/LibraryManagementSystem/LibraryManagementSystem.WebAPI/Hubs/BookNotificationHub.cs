using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class BookNotificationHub : Hub<IBookNotification>
    {
        public async Task SendBookNotification(bool value)
        {
            await Clients.All.SendBookNotification(value);
        }
    }
}
