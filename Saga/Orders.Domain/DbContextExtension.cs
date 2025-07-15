namespace Orders.Domain
{
    public static class DbContextExtenstion
    {

        public static void EnsureSeeded(this OrderContext context)
        {
            DataSeeder.SeedData(context);
        }

    }
}
