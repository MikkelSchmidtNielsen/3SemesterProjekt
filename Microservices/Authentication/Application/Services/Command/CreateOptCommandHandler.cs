using Application.ServiceInterfaces.Command;
using Domain.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Application.RepositoryInterfaces;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
using Application.InfrastructureInterfaces;
using Common;

namespace Application.Services.Command
{
    public class CreateOptCommandHandler : ICreateOptCommandHandler
    {
        private readonly IUserRepository _repository;
        private readonly ISendEmail _sendEmail;
        public CreateOptCommandHandler(IUserRepository repository, ISendEmail sendEmail)
        {
            _repository = repository;
            _sendEmail = sendEmail;
        }
        public async Task<Result<string>> Handle(string email)
        {
            var userResult = await _repository.ReadUserByEmailAsync(email);

            if (userResult.GetSuccess().OriginalType != null)
            {
                Random random = new Random();
                int otp = random.Next(100000, 999999);
                DateTime expiryTime = DateTime.UtcNow.AddMinutes(30);

                userResult.GetSuccess().OriginalType.SetOtp(otp, expiryTime);
            }

            var updateResult = await _repository.UpdateUserAsync(userResult.GetSuccess().OriginalType);

            if (updateResult.IsSuccess())
            {
                SendOtpEmail sendOtpEmail = new SendOtpEmail(updateResult.GetSuccess().OriginalType);

                var emailResult = _sendEmail.SendEmail(sendOtpEmail);

                if (!emailResult.IsSuccess())
                {
                    return Result<string>.Error(email, emailResult.GetError().Exception!);
                }
                return Result<string>.Success(email);
            }
            else
            {
                return Result<string>.Error(email, updateResult.GetError().Exception!);
            }

        }
    }
}
