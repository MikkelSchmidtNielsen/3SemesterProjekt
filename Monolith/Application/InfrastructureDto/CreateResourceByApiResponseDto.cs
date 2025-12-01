using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureDto
{
    public class CreateResourceByApiResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Location { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
