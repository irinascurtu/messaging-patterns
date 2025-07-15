using Microsoft.EntityFrameworkCore;
using Stocks.Repository;
using Stocks.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StockContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
        .EnableSensitiveDataLogging()); // Optional: shows parameter values in SQL logs);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IProductStockRepository, ProductStockRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
