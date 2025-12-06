using Application.ApplicationDto;
using Common.ResultInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateResourceCommandHandler
    {
        public Task<IResult<ResourceResponseDto>> HandleAsync(CreateResourceCommandDto dto);
    }
}
