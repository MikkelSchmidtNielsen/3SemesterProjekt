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
            Mock<IResourceRepository> resourceRepo = new Mock<IResourceRepository>();
            Mock<IGuestRepository> guestRepo = new Mock<IGuestRepository>();

            Booking booking1 = new Booking(1, 1, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(4)), 3000);
            Booking booking2 = new Booking(2, 2, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(6)), 9000);
            Guest guest1 = new Guest("Jens", "Jensen", null, null, null, null, null);
            Guest guest2 = new Guest("Birger", null, null, null, null, null, null);
            Resource resource1 = new Resource("Luksushytte", "Hytte", 400, 1, null);
            Resource resource2 = new Resource("Hyggeplads", "Plads", 200, 2, null);
            

            IEnumerable<Booking> bookingList = new List<Booking>() { booking1, booking2 };
            guestRepo.Setup(x => x.CreateGuestAsync(guest1)).ReturnsAsync(Result<Guest>.Success(guest1));
            guestRepo.Setup(x => x.CreateGuestAsync(guest2)).ReturnsAsync(Result<Guest>.Success(guest2));

            resourceRepo.Setup(x => x.AddResourceToDBAsync(resource1)).ReturnsAsync(Result<Resource>.Success(resource1));
            resourceRepo.Setup(x => x.AddResourceToDBAsync(resource2)).ReturnsAsync(Result<Resource>.Success(resource2));

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
