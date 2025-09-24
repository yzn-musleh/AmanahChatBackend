using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccessKeys.AddAccessKey
{
    public class AddAccessKeyRequest : IRequest<ApiResult<string>>
    {
        public Guid TenantId { get; set; }
    }
}
