using Microsoft.OpenApi.Models;
using LibraryManagementSystem.Bll.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
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

builder.Services.AddBusinessLogicLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - Library Management System";
    });
}

/* Habilitar CORS */
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
