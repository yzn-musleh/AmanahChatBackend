using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.LoginUser
{
    public class LoginUserRequest: IRequest<ApiResult<GetLoggedUserDto>>
    {
        public string UserName { get; set;}
        public string Password { get; set;}
    }
}
