namespace Contracts.Models
{
    public class PayOrderModel
    {
        public Guid OrderId { get;  set; }
        public decimal AmountPaid { get;  set; }
        public string PaymentMethod { get;  set; }
        public DateTime PaymentDate { get;  set; }

        // Shipping Information
        public string ShippingStreet { get;  set; }
        public string ShippingCity { get;  set; }
        public string ShippingState { get;  set; }
        public string ShippingPostalCode { get;  set; }
        public string ShippingCountry { get;  set; }
        public string ShippingMethod { get;  set; }
        public DateTime EstimatedDeliveryDate { get;  set; }

        // Contact Information
        public string ContactName { get;  set; }
        public string ContactPhoneNumber { get;  set; }
    }
}
