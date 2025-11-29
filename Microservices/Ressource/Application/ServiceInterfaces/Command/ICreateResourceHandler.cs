using Common.ResultInterfaces;
using Domain.Models;
using Application.ApplicationDto.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateResourceHandler
    {
        public Task<IResult<Resource>> Handle(UICreateResourceDto dto);
    }
}
