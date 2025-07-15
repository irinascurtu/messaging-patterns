using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Filters
{
    public class TenantSendFilter<T> : IFilter<SendContext<T>> where T : class
    {
        private readonly Tenant tenant;

        public TenantSendFilter(Tenant tenant)
        {
            this.tenant = tenant;
        }
        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }

        public Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
        {
            if (!string.IsNullOrEmpty(tenant.TenantId))
            {
                context.Headers.Set("Tenant-From-Send", tenant.TenantId);
            }

            return next.Send(context);
        }
    }
}
