using Common.ResultInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateTokenCommandHandler
    {
        public Task<IResult<string>> Handle(string email);
    }
}
