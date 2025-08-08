using classLib;
using Infrastructure.Messaging;
using LoggingAPI.Business.Service;
using LoggingAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ILogRepository, LogRepository>();
builder.Services.AddSingleton<ILogService,LogService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<RabbitMqConnectionManager>();
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



int maxRetries = 5;
int retryDelaySeconds = 3;

for (int attempt = 1; attempt <= maxRetries; attempt++)
{
    try
    {
        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors();
        app.MapControllers();
        app.Run(); 
        break; 
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[App] Failed to start app (attempt {attempt}): {ex.Message}");

        if (attempt == maxRetries)
        {
            Console.WriteLine("[App] Max retry attempts reached. Exiting.");
            throw; 
        }

        Console.WriteLine($"[App] Waiting {retryDelaySeconds} seconds before retry...");
        Thread.Sleep(retryDelaySeconds * 1000); 
    }
}

