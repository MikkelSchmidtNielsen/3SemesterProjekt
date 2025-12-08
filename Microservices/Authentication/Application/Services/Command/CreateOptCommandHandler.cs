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

namespace Application.Services.Command
{
    public class CreateOptCommandHandler : ICreateOptCommandHandler
    {
        private readonly IUserRepository _repository;
        public CreateOptCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> Handle(string email)
        {
            var userResult = await _repository.ReadUserByEmailAsync(email);

            if (userResult.GetSuccess().OriginalType != null)
            {
                Random random = new Random();
                int otp = random.Next(100000, 999999);
                DateTime expiryTime = DateTime.UtcNow.AddMinutes(30);

                userResult.GetSuccess().OriginalType.SetOtp(otp, expiryTime);

            }

        }
    }
}
