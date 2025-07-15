
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProductsApi.Data;
using ProductsApi.Data.Repositories;
using ProductsApi.Service;
using System.Threading.RateLimiting;

namespace ProductsApi
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

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ProductsApi"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.EnrichWithIDbCommand = (activity, command) =>
                        {
                            var stateDisplayName = $"{command.CommandType} main";
                            activity.DisplayName = stateDisplayName;
                            activity.SetTag("db.name", stateDisplayName);
                        };
                    });

                tracing.AddOtlpExporter();
            });

            builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(
              new MemoryCacheOptions
              {
                  TrackStatistics = true,
                  SizeLimit = 50 // Products.
              }));


            builder.Services.AddControllers(options =>
            {

                options.ReturnHttpNotAcceptable = true;
                options.AllowEmptyInputInBodyModelBinding = true;
                options.RespectBrowserAcceptHeader = true;
            });


            builder.Services.AddDbContext<ProductContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging()); // Optional: shows parameter values in SQL logs);


            //builder.Services.AddRateLimiter(_ => _
            //.AddSlidingWindowLimiter(policyName: "sliding",
            //options =>
            //{
            //    options.PermitLimit = 1;
            //    options.Window = TimeSpan.FromSeconds(12);
            //    options.SegmentsPerWindow = 1;

            //}));
            builder.Services.AddRateLimiter(options => options
                .AddConcurrencyLimiter(policyName: "concurrency", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 1;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                }));
            #region Versioning



            builder.Services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Asp.Versioning.ApiVersion(2, 0);
                //   o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                //  o.ApiVersionReader = new MediaTypeApiVersionReader();
                // o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
                //o.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
                // o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
                //.ApiVersionReader = builder.Template("application/vnd.my.company.v{version}+json")
                //                         .Build();
                //

                o.ApiVersionReader = new MediaTypeApiVersionReaderBuilder().Template("application/vnd.example.v{api-version}+json").Build();
            })
                .AddMvc().AddApiExplorer(
                    options =>
                    {
                        // the default is ToString(), but we want "'v'major[.minor][-status]"
                        options.GroupNameFormat = "'v'VVV";
                    }); ;


            #endregion


            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            builder.Services.AddSwaggerGen(c =>
            {


                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"My API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
                }

                c.ResolveConflictingActions(apiDescriptions =>
                {

                    var descriptions = apiDescriptions as ApiDescription[] ?? apiDescriptions.ToArray();
                    var first = descriptions.First(); // build relative to the 1st method
                    var parameters = descriptions.SelectMany(d => d.ParameterDescriptions).ToList();
                    var actionDescriptions = descriptions.Select(d => d.ActionDescriptor).ToList();


                    var mediatypes = descriptions.SelectMany(d => d.SupportedRequestFormats)
                    .Select(x => x.MediaType).Distinct().ToList();

                    first.ParameterDescriptions.Clear();
                    // add parameters and make them optional
                    foreach (var parameter in parameters)
                        if (first.ParameterDescriptions.All(x => x.Name != parameter.Name))
                        {
                            first.ParameterDescriptions.Add(new ApiParameterDescription
                            {
                                ModelMetadata = parameter.ModelMetadata,
                                Name = parameter.Name,
                                ParameterDescriptor = parameter.ParameterDescriptor,
                                Source = parameter.Source,
                                IsRequired = false,
                                DefaultValue = null
                            });
                        }
                    return first;
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // Add a Swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }

                });

                app.UseDeveloperExceptionPage();
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<ProductContext>().Database.EnsureCreated();
                    serviceScope.ServiceProvider.GetService<ProductContext>().EnsureSeeded();
                }
            }

            app.UseRateLimiter();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
