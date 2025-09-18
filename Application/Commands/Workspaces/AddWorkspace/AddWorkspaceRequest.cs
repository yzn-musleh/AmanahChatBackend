using Application.Common.Interfaces;
using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Commands.Workspaces.AddWorkspace
{
    public class AddWorkspaceRequest : IRequest<ApiResult<Guid>>, IUserContext
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public List<KeyValuePair<string, string>>? Metadata { get; set; }
        Guid IUserContext.UserId { get; set; }
        Guid IUserContext.WorkspaceId { get; set; }
    }

    public class AddWorkspaceRequestValidator : AbstractValidator<AddWorkspaceRequest>
    {
        public AddWorkspaceRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();

            RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
