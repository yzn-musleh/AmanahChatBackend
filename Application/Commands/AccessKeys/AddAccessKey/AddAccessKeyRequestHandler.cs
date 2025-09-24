using Application.Common.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Commands.AccessKeys.AddAccessKey
{
    public class AddAccessKeyRequestHandler(IApikeyService _service , IApplicationDbContext _applicationDbContext) : IRequestHandler<AddAccessKeyRequest, ApiResult<string>>
    {

        public async Task<ApiResult<string>> Handle(AddAccessKeyRequest request, CancellationToken cancellationToken)
        {
            var result = new ApiResult<string>();

            try
            {

                var apiKey = _service.GenerateApiKey(request.TenantId);

                return result.CreateSuccess(apiKey);

            } catch (Exception ex)
            {
                return result.CreateError(Domain.Enums.ErrorCodeEnum.DatabaseError, ex.Message);
            }
        }
    }
}
