using Application.RepositoryInterfaces;
using Common;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class BookingCheckOutQueryTest
    {
        [Fact]
        public async Task GetFinishedBookingsWithMissingCheckOutsAsync_ShouldPass_WhenGivenListOfMissedCheckouts()
        {
            // Arrange
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();
            Booking booking1 = new Booking(3, 6, DateOnly.FromDateTime(DateTime.Now.AddDays(-4)), DateOnly.FromDateTime(DateTime.Now), 2000);
            Booking booking2 = new Booking(6, 12, DateOnly.FromDateTime(DateTime.Now.AddDays(-8)), DateOnly.FromDateTime(DateTime.Now), 6000);
            Guest guest1 = new Guest("Hans", "Hansen", null, null, null, null, null);
            Guest guest2 = new Guest("Ole", "Olsen", null, null, null, null, null);
            Resource resource1 = new Resource("Familiehytten", "Hytte", 400, 1, null);
            Resource resource2 = new Resource("Teltplads", "Plads", 200, 2, null);

            var unitTestBooking1 = BookingCheckInAndOutQueryTestHelper.CreateBookingWithResourceAndGuest(booking1, guest1, resource1);
            var unitTestBooking2 = BookingCheckInAndOutQueryTestHelper.CreateBookingWithResourceAndGuest(booking2, guest2, resource2);

            IEnumerable<Booking> missedCheckOuts = new List<Booking>() { booking1,  booking2 };
            bookingRepo.Setup(x => x.CreateBookingAsync(unitTestBooking1)).ReturnsAsync(Result<Booking>.Success(unitTestBooking1));
            bookingRepo.Setup(x => x.CreateBookingAsync(unitTestBooking2)).ReturnsAsync(Result<Booking>.Success(unitTestBooking2));

            // Act

            // Assert
        }
    }
}
