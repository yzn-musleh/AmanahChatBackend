using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Workspaces.GetWorkspaces
{
    public class GetWorkspaceQuery : IRequest<ApiResult<List<GetWorkspaceDto>>>
    {

    }
}
