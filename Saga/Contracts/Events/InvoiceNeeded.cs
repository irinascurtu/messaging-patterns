using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class InvoiceNeeded
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
