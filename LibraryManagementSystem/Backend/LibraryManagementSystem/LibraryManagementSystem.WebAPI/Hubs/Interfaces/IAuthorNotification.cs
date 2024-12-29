namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface IAuthorNotification
    {
        Task SendAuthorNotification(bool value);
    }
}