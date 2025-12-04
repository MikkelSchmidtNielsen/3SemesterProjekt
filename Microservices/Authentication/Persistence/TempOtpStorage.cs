using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Common.ResultInterfaces;

namespace Persistence
{
    public class TempOtpStorage
    {
        public static Dictionary<string, Otp> OtpDictionary = new Dictionary<string, Otp>();

        public void AddOtpToDictionary(string email, Otp otp)
        {
            OtpDictionary.Add(email, otp);
        }

        //public IResult<Otp> ValidateOtp(string email, Otp otp)
        //{

        //}

        public void RemoveOtpFromDictionary(string email, Otp otp)
        {
            OtpDictionary.Remove(email);
        }
    }
}
