using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto
{
	public class ApiResponseDto
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }
		public ICollection<ResourceResponseDto>? Responses { get; set; }
	}
}
