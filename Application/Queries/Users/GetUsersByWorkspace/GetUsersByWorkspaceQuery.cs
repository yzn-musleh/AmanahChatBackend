
using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Users.GetUsersByWorkspace
{
    public class GetUsersByWorkspaceQuery : IRequest<ApiResult<List<GetUserDto>>>, IUserContext
    {
        public Guid WorkspaceId { get; set; }
        public Guid UserId { get; set; }
    }
}
