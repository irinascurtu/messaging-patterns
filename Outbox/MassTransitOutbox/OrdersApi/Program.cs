using Contracts.Events;
using Contracts.Filters;
using Contracts.Infrastructure;
using Contracts.Models;
using Contracts.Response;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.Domain;
using Orders.Service;
using OrdersApi.Consumers;
using OrdersApi.Infrastructure.Mappings;
using OrdersApi.Service.Clients;
using OrdersApi.Services;
using System.Reflection;

namespace OrdersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddAutoMapper(typeof(OrderProfileMapping));
            builder.Services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());

            });

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
         

            builder.Services.AddHttpClient<IProductStockServiceClient, ProductStockServiceClient>();

            builder.Services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddEntityFrameworkOutbox<OrderContext>(o => //1. tells the context to create tables
                {
                    o.UseSqlServer();
                    o.UseBusOutbox(x => x.DisableDeliveryService());
                    o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                    //o.QueryDelay = TimeSpan.FromSeconds(5);
                });

                x.AddConsumer<OrderCreatedConsumer, OrderCreatedConsumerDefinition>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                    {
                        r.Immediate(2);
                    });

                    cfg.SendTopology.ErrorQueueNameFormatter = new MyCoolErrorQueueNameFormatter();

                    //cfg.ReceiveEndpoint("order-created", e =>
                    //{
                    //    cfg.UseMessageRetry(x => x.Interval(1, 200));

                    //    e.ConfigureConsumer<OrderCreatedConsumer>(context);
                    //});

                    cfg.ConfigureEndpoints(context);
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<OrderContext>().Database.EnsureCreated();
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
