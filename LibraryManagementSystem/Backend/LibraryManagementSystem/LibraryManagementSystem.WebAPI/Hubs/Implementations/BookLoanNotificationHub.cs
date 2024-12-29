using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class BookLoanNotificationHub : Hub<ILoanNotification>
    {
        public async Task SendBookLoanNotification(bool loanCreated)
        {
            await Clients.Others.SendLoanNotification(loanCreated);
        }
    }
}