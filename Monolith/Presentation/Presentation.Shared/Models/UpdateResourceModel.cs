using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Shared.Models
{
	public class UpdateResourceModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;
		public decimal BasePrice { get; set; }
		public int Location { get; set; }
		public string Description { get; set; } = string.Empty;
		public bool IsAvailable { get; set; } = true;
		public byte[] RowVersion { get; set; }
	}
}
