using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Filters
{
    public class MyCoolFilter : IFilter<ConsumeContext>
    {
        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }

        public Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            context.ReceiveContext.TransportHeaders.TryGetHeader("Tenant-From-Send", out var tenant);
            Console.WriteLine("Hello from MyCoolFilter Middleware");
            return next.Send(context);
        }
    }
}
