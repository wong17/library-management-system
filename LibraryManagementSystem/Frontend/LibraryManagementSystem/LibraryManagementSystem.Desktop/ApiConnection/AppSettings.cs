using System.Configuration;

namespace LibraryManagementSystem.Desktop.ApiConnection
{
    public class AppSettings
    {
        private static readonly Lazy<AppSettings> instance = new(() => new AppSettings());
        public static AppSettings Instance => instance.Value;

        public string? UrlApi { get; set; }

        public string? CareerEndpointUrl { get; set; }
        public string? StudentEndpointUrl { get; set; }

        public string? UserEndpointUrl { get; set; }
        public string? RoleEndpointUrl { get; set; }
        public string? UserRoleEndpointUrl { get; set; }

        public string? AuthorEndpointUrl { get; set; }
        public string? BookAuthorEndpointUrl { get; set; }
        public string? BookEndpointUrl { get; set; }
        public string? BookLoanEndpointUrl { get; set; }
        public string? BookSubCategoryEndpointUrl { get; set; }
        public string? CategoryEndpointUrl { get; set; }
        public string? MonographAuthorEndpointUrl { get; set; }
        public string? MonographEndpointUrl { get; set; }
        public string? MonographLoanEndpointUrl { get; set; }
        public string? PublisherEndpointUrl { get; set; }
        public string? SubCategoryEndpointUrl { get; set; }

        private AppSettings()
        {
            UrlApi = ConfigurationManager.AppSettings["UrlApi"]?.ToString();

            CareerEndpointUrl = ConfigurationManager.AppSettings["CareerEndpointUrl"]?.ToString();
            StudentEndpointUrl = ConfigurationManager.AppSettings["StudentEndpointUrl"]?.ToString();

            UserEndpointUrl = ConfigurationManager.AppSettings["UserEndpointUrl"]?.ToString();
            RoleEndpointUrl = ConfigurationManager.AppSettings["RoleEndpointUrl"]?.ToString();
            UserRoleEndpointUrl = ConfigurationManager.AppSettings["UserRoleEndpointUrl"]?.ToString();

            AuthorEndpointUrl = ConfigurationManager.AppSettings["AuthorEndpointUrl"]?.ToString();
            BookAuthorEndpointUrl = ConfigurationManager.AppSettings["BookAuthorEndpointUrl"]?.ToString();
            BookEndpointUrl = ConfigurationManager.AppSettings["BookEndpointUrl"]?.ToString();
            BookLoanEndpointUrl = ConfigurationManager.AppSettings["BookLoanEndpointUrl"]?.ToString();
            BookSubCategoryEndpointUrl = ConfigurationManager.AppSettings["BookSubCategoryEndpointUrl"]?.ToString();
            CategoryEndpointUrl = ConfigurationManager.AppSettings["CategoryEndpointUrl"]?.ToString();
            MonographAuthorEndpointUrl = ConfigurationManager.AppSettings["MonographAuthorEndpointUrl"]?.ToString();
            MonographEndpointUrl = ConfigurationManager.AppSettings["MonographEndpointUrl"]?.ToString();
            MonographLoanEndpointUrl = ConfigurationManager.AppSettings["MonographLoanEndpointUrl"]?.ToString();
            PublisherEndpointUrl = ConfigurationManager.AppSettings["PublisherEndpointUrl"]?.ToString();
            SubCategoryEndpointUrl = ConfigurationManager.AppSettings["SubCategoryEndpointUrl"]?.ToString();
        }
    }
}
