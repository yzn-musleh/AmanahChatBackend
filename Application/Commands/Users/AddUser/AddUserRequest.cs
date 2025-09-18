using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application.Commands.Users.AddUser
{
    public class AddUserRequest : IRequest<ApiResult<string>>
    {
        public string? FirstName { get; set; } ="";

        public string? LastName { get; set; } = "";

        public string? Username { get; set; } = null!;

        public string? Email { get; set; }

        public string? ProfileImageUrl { get; set; }

        public Guid? WorkspaceId { get; set; }
        public bool IsAdmin { get; set; } = false;

        public string Password { get; set; }

        public List<KeyValuePair<string, string>>? Metadata { get; set; }
    }

    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotNull().NotEmpty();

            RuleFor(x => x.LastName).NotNull().NotEmpty();

            RuleFor(x => x.Username).NotNull().NotEmpty();
        }
    }
}
