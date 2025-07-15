using Microsoft.EntityFrameworkCore;
using NServiceBus;
using OrdersApi.Infrastructure.Mappings;
using OrdersApiNSB.Data;
using OrdersApiNSB.Domain;
using OrdersApiNSB.Services;
using OrderRepository = OrdersApiNSB.Data.OrderRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddAutoMapper(typeof(OrderProfileMapping));
builder.Services.AddDbContext<OrdersContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add NServiceBus
builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("OrdersApiNSB");

    // Configure transport (RabbitMQ in this example)
    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
    transport.UseConventionalRoutingTopology(QueueType.Classic);
    transport.ConnectionString("host=localhost"); // Update with your RabbitMQ connection string

    // Configure persistence (if needed)
    var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
    persistence.ConnectionBuilder(
        connectionBuilder: () =>
            new Microsoft.Data.SqlClient.SqlConnection(
                context.Configuration.GetConnectionString("DefaultConnection")));
    persistence.SqlDialect<SqlDialect.MsSqlServer>();

    // Enable outbox for reliable messaging
    endpointConfiguration.EnableOutbox();

    // Configure error handling
    endpointConfiguration.SendFailedMessagesTo("error");
    endpointConfiguration.EnableInstallers();

    // Configure serialization
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

    // Define which types are events
    var conventions = endpointConfiguration.Conventions();
    conventions.DefiningEventsAs(type => type.Namespace == "Contracts.Events");

    return endpointConfiguration;
});

// Register dependencies
builder.Services.AddScoped<OrdersApiNSB.Data.IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderServiceNSB, OrderServiceNSB>();
// Add other dependencies (OrderRepository, etc.)

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrdersContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
