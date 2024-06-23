using Microsoft.AspNetCore.SignalR;

namespace LibraryManagementSystem.WebAPI.Hubs
{
    public class MonographLoanNotificationHub : Hub<ILoanNotification>
    {
        public async Task SendMonographLoanNotification(bool loanCreated)
        {
            await Clients.Others.SendLoanNotification(loanCreated);
        }
    }
}
