using System.ComponentModel.DataAnnotations;

namespace WackyPayments.Models
{
    public class PaymentResponse
    {
        public PaymentResponse(string id, decimal amountRequested)
        {
            OrderId = id;
            Amount = amountRequested;
        }

        [Required]
        public string OrderId { get; set; }
        public string AuthNumber { get; set; } = null;

        public PaymentStatus PaymentStatus { get; set; }


        public decimal Amount { get; set; }
        public string GetNumber()
        {


            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }

}
