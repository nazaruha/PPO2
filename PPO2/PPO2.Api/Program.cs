using PPO2.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PPO2.Core;

var builder = WebApplication.CreateBuilder(args);

//// Add Template repository
// Database repositories
builder.Services.AddRepositories();

//// Add services to the container.
// Database context
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // get connection string from appsettings.json
builder.Services.AddDbContext(connectionString); // connect to our DB
// Add core services
builder.Services.AddCoreServices();
// Add core automapper
builder.Services.AddAutoMapper();

builder.Services.CorsConfiguration();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS. To communicate with react
//app.UseCors(options => options
//    .SetIsOriginAllowed(origin => true)
//    .AllowAnyHeader()
//    .AllowCredentials());
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
