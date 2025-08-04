using AdvancedCopilotAPI.Data;
using AdvancedCopilotAPI.Factories;
using AdvancedCopilotAPI.Handlers;
using AdvancedCopilotAPI.Interfaces;
using AdvancedCopilotAPI.Repositories;
using AdvancedCopilotAPI.Security;
using AdvancedCopilotAPI.Services;
using AdvancedCopilotAPI.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AdvancedCopilotDb"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddHttpClient<IResilientHttpClient, ResilientHttpClient>();

builder.Services.AddScoped<INotificationFactory, NotificationFactory>();
builder.Services.AddScoped<EmailNotificationService>();
builder.Services.AddScoped<SmsNotificationService>();
builder.Services.AddScoped<PushNotificationService>();
builder.Services.AddScoped<InAppNotificationService>();

builder.Services.AddSingleton<InputSanitizer>();
builder.Services.AddSingleton<IPerformanceMonitor, PerformanceMonitor>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

public partial class Program { }
