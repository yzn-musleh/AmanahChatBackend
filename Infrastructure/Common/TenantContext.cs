using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class TenantContext
    {
        public Guid TenantId { get; private set; }

        public void SetTenant (Guid tenantId)
        {
            TenantId = tenantId;
            Console.WriteLine(tenantId);
        }
    }
}
