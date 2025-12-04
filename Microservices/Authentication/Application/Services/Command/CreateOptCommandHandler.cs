using Application.ServiceInterfaces.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
    public class CreateOptCommandHandler : ICreateOptCommandHandler
    {
        public Task Handle(string email)
        {
            throw new NotImplementedException();
        }
    }
}
