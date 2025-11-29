using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    internal static class BookingTestHelper
    {
        public static Booking CreateBookingWithResourceAndGuest(
            int guestId, int resourceId, DateOnly startDate, DateOnly endDate, decimal totalPrice, Guest guest, Resource resource
        )
        {
            var createdBooking = new BookingCheckInAndOutQueryTestClass(guestId, resourceId, startDate, endDate, totalPrice, guest, resource);

            return createdBooking;
        }
    }
}
