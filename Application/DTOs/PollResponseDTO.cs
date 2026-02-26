using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PollResponseDTO
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public int TotalVotes { get; set; } 
        public int? VotedOptionId { get; set; } 
        public bool IsActive { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OptionResponseDTO> Options { get; set; } = new();
    }
}
