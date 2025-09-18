namespace Application.Common.Interfaces
{
    public interface IUserContext
    {
        public Guid UserId { get; internal set; }
        public Guid WorkspaceId { get; internal set; }
    }
}
