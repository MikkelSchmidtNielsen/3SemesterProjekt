using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto
{
	public class ResourceResponseDto
	{
		public int? Id { get; set; }
		public string? Name { get; set; }
		public string? Type { get; set; }
		public decimal BasePrice { get; set; }
		public int Location { get; set; }
		public bool IsAvailable { get; set; }
		public string? Description { get; set; }
		public byte[] RowVersion { get; set; }
	}
}
