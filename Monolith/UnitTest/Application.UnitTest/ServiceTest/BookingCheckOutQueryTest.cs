using Application.ApplicationDto.Query;
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
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class BookingCheckOutQueryTest
    {
        [Fact]
        public async Task GetFinishedBookingsWithMissingCheckOutsAsync_ShouldPass_WhenGivenListOfMissedCheckouts()
        {
            // Arrange
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();
            Mock<IReadAllResourcesQuery> resourcesQuery = new Mock<IReadAllResourcesQuery>();

            Guest guest1 = Impression.Of<Guest>()
                .With("Id", 1)
                .WithDefaults()
                .Create();

            Guest guest2 = Impression.Of<Guest>()
                .With("Id", 2)
                .WithDefaults()
                .Create();

            ReadResourceQueryResponseDto resource1 = Impression.Of<ReadResourceQueryResponseDto>()
                .With("Id", 1)
                .With("IsAvailable", false)
                .WithDefaults()
                .Create();

            ReadResourceQueryResponseDto resource2 = Impression.Of<ReadResourceQueryResponseDto>()
                .With("Id", 2)
                .With("IsAvailable", false)
                .WithDefaults()
                .Create();

            var unitTestBooking1 = BookingTestHelper.CreateBookingWithResourceAndGuest(1, 1, DateOnly.FromDateTime(DateTime.Now.AddDays(-4)), DateOnly.FromDateTime(DateTime.Now), 2000, guest1);
            var unitTestBooking2 = BookingTestHelper.CreateBookingWithResourceAndGuest(2, 2, DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), DateOnly.FromDateTime(DateTime.Now), 6000, guest2);

            IEnumerable<Booking> missedCheckOuts = new List<Booking>() { unitTestBooking1,  unitTestBooking2 };
            IEnumerable<ReadResourceQueryResponseDto> resourceList = new List<ReadResourceQueryResponseDto>() { resource1, resource2 };

            bookingRepo
                .Setup(x => x.CreateBookingAsync(unitTestBooking1))
                .ReturnsAsync(Result<Booking>.Success(unitTestBooking1));

            bookingRepo
                .Setup(x => x.CreateBookingAsync(unitTestBooking2))
                .ReturnsAsync(Result<Booking>.Success(unitTestBooking2));

            bookingRepo
                .Setup(x => x.GetFinishedBookingsWithMissingCheckOutsAsync())
                .ReturnsAsync(Result<IEnumerable<Booking>>.Success(missedCheckOuts));

            resourcesQuery
                .Setup(x => x.ReadAllResourcesAsync(It.IsAny<ResourceFilterDto>()))
                .ReturnsAsync(Result<IEnumerable<ReadResourceQueryResponseDto>>.Success(resourceList));

            IBookingCheckOutQuery bookingCheckOutQuery = new BookingCheckOutQuery(bookingRepo.Object, resourcesQuery.Object);

            // Act
            var result = await bookingCheckOutQuery.GetFinishedBookingsWithMissingCheckOutsAsync();

            // Assert
            Assert.NotEmpty(result.GetSuccess().OriginalType);
        }
    }
}
