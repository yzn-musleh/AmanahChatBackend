using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Queries.Workspaces.SearchWorkspaceByMetadata
{
    public class SearchWorkspaceByMetadataRequest : IRequest<ApiResult<Guid>>
    {
        public List<KeyValuePair<string, string>>? Metadata { get; set; }
    }

    public class SearchWorkspaceByMetadataRequestValidator : AbstractValidator<SearchWorkspaceByMetadataRequest>
    {
        public SearchWorkspaceByMetadataRequestValidator()
        {
        }
    }
}
