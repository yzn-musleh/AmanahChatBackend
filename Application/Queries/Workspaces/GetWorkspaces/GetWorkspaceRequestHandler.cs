using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Workspaces.GetWorkspaces
{
    public class GetWorkspaceRequestHandler(IApplicationDbContext _applicationDbContext,
        IMapper _mapper) : IRequestHandler<GetWorkspaceQuery, ApiResult<List<GetWorkspaceDto>>>
    {
        public async Task<ApiResult<List<GetWorkspaceDto>>> Handle(GetWorkspaceQuery request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<List<GetWorkspaceDto>>();

            try
            {
                var query = await _applicationDbContext.Workspaces.ToListAsync();

                var workspaces = _mapper.Map<List<GetWorkspaceDto>>(query);

                return result.CreateSuccess(workspaces);
            }
            catch (Exception ex)
            {
                return result.CreateError(ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
