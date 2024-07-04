namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface IMonographNotification
    {
        Task SendMonographNotification(bool value);
    }
}
