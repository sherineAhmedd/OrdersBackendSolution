using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Orders.API.Middlewares;
using Orders.BLL.ServiceInterfaces;
using Orders.BLL.Services;
using Orders.DAL.Context;
using Orders.DAL.Data.Seeding;
using Orders.DAL.Repositories;
using Orders.DAL.RepositoryInterfaces;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Connect to Redis 
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
// "redis:6379" 

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisConnectionString + ",abortConnect=false"));
builder.Services.AddControllers();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Use absolute URL to avoid /openapi fetch errors
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Orders API",
        Version = "v1"
    });
});


// Register Redis cache service
builder.Services.AddScoped<ICacheService, RedisCacheService>();


// Register services
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.SeedData(db);
}
app.UseMiddleware<ExceptionMiddleware>();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        // Absolute URL fixes fetch errors
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Orders API v1");
        // UI available at /swagger
        c.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
