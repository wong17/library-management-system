namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface ILoanNotification
    {
        Task SendLoanNotification(bool loanCreated);
    }
}