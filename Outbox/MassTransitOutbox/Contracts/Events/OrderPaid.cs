using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class OrderPaid
    {
        public Guid OrderId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        // Shipping Information
        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }

        //public bool IsGift { get; set; }

        // Contact Information
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
    }
}
