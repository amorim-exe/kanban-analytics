using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Revo.Application.Interfaces;
using Revo.Infrastructure.Cache;
using Revo.Infrastructure.Data;
using Revo.Infrastructure.Repositories;
using Revo.Infrastructure.Services;
using StackExchange.Redis;
using Revo.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RevoDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration =
        builder.Configuration["Redis:ConnectionString"];

    if (string.IsNullOrWhiteSpace(configuration))
    {
        throw new InvalidOperationException(
            "Redis:ConnectionString não foi configurada no appsettings.json.");
    }

    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration
                    .GetSection("Cors:AllowedOrigins")
                    .Get<string[]>()
                ?? new[] { "http://localhost:5173" })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Revo API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.MapHub<RevoHub>("/hubs/revo");

app.Run();
