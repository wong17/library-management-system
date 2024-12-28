namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface ICategoryNotification
    {
        Task SendCategoryNotification(bool value);
    }
}