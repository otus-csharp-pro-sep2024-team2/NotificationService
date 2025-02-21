using Microsoft.EntityFrameworkCore;
using NotificationService.API.Extension;
using NotificationService.Infrastructure.Implemetations;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Services.Implementations;
using NotificationService.Infrastructure.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration for logging, swagger, etc. ---
builder.Services.AddHttpClient(); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNotificationServices(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddHostedService<MessageConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();