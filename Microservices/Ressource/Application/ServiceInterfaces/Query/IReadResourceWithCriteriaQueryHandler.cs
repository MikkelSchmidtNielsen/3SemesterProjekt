using Application.ApplicationDto;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
	public interface IReadResourceWithCriteriaQueryHandler
	{
		public Task<IResult<IEnumerable<ResourceResponseDto>>> HandleAsync(ReadResourceListQueryDto criteria);
	}
}
