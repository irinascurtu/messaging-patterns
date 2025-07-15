using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

namespace AdminNotification.Worker
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
                        //.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
                        options.EnableSensitiveDataLogging(true);

                        options.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
                    });

                    services.AddAutoMapper(typeof(OrderProfileMapping));
                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<IOrderService, OrderService>();

                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        //var entryAssembly = Assembly.GetEntryAssembly();
                        //x.AddConsumers(entryAssembly);
                        x.AddConsumer<OrderReceivedConsumer>();
                        x.AddConsumer<OrderCreatedNotification>();
                        x.AddEntityFrameworkOutbox<OrderContext>(o =>
                        {
                            o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                            //     o.QueryDelay = TimeSpan.FromSeconds(5);
                            o.UseSqlServer();
                            //o.DisableInboxCleanupService();
                            o.UseBusOutbox();
                            
                        });

                        x.AddConfigureEndpointsCallback((context, name, cfg) =>
                        {
                            cfg.UseEntityFrameworkOutbox<OrderContext>(context);
                        });

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint("order-created", e =>
                           {

                               e.ConfigureConsumer<OrderCreatedNotification>(context);
                           });

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
