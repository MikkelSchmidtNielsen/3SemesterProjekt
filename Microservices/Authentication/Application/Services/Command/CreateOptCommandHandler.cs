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
using Persistence;

namespace Application.Services.Command
{
    public class CreateOptCommandHandler : ICreateOptCommandHandler
    {
        private TempOtpStorage _tempOtpStorage;
        public CreateOptCommandHandler(TempOtpStorage tempOtpStorage)
        {
            _tempOtpStorage = tempOtpStorage;
        }
        public Task Handle(string email)
        {
            Random random = new Random();
            int value = random.Next(100000, 999999);
            DateTime expiryTime = DateTime.UtcNow.AddMinutes(30);
            Otp otp = new Otp(value, expiryTime);

            _tempOtpStorage.OtpDictionary.Add(email, otp);


        }
    }
}
