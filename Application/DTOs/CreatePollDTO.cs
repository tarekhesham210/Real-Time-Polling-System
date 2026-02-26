using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreatePollDTO
    {
        public string Question { get; set; }=string.Empty;

        public List<string>  Options { get; set; }=new List<string>();
    }
}
