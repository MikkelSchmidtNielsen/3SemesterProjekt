using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
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
            

            IEnumerable<Booking> bookingList = new List<Booking>() { booking1, booking2 };

            bookingRepo.Setup(x => x.CreateBookingAsync(booking1)).ReturnsAsync(Result<Booking>.Success(booking1));
            bookingRepo.Setup(x => x.CreateBookingAsync(booking2)).ReturnsAsync(Result<Booking>.Success(booking2));
            bookingRepo.Setup(x => x.GetActiveBookingsWithMissingCheckInsAsync()).ReturnsAsync(Result<IEnumerable<Booking>>.Success(bookingList));

            IBookingCheckInQuery bookingCheckInQuery = new BookingCheckInQuery(bookingRepo.Object);

            // Act
            var result = await bookingCheckInQuery.GetActiveBookingsWithMissingCheckInsAsync();

            // Assert
            Assert.NotEmpty(result.GetSuccess().OriginalType);
        }
    }
}
