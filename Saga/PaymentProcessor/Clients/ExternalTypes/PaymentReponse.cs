using PaymentProcessor.Data.Entities;

namespace PaymentProcessor.Clients.ExternalTypes
{
    public class PaymentReponse
    {
        public string OrderId { get; set; }
        public string AuthNumber { get; set; } = null;
        public PaymentStatus PaymentStatus { get; set; }


        public decimal Amount { get; set; }
    }
}
