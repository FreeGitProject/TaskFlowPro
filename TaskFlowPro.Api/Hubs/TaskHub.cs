using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace TaskFlowPro.Api.Hubs
{
    [Authorize]
    public class TaskHub : Hub
    {
        public async Task JoinTaskGroup(int taskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"task-{taskId}");
        }

        public async Task LeaveTaskGroup(int taskId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"task-{taskId}");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
