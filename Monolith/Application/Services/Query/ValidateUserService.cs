using Application.ApplicationDto.Query;
using Application.InfrastructureDto;
using Application.ServiceInterfaces.Query;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
    public class ValidateUserService : IValidateUserService
    {
        private readonly IUserAuthenticationApiService _apiservice;

        public ValidateUserService(IUserAuthenticationApiService apiservice)
        {
            _apiservice = apiservice;
        }

        public async Task<IResult<ValidateUserByApiResponseDto>> ValidateOtpAsync(string email, int otp)
        {
            ValidateUserQueryDto dto = new ValidateUserQueryDto() { Email =  email, Otp = otp };
            return await _apiservice.ValidateUserAsync(dto);
        }
    }
}
