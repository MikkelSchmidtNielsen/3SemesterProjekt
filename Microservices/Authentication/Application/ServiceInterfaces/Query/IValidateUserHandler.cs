using Application.ApplicationDto;
using Common.ResultInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
    public interface IValidateUserHandler
    {
        public Task<IResult<ValidateUserResponseDto>> Handle(ValidateUserQueryDto dto);
    }
}
