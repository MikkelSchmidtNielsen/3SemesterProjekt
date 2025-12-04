using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Otp
    {
        public int Id { get; init; }
        public int Value { get; init; }
        public DateTime ExpiryTime { get; init; }

        public Otp(int value, DateTime expiryTime)
        {
            Value = value;
            ExpiryTime = expiryTime;
        }
    }
}
