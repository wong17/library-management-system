namespace LibraryManagementSystem.WebAPI.Hubs
{
    public interface ISubCategoryNotification
    {
        Task SendSubCategoryNotification(bool value);
    }
}
