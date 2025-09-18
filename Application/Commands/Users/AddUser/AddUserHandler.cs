using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserHandler(
       IApplicationDbContext _applicationDbContext,
       IMapper _mapper,
       IJwtWebTokenGenerator _tokenGenerator)
       : IRequestHandler<AddUserRequest, ApiResult<string>>
    {
        public async Task<ApiResult<string>> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<string>();

            var userExists = _applicationDbContext.Users
                .Any(u => u.Username == request.Username || u.Email == request.Email);

            if (userExists)
            {
                return result.CreateError(ErrorCodeEnum.DuplicateEntry, "Username is taken");
            }

            var user = _mapper.Map<User>(request);

            user.Id = Guid.NewGuid();

            _applicationDbContext.Users.Add(user);

            await _applicationDbContext.SaveChangesAsync(new CancellationToken());

            var token = _tokenGenerator.GenerateToken(user.Id, user.WorkspaceId, user.Username);
            return result.CreateSuccess(token);
        }
    }
}
