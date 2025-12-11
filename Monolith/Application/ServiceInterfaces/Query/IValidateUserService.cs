using Application.ApplicationDto.Query;
using Application.InfrastructureDto;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces.Query
{
    public interface IValidateUserService
    {
        Task<IResult<ValidateUserByApiResponseDto>> ValidateOtpAsync(string email, int otp);
    }
}
