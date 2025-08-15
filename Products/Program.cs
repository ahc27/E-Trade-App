using classLib;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.Data.Repositories;
using Products.Infrastructures.Messaging;
using Products.Infrastructures.Mapping;
using Products.Service;
using Products.Validators;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAutoMapper(typeof(ProductMapping).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers();
builder.Services.AddSingleton<RabbitMqConnectionManager>();
builder.Services.AddSingleton<RabbitMqProducer>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Database Migration (Container baþlangýcýnda)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate(); // Container açýlýrken migration uygulanacak
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Database migration failed");
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();