using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Domain.UnitTest
{
    internal class BookingEntityTestClass : Booking
    {
        public BookingEntityTestClass() : base() { }
        public new int GetNumberOfDecimals(decimal TotalPrice)
        {
            return Booking.GetNumberOfDecimals(TotalPrice);
        }
    }
}
