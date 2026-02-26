using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Message
{
    public interface IMessageService
    {
        Task SendPollUpdate(object data, int pollId);
    }
}
