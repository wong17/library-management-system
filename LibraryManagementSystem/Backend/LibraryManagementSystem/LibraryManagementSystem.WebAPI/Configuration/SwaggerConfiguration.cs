using Microsoft.OpenApi.Models;

namespace LibraryManagementSystem.WebAPI.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Swagger UI - Library Management System",
                    Description = "API - Sistema para gestión de biblioteca universitaria, diseñado para llevar el control tanto de libros como monografías y " +
                                    "préstamos de estos realizados por los estudiantes.",
                    Contact = new OpenApiContact
                    {
                        Name = "Denis Wong",
                        Url = new Uri("https://github.com/wong17/library-management-system")
                    }
                });
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger UI - Library Management System";
            });
        }
    }
}