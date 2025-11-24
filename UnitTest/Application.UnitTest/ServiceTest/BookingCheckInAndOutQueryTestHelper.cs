using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    internal static class BookingCheckInAndOutQueryTestHelper
    {
        public static Booking CreateBookingWithResourceAndGuest(
            Booking booking,
            Guest guest,
            Resource resource
        )
        {
            var createdBooking = new BookingCheckInAndOutQueryTestClass(booking, guest, resource);

            return createdBooking;
        }
    }
}
