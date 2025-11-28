using Application.ApplicationDto.Command;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InfrastructureInterfaces
{
	public interface ISendEmail
	{
		public IResult<ISendEmailSpecification> SendEmail(ISendEmailSpecification emailSpecification);
	}
}
