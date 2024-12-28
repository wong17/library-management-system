using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class BookLoanNotificationHub : Hub<ILoanNotification>
    {
        public async Task SendBookLoanNotification(bool loanCreated)
        {
            await Clients.Others.SendLoanNotification(loanCreated);
        }
    }
}