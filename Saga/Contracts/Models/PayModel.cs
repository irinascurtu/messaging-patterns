using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    public class PayModel
    {
        public Guid OrderId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
    }
}
