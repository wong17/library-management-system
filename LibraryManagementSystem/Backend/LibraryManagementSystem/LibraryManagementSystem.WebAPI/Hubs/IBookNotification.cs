namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface IBookNotification
    {
        Task SendBookStillAvailableNotification(bool value);
    }
}
