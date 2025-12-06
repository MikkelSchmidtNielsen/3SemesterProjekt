using Application.ApplicationDto.Command;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    internal class BookingCheckInAndOutQueryTestClass : Booking
    {
        public BookingCheckInAndOutQueryTestClass(int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice, Guest guest) : base(guestId, resourceId, startDate, endDate, totalPrice, guest)
        {
        }
    }
}
