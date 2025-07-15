namespace ProductsApi.Data
{
    public static class DbContextExtenstion
    {

        public static void EnsureSeeded(this ProductContext context)
        {
            DataSeeder.SeedData(context);
        }

    }
}
