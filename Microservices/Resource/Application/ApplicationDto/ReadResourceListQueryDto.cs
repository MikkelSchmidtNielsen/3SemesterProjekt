using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto
{
	public class ReadResourceListQueryDto
	{
		public string? Name { get; set; }
		public IEnumerable<string>? Type { get; set; } 
		public int? Location { get; set; }
		public bool? IsAvailable { get; set; }
		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
	}
}
