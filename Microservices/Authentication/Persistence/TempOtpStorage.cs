using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Common.ResultInterfaces;
using Application.RepositoryInterfaces;

namespace Persistence
{
    public class TempOtpStorage : ITempOtpStorage
    {
        public Dictionary<string, Otp> OtpDictionary = new Dictionary<string, Otp>();
    }
}
