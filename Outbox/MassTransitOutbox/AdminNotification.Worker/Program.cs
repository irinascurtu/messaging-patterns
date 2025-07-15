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
                    #region Hidden

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
                    #endregion

                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();
                        #region Hidden
                        x.AddConsumer<OrderReceivedConsumer>();
                        x.AddConsumer<OrderCreatedNotification>();
                        #endregion

                        x.AddEntityFrameworkOutbox<OrderContext>(o =>
                        {
                            o.UseSqlServer();
                            o.DisableInboxCleanupService();
                            o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
                            o.UseBusOutbox();

                        });

                        x.AddConfigureEndpointsCallback((context, name, cfg) =>
                        {
                            cfg.UseEntityFrameworkOutbox<OrderContext>(context);//all
                        });

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint("order-created", e =>
                            {
                               // e.UseEntityFrameworkOutbox<OrderContext>(context);//only this
                                e.ConfigureConsumer<OrderCreatedNotification>(context);
                           });

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
