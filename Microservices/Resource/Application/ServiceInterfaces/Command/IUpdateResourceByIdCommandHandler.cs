using Application.ApplicationDto;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
	public interface IUpdateResourceByIdCommandHandler
	{
		public Task<IResult<ResourceResponseDto>> HandleAsync(int id, UpdateResourceByIdCommandDto dto);
	}
}
