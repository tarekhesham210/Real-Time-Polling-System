using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace API.Hubs
{
    public class PollHub : Hub
    {
        public async Task JoinPollRoom(int pollId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, pollId.ToString());
        }

        public async Task LeavePollRoom(int pollId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, pollId.ToString());
        }
    }
}
