namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface ILoanNotification
    {
        Task SendLoanNotification(bool loanCreated);
    }
}