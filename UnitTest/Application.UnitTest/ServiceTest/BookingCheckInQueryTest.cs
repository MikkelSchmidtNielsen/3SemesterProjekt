using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Application.Services.Query;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class BookingCheckInQueryTest
    {
        [Fact]

        public async Task GetActiveBookingsWithMissingCheckInsAsync_ShouldPass_WhenReturningListOfBookings()
        {
            // Arrange

            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();

            Booking booking1 = new Booking(1, 1, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(4)), 3000);
            Booking booking2 = new Booking(2, 2, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(6)), 9000);
            Guest guest1 = new Guest("Jens", "Jensen", null, null, null, null, null);
            Guest guest2 = new Guest("Birger", null, null, null, null, null, null);
            Resource resource1 = new Resource("Luksushytte", "Hytte", 400, 1, null);
            Resource resource2 = new Resource("Hyggeplads", "Plads", 200, 2, null);

            var bookingForTest1 = BookingCheckInAndOutQueryTestHelper.CreateBookingWithResourceAndGuest(booking1, guest1, resource1);
            var bookingForTest2 = BookingCheckInAndOutQueryTestHelper.CreateBookingWithResourceAndGuest(booking2 , guest2, resource2);

            IEnumerable<Booking> bookingList = new List<Booking>() { bookingForTest1, bookingForTest2 };

            bookingRepo.Setup(x => x.CreateBookingAsync(bookingForTest1)).ReturnsAsync(Result<Booking>.Success(bookingForTest1));
            bookingRepo.Setup(x => x.CreateBookingAsync(bookingForTest2)).ReturnsAsync(Result<Booking>.Success(bookingForTest2));
            bookingRepo.Setup(x => x.GetActiveBookingsWithMissingCheckInsAsync()).ReturnsAsync(Result<IEnumerable<Booking>>.Success(bookingList));

            IBookingCheckInQuery bookingCheckInQuery = new BookingCheckInQuery(bookingRepo.Object);

            // Act
            var result = await bookingCheckInQuery.GetActiveBookingsWithMissingCheckInsAsync();

            // Assert
            Assert.NotEmpty(result.GetSuccess().OriginalType);
        }
    }
}
