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
        public BookingCheckInAndOutQueryTestClass(Booking booking, Guest guest, Resource resource) : base(booking, guest, resource)
        {
        }
    }
}
