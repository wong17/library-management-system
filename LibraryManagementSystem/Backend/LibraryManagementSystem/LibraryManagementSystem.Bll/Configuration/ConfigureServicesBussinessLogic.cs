using LibraryManagementSystem.Bll.Implements.Library;
using LibraryManagementSystem.Bll.Implements.Security;
using LibraryManagementSystem.Bll.Implements.University;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Bll.Mapping;
using LibraryManagementSystem.Dal.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Bll.Configuration
{
    public static class ConfigureServicesBussinessLogic
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            /* Agregar primero servicios de Data Access Layer (DAL) */
            services.AddDataAccessLayer();
            /* Agregar BLLs Security */
            services.AddScoped<IUserBll, UserBll>();
            services.AddScoped<IRoleBll, RoleBll>();
            services.AddScoped<IUserRoleBll, UserRoleBll>();
            /* Agregar BLLs University */
            services.AddScoped<ICareerBll, CareerBll>();
            services.AddScoped<IStudentBll, StudentBll>();
            /* Agregar BLLs Library */
            services.AddScoped<IPublisherBll, PublisherBll>();
            services.AddScoped<ICategoryBll, CategoryBll>();
            services.AddScoped<ISubCategoryBll, SubCategoryBll>();
            services.AddScoped<IAuthorBll, AuthorBll>();
            services.AddScoped<IBookBll, BookBll>();
            services.AddScoped<IBookSubCategoryBll, BookSubCategoryBll>();
            services.AddScoped<IBookAuthorBll, BookAuthorBll>();
            services.AddScoped<IBookLoanBll, BookLoanBll>();
            services.AddScoped<IMonographBll, MonographBll>();
            services.AddScoped<IMonographLoanBll, MonographLoanBll>();
            services.AddScoped<IMonographAuthorBll, MonographAuthorBll>();
            /* Agregar configuración de AutoMapper */
            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }
    }
}
