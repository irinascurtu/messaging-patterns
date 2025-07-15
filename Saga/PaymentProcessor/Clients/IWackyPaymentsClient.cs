using PaymentProcessor.Clients.ExternalTypes;

namespace PaymentProcessor.Clients
{
    public interface IWackyPaymentsClient
    {
        ValueTask<PaymentReponse> CapturePayments(PaymentRequest request);
    }
}