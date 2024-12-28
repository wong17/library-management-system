using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class AuthorNotificationHub : Hub<IAuthorNotification>
    {
        public async Task SendAuthorNotification(bool value)
        {
            await Clients.All.SendAuthorNotification(value);
        }
    }
}