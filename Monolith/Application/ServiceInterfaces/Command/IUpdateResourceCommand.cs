using Application.ApplicationDto.Command;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
	public interface IUpdateResourceCommand
	{
		public Task<IResult<UpdateResourceResponseDto>> HandleAsync(UpdateResourceCommandDto command);
	}
}
