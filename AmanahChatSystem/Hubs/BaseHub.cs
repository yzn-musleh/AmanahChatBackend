using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.UI
{
    public abstract class BaseHub : Hub
    {
        private readonly IMediator Mediator;

        public BaseHub(IServiceProvider serviceProvider)
        {
            Mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        public async Task AddToGroup(Guid groupId, KeyValuePair<string, string>[] keys)
        {
            //long.TryParse(Context.UserIdentifier ?? "", out long userId);


            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
        }

        public async Task RemoveFromGroup(Guid groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnConnectedAsync();
        }
    }
}
