using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PollResultsUpdateDTO
    {
        public int PollId { get; set; }
        public int TotalVotes { get; set; }
        public List<OptionResponseDTO> Options { get; set; } = new();
    }
}
