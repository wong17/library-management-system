using LibraryManagementSystem.Bll.Configuration;
using LibraryManagementSystem.WebAPI.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add SignalR service
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",    // admin
            "http://localhost:4201",    // bibliotecario
            "http://localhost:4202")    // estudiante
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Permite las credenciales (necesario para WebSockets)
    });
});
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
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "Swagger UI - Library Management System";
});
//}

/* Habilitar CORS */
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/* Hubs */
app.MapHub<BookLoanNotificationHub>("/hubs/bookloan_hub");
app.MapHub<MonographLoanNotificationHub>("/hubs/monographloan_hub");
app.MapHub<BookLoanNotificationHub>("/hubs/book_hub");
app.MapHub<MonographNotificationHub>("/hubs/monograph_hub");
app.MapHub<PublisherNotificationHub>("/hubs/publisher_hub");
app.MapHub<CategoryNotificationHub>("/hubs/category_hub");
app.MapHub<AuthorNotificationHub>("/hubs/author_hub");
app.MapHub<SubCategoryNotificationHub>("/hubs/sub_category_hub");

app.Run();