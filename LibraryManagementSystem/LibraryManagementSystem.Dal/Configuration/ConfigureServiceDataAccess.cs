using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Implements;
using LibraryManagementSystem.Dal.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Dal.Configuration
{
    public static class ConfigureServiceDataAccess
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            /* Core */
            services.AddScoped<ISqlConnector, SqlServerConnector>();
            /* Agregar servicios DALs */
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookLoanRepository, BookLoanRepository>();
            services.AddScoped<IBookSubCategoryRepository, BookSubCategoryRepository>();
            services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
            services.AddScoped<IMonographRepository, MonographRepository>();
            services.AddScoped<IMonographLoanRepository, MonographLoanRepository>();
            services.AddScoped<IMonographAuthorRepository, MonographAutorRepository>();

            return services;
        }
    }
}
