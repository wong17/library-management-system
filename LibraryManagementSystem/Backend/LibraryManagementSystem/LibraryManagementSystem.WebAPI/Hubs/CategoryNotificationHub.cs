using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class CategoryNotificationHub : Hub<ICategoryNotification>
    {
        public async Task SendCategoryNotification(bool value)
        {
            await Clients.All.SendCategoryNotification(value);
        }
    }
}