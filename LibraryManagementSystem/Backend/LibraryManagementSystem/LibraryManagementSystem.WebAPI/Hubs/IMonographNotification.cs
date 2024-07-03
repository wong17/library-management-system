namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface IMonographNotification
    {
        Task SendMonographStillAvailableNotification(bool value);
    }
}
