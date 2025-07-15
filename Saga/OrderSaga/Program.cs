using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Data;
using Orders.Domain;
using Orders.Domain.StateMachine;
using Orders.Service;
using OrderSaga.StateMachine;
using OrdersApi.Infrastructure.Mappings;
using OrdersApi.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static MassTransit.Logging.OperationName;

namespace OrderSaga
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


                        x.SetEntityFrameworkSagaRepositoryProvider(r =>
                        {
                            r.ExistingDbContext<OrderContext>();
                            r.UseSqlServer();
                        });

                      
                        x.AddSagaStateMachine<OrderStateMachine, OrderStateData>()
                        .EntityFrameworkRepository(r =>
                        {
                            r.ExistingDbContext<OrderContext>();
                            r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                        });

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);
                        services.AddQuartz(q =>
                        {
                            q.UseMicrosoftDependencyInjectionJobFactory();
                        });
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.UseInMemoryScheduler();
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
