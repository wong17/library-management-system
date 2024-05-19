using LibraryManagementSystem.Bll.Implements;
using LibraryManagementSystem.Bll.Interfaces;
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
            /* Agregar BLLs */
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
