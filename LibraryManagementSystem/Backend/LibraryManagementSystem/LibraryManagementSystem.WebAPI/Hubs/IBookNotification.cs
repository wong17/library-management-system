namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface IBookNotification
    {
        Task SendBookNotification(bool value);
    }
}
