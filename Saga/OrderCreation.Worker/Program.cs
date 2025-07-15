using Contracts.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Data;
using Orders.Domain;
using Orders.Service;
using OrdersApi.Infrastructure.Mappings;
using OrdersApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OrderCreation.Worker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<OrderContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                        options.EnableSensitiveDataLogging(true);
                    });


                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<IOrderService, OrderService>();
                    services.AddAutoMapper(typeof(OrderProfileMapping));

                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();
                        var entryAssembly = Assembly.GetEntryAssembly();
                        x.AddConsumers(entryAssembly);
                        x.AddConsumer<CreateOrderConsumer, CreateOrderConsumerDefinition>();
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.SendTopology.ErrorQueueNameFormatter = new MyCoolErrorQueueNameFormatter();
                            cfg.ReceiveEndpoint("create-order-command", e =>
                            {
                                e.ConfigureConsumer<CreateOrderConsumer>(context);
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
