namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface ICategoryNotification
    {
        Task SendCategoryNotification(bool value);
    }
}