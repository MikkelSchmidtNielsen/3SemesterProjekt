using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelsDto
{
    public class CreateResourceFactoryDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal BasePrice { get; set; }
        public int Location {  get; set; }
        public string? Description { get; set; }
    }
}
