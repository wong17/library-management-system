using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class CategoryNotificationHub : Hub<ICategoryNotification>
    {
        public async Task SendCategoryNotification(bool value)
        {
            await Clients.All.SendCategoryNotification(value);
        }
    }
}