using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class SubCategoryNotificationHub : Hub<ISubCategoryNotification>
    {
        public async Task SendSubCategoryNotification(bool value)
        {
            await Clients.All.SendSubCategoryNotification(value);
        }
    }
}