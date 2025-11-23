using Application.ApplicationDto;
using Common.ResultInterfaces;
using Domain;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateUserCommandHandler
    {
        public Task<IResult<CreateUserResponseDto>> Handle(string input);
    }
}
