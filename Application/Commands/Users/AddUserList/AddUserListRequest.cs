using Application.Commands.Users.AddUser;
using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Commands.Users.AddUserList
{
    public class AddEditUserListRequest : IRequest<ApiResult<bool>>
    {
        public List<AddUserRequest> Users { get; set; } = new();
    }

    public class AddUserListRequestValidator : AbstractValidator<AddEditUserListRequest>
    {
        public AddUserListRequestValidator()
        {
        }
    }
}
