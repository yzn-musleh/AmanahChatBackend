using Domain.Common;
using MediatR;

namespace Application.Commands.Users.DeleteUser
{
    public class DeleteUserRequest : IRequest<ApiResult<Guid>>
    {
        public Guid UserId { get; set; }
    }
}
