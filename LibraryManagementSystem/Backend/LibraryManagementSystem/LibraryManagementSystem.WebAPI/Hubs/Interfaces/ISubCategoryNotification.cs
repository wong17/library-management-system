namespace LibraryManagementSystem.WebAPI.Hubs.Interfaces
{
    public interface ISubCategoryNotification
    {
        Task SendSubCategoryNotification(bool value);
    }
}