using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
    public interface IOtpCreateCommand
    {
        public Task<IResult<string>> CreateOtpAsync(string email);
    }
}
