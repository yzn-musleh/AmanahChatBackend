using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Application.Common
{

    public class UserContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                request.UserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? user.FindFirst("sub")?.Value);
                request.WorkspaceId = Guid.Parse(user.FindFirst("WorkspaceId")?.Value ?? null);
            }
            else
            {
              //  throw new UnauthorizedAccessException("User not authenticated");
            }

            return await next();
        }
    }

}
