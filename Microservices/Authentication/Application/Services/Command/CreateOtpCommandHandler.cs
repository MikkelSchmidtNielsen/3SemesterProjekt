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
    public class CreateOtpCommandHandler : ICreateOtpCommandHandler
    {
        private readonly IUserRepository _repository;
        private readonly ISendEmail _sendEmail;
        public CreateOtpCommandHandler(IUserRepository repository, ISendEmail sendEmail)
        {
            _repository = repository;
            _sendEmail = sendEmail;
        }
        public async Task Handle(string email)
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

            SendOtpEmail sendOtpEmail = new SendOtpEmail(updateResult.GetSuccess().OriginalType);

            var emailResult = _sendEmail.SendEmail(sendOtpEmail);

        }
    }
}
