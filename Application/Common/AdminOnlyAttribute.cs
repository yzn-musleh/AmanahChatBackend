using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Common
{
    

    public class AdminOnlyAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _workspaceIdClaim = "WorkspaceId";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var workspaceId = Guid.Parse(user.FindFirst(_workspaceIdClaim)?.Value ?? Guid.Empty.ToString());
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

            // check DB for IsAdmin
            var db = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();
            var isAdmin = await db.Users.AnyAsync(u => u.Id == userId && u.WorkspaceId == workspaceId && u.IsAdmin);

            if (!isAdmin)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }

}
