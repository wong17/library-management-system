namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface IAuthorNotification
    {
        Task SendAuthorNotification(bool value);
    }
}
