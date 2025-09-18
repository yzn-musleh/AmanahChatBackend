using Domain.Common;
using Domain.Common.DTO;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatSystem.UI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _Mediator;

        public BaseController(IServiceProvider serviceProvider)
        {
            _Mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        
        //public LoggedInTenantDto? LoggedInTenantInfo
        //{
        //    get
        //    {
        //        JwtSecurityToken? jwtSecurityToken = null;
        //            string? authorization = HttpContext?.Request?
        //            .Headers["Authorization"].FirstOrDefault()?
        //            .Replace("Bearer ", "");
        //        if (!string.IsNullOrWhiteSpace(authorization))
        //            {
        //            var jwtHandler = new JwtSecurityTokenHandler();
        //            jwtSecurityToken = jwtHandler.ReadJwtToken(authorization);
        //                   var userClaims = jwtSecurityToken.Claims;

        //            return new LoggedInTenantDto()
        //            {
        //                TenantId = userClaims?.FirstOrDefault(x => x.Type == "Tenant_Id")?.Value;
        //                UserId = userClaims?.FirstOrDefault(x => x.);
        //            };
        //        }
        //        }
        //        }
        //        return null;

        protected IActionResult CreateResponseResult<T>(ApiResult<T> apiResult)
        {
            return apiResult?.ErrorCode == ErrorCodeEnum.None ? Ok(apiResult) : BadRequest(apiResult);
        }
    }
}
