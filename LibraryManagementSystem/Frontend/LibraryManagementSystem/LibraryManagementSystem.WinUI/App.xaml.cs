using LibraryManagementSystem.WinUI.Activation;
using LibraryManagementSystem.WinUI.Contracts.Services;
using LibraryManagementSystem.WinUI.Helpers;
using LibraryManagementSystem.WinUI.Models;
using LibraryManagementSystem.WinUI.Notifications;
using LibraryManagementSystem.WinUI.Services;
using LibraryManagementSystem.WinUI.Services.Library;
using LibraryManagementSystem.WinUI.Services.Security;
using LibraryManagementSystem.WinUI.Services.University;
using LibraryManagementSystem.WinUI.ViewModels;
using LibraryManagementSystem.WinUI.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace LibraryManagementSystem.WinUI;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<IFileService, FileService>();

            services.AddScoped<AuthorService>();
            services.AddScoped<BookAuthorService>();
            services.AddScoped<BookLoanService>();
            services.AddScoped<BookService>();
            services.AddScoped<BookSubCategoryService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<MonographAuthorService>();
            services.AddScoped<MonographLoanService>();
            services.AddScoped<MonographService>();
            services.AddScoped<PublisherService>();
            services.AddScoped<SubCategoryService>();

            services.AddScoped<RoleService>();
            services.AddScoped<UserRoleService>();
            services.AddScoped<UserService>();

            services.AddScoped<CareerService>();
            services.AddScoped<StudentService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();

            services.AddTransient<MonographViewModel>();
            services.AddTransient<MonographPage>();
            services.AddTransient<BookViewModel>();
            services.AddTransient<BookPage>();

            services.AddTransient<AuthorViewModel>();
            services.AddTransient<AuthorPage>();
            services.AddTransient<SubCategoryViewModel>();
            services.AddTransient<SubCategoryPage>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<CategoryPage>();
            services.AddTransient<PublisherViewModel>();
            services.AddTransient<PublisherPage>();

            services.AddTransient<UserViewModel>();
            services.AddTransient<UserPage>();
            services.AddTransient<RoleViewModel>();
            services.AddTransient<RolePage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
