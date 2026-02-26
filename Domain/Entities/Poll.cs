using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Poll
    {
        public int Id { get; set; }
        public string Question { get; set; }= string.Empty;
        public string CreatedById { get; set; } = string.Empty; 
        public ApplicationUser User { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public List<Option> Options { get; set; } = new();
    
           
    }
}
