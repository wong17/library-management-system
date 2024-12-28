using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Implements.Library;
using LibraryManagementSystem.Dal.Repository.Implements.Security;
using LibraryManagementSystem.Dal.Repository.Implements.University;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Dal.Repository.Interfaces.University;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Dal.Configuration
{
    public static class ConfigureServiceDataAccess
    {
        public static void AddDataAccessLayer(this IServiceCollection services)
        {
            /* Core */
            services.AddScoped<ISqlConnector, SqlServerConnector>();
            /* Agregar servicios DALs Security */
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            /* Agregar servicios DALs University */
            services.AddScoped<ICareerRepository, CareerRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            /* Agregar servicios DALs Library */
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
        }
    }
}