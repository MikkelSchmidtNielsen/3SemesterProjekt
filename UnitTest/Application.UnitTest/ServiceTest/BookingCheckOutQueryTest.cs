using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
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

            Guest guest1 = new Guest("Hans", "Hansen", null, null, null, null, null);
            Guest guest2 = new Guest("Ole", "Olsen", null, null, null, null, null);
            Resource resource1 = new Resource("Familiehytten", "Hytte", 400, 1, null);
            Resource resource2 = new Resource("Teltplads", "Plads", 200, 2, null);

            var unitTestBooking1 = BookingCheckInAndOutQueryTestHelper.CreateBookingWithResourceAndGuest(3, 6, DateOnly.FromDateTime(DateTime.Now.AddDays(-4)), DateOnly.FromDateTime(DateTime.Now), 2000, guest1, resource1);
            var unitTestBooking2 = BookingCheckInAndOutQueryTestHelper.CreateBookingWithResourceAndGuest(6, 12, DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), DateOnly.FromDateTime(DateTime.Now), 6000, guest2, resource2);

            IEnumerable<Booking> missedCheckOuts = new List<Booking>() { unitTestBooking1,  unitTestBooking2 };
            bookingRepo.Setup(x => x.CreateBookingAsync(unitTestBooking1)).ReturnsAsync(Result<Booking>.Success(unitTestBooking1));
            bookingRepo.Setup(x => x.CreateBookingAsync(unitTestBooking2)).ReturnsAsync(Result<Booking>.Success(unitTestBooking2));
            bookingRepo.Setup(x => x.GetFinishedBookingsWithMissingCheckOutsAsync()).ReturnsAsync(Result<IEnumerable<Booking>>.Success(missedCheckOuts));

            IBookingCheckOutQuery bookingCheckOutQuery = new BookingCheckOutQuery(bookingRepo.Object);

            // Act

            var result = await bookingCheckOutQuery.GetFinishedBookingsWithMissingCheckOutsAsync();

            // Assert

            Assert.NotEmpty(result.GetSuccess().OriginalType);
        }
    }
}
