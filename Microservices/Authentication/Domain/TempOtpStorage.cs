using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Common.ResultInterfaces;

namespace Persistence
{
    public class TempOtpStorage
    {
        public Dictionary<string, Otp> OtpDictionary = new Dictionary<string, Otp>();
    }
}
