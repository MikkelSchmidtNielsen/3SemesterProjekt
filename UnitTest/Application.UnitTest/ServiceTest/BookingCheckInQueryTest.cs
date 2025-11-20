using Application.Factories;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
using Common;
using Domain.DomainInterfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class BookingCheckInQueryTest
    {
        [Fact]

        public async Task GetActiveBookingsWithMissingCheckInsAsync_ShouldPass_WhenReturningListOfBookings()
        {
            // Arrange
            Booking booking1 = new Booking(1, 1, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(4)), 3000);
            Booking booking2 = new Booking(2, 2, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(6)), 9000);

            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            repository.Setup(x => x.CreateBookingAsync(booking1)).ReturnsAsync(Result<Booking>.Success(booking1));
            repository.Setup(x => x.CreateBookingAsync(booking2)).ReturnsAsync(Result<Booking>.Success(booking2));

            IBookingCheckInQuery bookingCheckInQuery = new BookingCheckInQuery(repository.Object);

            // Act
            var result = await bookingCheckInQuery.GetActiveBookingsWithMissingCheckInsAsync();

            // Assert
            Assert.NotEmpty(result.GetSuccess().OriginalType);
        }
    }
}
