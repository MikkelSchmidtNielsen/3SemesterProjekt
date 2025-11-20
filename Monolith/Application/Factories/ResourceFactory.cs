using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
	public class ResourceFactory : IResourceFactory
	{
		public IResult<Resource> Create()
		{
			throw new NotImplementedException();
		}
	}
}
