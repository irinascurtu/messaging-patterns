using PaymentProcessor.Services;

namespace PaymentProcessor.Infrastructure
{
    public static class PaymentProcessCollection
    {
        public static IServiceCollection AddPayments(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentService, PaymentService>();
            return services;
        }

        public static IServiceCollection AddPayments(this IServiceCollection services, IConfiguration config)
        {
            return services;

        }
    }
}


