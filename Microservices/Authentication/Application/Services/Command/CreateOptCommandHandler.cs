using Application.ServiceInterfaces.Command;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Application.Services.Command
{
    public class CreateOptCommandHandler : ICreateOptCommandHandler
    {
        public Task Handle(string email)
        {
            Random random = new Random();
            int value = random.Next(100000, 999999);
            DateTime expiryTime = DateTime.UtcNow.AddMinutes(30);
            Otp otp = new Otp(value, expiryTime);

            // Call to email method here

            var Result = 
        }
    }
}
