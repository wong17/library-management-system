namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface IPublisherNotification
    {
        Task SendPublisherNotification(bool value);
    }
}