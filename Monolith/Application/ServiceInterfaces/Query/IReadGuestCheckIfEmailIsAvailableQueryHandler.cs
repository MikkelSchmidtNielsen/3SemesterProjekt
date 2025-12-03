using Application.ApplicationDto.Query;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
	public interface IReadGuestCheckIfEmailIsAvailableQueryHandler
	{
		public Task<IResult<ReadGuestCheckIfEmailIsAvailableResponseDto>> HandleAsync(ReadGuestCheckIfEmailIsAvailableQueryDto dto);
	}
}
