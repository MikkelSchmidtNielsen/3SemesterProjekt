using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ResultInterfaces;
using Domain.Models;

namespace Domain.DomainInterfaces
{
	public interface IResourceFactory
	{
		IResult<Resource> CreateResource();
	}
}
