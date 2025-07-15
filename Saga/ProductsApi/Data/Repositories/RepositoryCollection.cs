namespace ProductsApi.Data.Repositories
{
    public static class RepositoryCollection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
