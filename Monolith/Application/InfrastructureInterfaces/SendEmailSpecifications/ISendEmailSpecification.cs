using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureInterfaces.SendEmailSpecifications
{
	public interface ISendEmailSpecification
	{
		public string RecieverEmail { get; }
		public string Subject { get; }
		public string Body { get; }	
	}
}
