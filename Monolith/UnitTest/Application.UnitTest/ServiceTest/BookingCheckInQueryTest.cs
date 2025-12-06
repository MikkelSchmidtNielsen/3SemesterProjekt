using Application.ApplicationDto.Query;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Query;
using Application.Services.Query;
using Common;
using Domain.Models;
using Moq;
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
                .WithDefaults()
                .Create();

            ReadResourceQueryResponseDto resource2 = Impression.Of<ReadResourceQueryResponseDto>()
                .With("Id", 2)
                .WithDefaults()
                .Create();

            var bookingForTest1 = BookingTestHelper.CreateBookingWithResourceAndGuest(1, 1, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(4)), 3000, guest1);
            var bookingForTest2 = BookingTestHelper.CreateBookingWithResourceAndGuest(2, 2, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(6)), 9000, guest2);

            IEnumerable<Booking> bookingList = new List<Booking>() { bookingForTest1, bookingForTest2 };
            IEnumerable<ReadResourceQueryResponseDto> resourceList = new List<ReadResourceQueryResponseDto>() { resource1, resource2 };

            bookingRepo
                .Setup(x => x.CreateBookingAsync(bookingForTest1))
                .ReturnsAsync(Result<Booking>.Success(bookingForTest1));

            bookingRepo
                .Setup(x => x.CreateBookingAsync(bookingForTest2))
                .ReturnsAsync(Result<Booking>.Success(bookingForTest2));

            bookingRepo
                .Setup(x => x.GetActiveBookingsWithMissingCheckInsAsync())
                .ReturnsAsync(Result<IEnumerable<Booking>>.Success(bookingList));

            resourcesQuery
                .Setup(x => x.ReadAllResourcesAsync(It.IsAny<ResourceFilterDto>()))
                .ReturnsAsync(Result<IEnumerable<ReadResourceQueryResponseDto>>.Success(resourceList));

            IBookingCheckInQuery bookingCheckInQuery = new BookingCheckInQuery(bookingRepo.Object, resourcesQuery.Object);

            // Act
            var result = await bookingCheckInQuery.GetActiveBookingsWithMissingCheckInsAsync();

            // Assert
            Assert.NotEmpty(result.GetSuccess().OriginalType);
        }
    }
}
