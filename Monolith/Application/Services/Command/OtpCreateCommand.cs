using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Common.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
    public class OtpCreateCommand : IOtpCreateCommand
    {
        private readonly IUserAuthenticationApiService _apiService;

        public OtpCreateCommand(IUserAuthenticationApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task CreateOtpAsync(string email)
        {
            await _apiService.RequestOtpAsync(email);
        }
    }
}
