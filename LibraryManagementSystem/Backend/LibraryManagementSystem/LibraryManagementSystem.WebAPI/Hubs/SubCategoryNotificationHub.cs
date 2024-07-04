using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class SubCategoryNotificationHub : Hub<ISubCategoryNotification>
    {
        public async Task SendSubCategoryNotification(bool value)
        {
            await Clients.All.SendSubCategoryNotification(value);
        }
    }
}
