using Application.ApplicationDto;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Query
{
    public class ValidateUserHandler : IValidateUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreateTokenCommandHandler _tokenHandler;

        public ValidateUserHandler(IUserRepository userRepository, ICreateTokenCommandHandler tokenHandler)
        {
            _userRepository = userRepository;
            _tokenHandler = tokenHandler;
        }

        public async Task<IResult<ValidateUserResponseDto>> Handle(ValidateUserDto dto)
        {
            var userResult = await _userRepository.ReadUserByEmailAsync(dto.Email);
            ValidateUserResponseDto response = Mapper.Map<ValidateUserResponseDto>(dto);

            if (userResult.GetSuccess().OriginalType.Otp == dto.Otp && userResult.GetSuccess().OriginalType.OtpExpiryTime > DateTime.UtcNow)
            {
                IResult<string> tokenResponse = _tokenHandler.Handle(userResult.GetSuccess().OriginalType);

                if (!tokenResponse.IsSuccess())
                {
                    Exception exception = tokenResponse.GetError().Exception!;

                    return Result<ValidateUserResponseDto>.Error(response, exception);
                }
                else
                {
                    response.Token = tokenResponse.GetSuccess().OriginalType;

                    return Result<ValidateUserResponseDto>.Success(response);
                }
            }
            else
            {
                Exception exception = new Exception("Invalid OTP");

                return Result<ValidateUserResponseDto>.Error(response, exception);
            }
        }
    }
}