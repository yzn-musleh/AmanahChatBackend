using Application.Commands.Users.AddUser;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IJwtWebTokenGenerator
    {
        string GenerateToken(Guid userId, Guid WorkspaceId, string username);
    }
}
