using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Infrastructure
{
    public class MyCoolErrorQueueNameFormatter : IErrorQueueNameFormatter
    {
        public string FormatErrorQueueName(string queueName)
        {
            return queueName + "-awesome_error";
        }
    }
}
