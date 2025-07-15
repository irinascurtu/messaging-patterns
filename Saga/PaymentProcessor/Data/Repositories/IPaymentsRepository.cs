using PaymentProcessor.Data.Entities;

namespace PaymentProcessor.Data.Repositories
{
    public interface IPaymentsRepository
    {
        public void SavePayment(Payment payment);
    }
}
