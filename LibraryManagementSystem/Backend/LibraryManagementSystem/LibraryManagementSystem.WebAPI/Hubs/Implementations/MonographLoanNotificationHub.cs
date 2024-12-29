using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs.Implementations
{
    public class MonographLoanNotificationHub : Hub<ILoanNotification>
    {
        public async Task SendMonographLoanNotification(bool loanCreated)
        {
            await Clients.Others.SendLoanNotification(loanCreated);
        }
    }
}