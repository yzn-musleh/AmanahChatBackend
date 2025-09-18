using Application.Common.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Users.AddUserList
{
    public class AddUserListHandler(
       IApplicationDbContext _applicationDbContext,
       IMapper _mapper)
       : IRequestHandler<AddEditUserListRequest, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(AddEditUserListRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<bool>();

            var users = _mapper.Map<List<User>>(request.Users);
            _applicationDbContext.Users.AddRange(users);

            await _applicationDbContext.SaveChangesAsync(new CancellationToken());

            return await Task.FromResult(result.CreateSuccess());
        }
    }
}
