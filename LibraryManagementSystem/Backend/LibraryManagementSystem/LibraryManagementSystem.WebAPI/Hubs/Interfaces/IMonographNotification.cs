namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface IMonographNotification
    {
        Task SendMonographNotification(bool value);
    }
}