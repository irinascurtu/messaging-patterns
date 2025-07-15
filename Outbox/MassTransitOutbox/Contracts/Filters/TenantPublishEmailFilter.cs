using Contracts.Models;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Filters
{
    public class TenantPublishEmailFilter : IFilter<PublishContext<Email>>
    {
        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }

        public Task Send(PublishContext<Email> context, IPipe<PublishContext<Email>> next)
        {
            Console.WriteLine("This is the email filter!");
            return next.Send(context);
        }
    }
}
