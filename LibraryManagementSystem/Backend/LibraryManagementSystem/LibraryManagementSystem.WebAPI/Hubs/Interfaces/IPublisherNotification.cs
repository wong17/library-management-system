namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface IPublisherNotification
    {
        Task SendPublisherNotification(bool value);
    }
}