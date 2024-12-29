using LibraryManagementSystem.WebAPI.Hubs.Implementations;

namespace LibraryManagementSystem.WebAPI.Configuration
{
    public static class SignalRHubConfiguration
    {
        public static void MapHubs(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<BookLoanNotificationHub>("/hubs/bookloan_hub");
            endpoints.MapHub<MonographLoanNotificationHub>("/hubs/monographloan_hub");
            endpoints.MapHub<BookLoanNotificationHub>("/hubs/book_hub");
            endpoints.MapHub<MonographNotificationHub>("/hubs/monograph_hub");
            endpoints.MapHub<PublisherNotificationHub>("/hubs/publisher_hub");
            endpoints.MapHub<CategoryNotificationHub>("/hubs/category_hub");
            endpoints.MapHub<AuthorNotificationHub>("/hubs/author_hub");
            endpoints.MapHub<SubCategoryNotificationHub>("/hubs/sub_category_hub");
        }
    }
}