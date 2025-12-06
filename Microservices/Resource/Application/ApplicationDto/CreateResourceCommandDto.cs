using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto
{
	public class CreateResourceCommandDto
	{
		public string Name { get; set; } = null!;
		public string Type { get; set; } = null!;
		public decimal BasePrice { get; set; }
		public int Location { get; set; }
		public string? Description { get; set; }
	}
}
