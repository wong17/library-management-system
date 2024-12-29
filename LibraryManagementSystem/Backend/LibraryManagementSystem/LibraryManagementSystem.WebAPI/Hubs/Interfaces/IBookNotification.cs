namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface IBookNotification
    {
        Task SendBookNotification(bool value);
    }
}