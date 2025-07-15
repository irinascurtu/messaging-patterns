using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Filters
{
    public class TenantPublishFilter<T> : IFilter<PublishContext<T>> where T : class
    {
        private readonly Tenant tenant;

        public TenantPublishFilter(Tenant tenant)
        {
            this.tenant = tenant;
        }

        public void Probe(ProbeContext context)
        {
            throw new NotImplementedException();
        }

        public Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
        {
            if (!string.IsNullOrEmpty(tenant.TenantId))
            {
                context.Headers.Set("Tenant-From-Publish", tenant.TenantId);
            }
            return next.Send(context);
        }
    }
}
