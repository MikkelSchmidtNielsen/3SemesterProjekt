using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto.Command
{
    public class UICreateResourceDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
    }
}
