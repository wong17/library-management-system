using LibraryManagementSystem.Bll.Configuration;
using LibraryManagementSystem.WebAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// CORS
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
// Swagger
builder.Services.AddSwaggerConfiguration();
// BLL
builder.Services.AddBusinessLogicLayer();
// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

/* Habilitar CORS */
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/* Hubs */
app.MapHubs();

app.Run();