using API.Hubs;
using Application.Interfaces.Message;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Message
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<PollHub> _hubContext;
        public MessageService(IHubContext<PollHub> hubContext) => _hubContext = hubContext;

        public async Task SendPollUpdate(object data, int pollId)
        {

            await _hubContext.Clients.Group(pollId.ToString()).SendAsync("ReceivePollUpdate", data);
        }
    } 
}
