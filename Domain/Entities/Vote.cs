using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int OptionId { get; set; }
        public int PollId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Option Option { get; set; } = null!;
    }
}
