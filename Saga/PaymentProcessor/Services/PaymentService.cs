using PaymentProcessor.Data.Entities;
using PaymentProcessor.Data.Repositories;

namespace PaymentProcessor.Services
{
    /// <summary>
    /// Takes care of the saving of the payment 
    /// 
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository paymentRepository;

        public PaymentService(IPaymentsRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public void SavePayment(Payment payment)
        {
            paymentRepository.SavePayment(payment);
        }
    }
}
