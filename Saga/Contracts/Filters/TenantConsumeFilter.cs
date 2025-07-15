using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Filters
{
    public class TenantConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly Tenant tenant;

        public TenantConsumeFilter()
        {
            this.tenant = new Tenant();
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("TenantConsume");
        }

        public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var tenantString = context.Headers.Get<string>("Tenant-From-Send");
            if (!string.IsNullOrEmpty(tenantString))
            {
                this.tenant.TenantId = tenantString;
            }


            return next.Send(context);
        }
    }
}
