using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;

namespace Domain.DomainInterfaces
{
	public interface IResourceFactory
	{
		public Task<IResult<Resource>> CreateResourceAsync(CreateResourceFactoryDto dto);
	}
}
