using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Moq;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class BookingCreateCommandTest
    {
        [Fact]
        public async Task CreateBookingAsync_ReturnsSuccess_WhenAllStepsSucceed()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IResourceIdQuery> resourceIdQuery = new Mock<IResourceIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();

            BookingCreateDto bookingDto = new BookingCreateDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),

                Guest = new GuestCreateDto
                {
                    FirstName = "Dansker",
                    LastName = "1",
                    Email = "test@example.com"
                }
            };

            Resource resource = new Resource(1, "Hytten med parasol", "Hytte", 100);

            Guest guest = new Guest("Dansker 1", null, 0, null, null, null, null);

            Booking booking = new Booking(guest.Id, bookingDto.ResourceId, bookingDto.StartDate, bookingDto.EndDate, 300);

            // What we expect as final
            var expectedDto = new CreatedBookingDto
            {
                ResourceId = bookingDto.ResourceId,
                GuestId = guest.Id,
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                TotalPrice = 300
            };

            // Mock Resource query
            resourceIdQuery
                .Setup(query => query.GetResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<Resource>.Success(resource));

            // Mock Guest creation
            guestCreateCommand
                .Setup(command => command.CreateGuestAsync(bookingDto.Guest))
                .ReturnsAsync(Result<Guest>.Success(guest));

            // Mock Booking factory
            bookingFactory
                .Setup(factory => factory.Create(It.IsAny<CreatedBookingDto>()))
                .Returns(Result<Booking>.Success(booking));

            // Mock Repository
            repository
                .Setup(repo => repo.CreateBookingAsync(booking))
                .ReturnsAsync(Result<Booking>.Success(booking));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestCreateCommand.Object
            );

            // Act
            IResult<CreatedBookingDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsSucces());

            CreatedBookingDto success = result.GetSuccess().OriginalType;
            Assert.Equal(expectedDto.ResourceId, success.ResourceId);
            Assert.Equal(expectedDto.GuestId, success.GuestId);
            Assert.Equal(expectedDto.TotalPrice, success.TotalPrice);
            Assert.Equal(expectedDto.StartDate, success.StartDate);
            Assert.Equal(expectedDto.EndDate, success.EndDate);

            // Verify mocks were called
            resourceIdQuery.Verify(query => query.GetResourceByIdAsync(bookingDto.ResourceId), Times.Once);
            guestCreateCommand.Verify(command => command.CreateGuestAsync(bookingDto.Guest), Times.Once);
            bookingFactory.Verify(factory => factory.Create(It.IsAny<CreatedBookingDto>()), Times.Once);
            repository.Verify(repo => repo.CreateBookingAsync(booking), Times.Once);
        }

        [Fact]
        public async Task CreateBookingAsync_ReturnsError_WhenGetResourceFails()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IResourceIdQuery> resourceIdQuery = new Mock<IResourceIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();

            BookingCreateDto bookingDto = new BookingCreateDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            };

            Exception resourceException = new Exception("Resource not found");

            // Mock Resource query
            resourceIdQuery
                .Setup(query => query.GetResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<Resource>.Error(originalType: null, exception: resourceException));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestCreateCommand.Object
            );

            // Act
            IResult<CreatedBookingDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreatedBookingDto> error = result.GetError();
            Assert.Equal(resourceException, error.Exception);

            // Verify mock were called
            resourceIdQuery.Verify(query => query.GetResourceByIdAsync(bookingDto.ResourceId), Times.Once);

            // Verify mocks weren't called
            guestCreateCommand.Verify(command => command.CreateGuestAsync(It.IsAny<GuestCreateDto>()), Times.Never());
            bookingFactory.Verify(factory => factory.Create(It.IsAny<CreatedBookingDto>()), Times.Never);
            repository.Verify(x => x.CreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CreateBookingAsync_ReturnsError_WhenGuestCreationFails()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IResourceIdQuery> resourceIdQuery = new Mock<IResourceIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();

            BookingCreateDto bookingDto = new BookingCreateDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                Guest = new GuestCreateDto()
            };

            Resource resource = new Resource(1, "Hytten med parasol", "Hytte", 100);

            Exception guestException = new Exception("Guest creation failed");

            // Mock Resource query
            resourceIdQuery
                .Setup(x => x.GetResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<Resource>.Success(resource));

            // Mock Guest creation
            guestCreateCommand
                .Setup(x => x.CreateGuestAsync(bookingDto.Guest))
                .ReturnsAsync(Result<Guest>.Error(originalType: null, exception: guestException));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestCreateCommand.Object
            );

            // Act
            IResult<CreatedBookingDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreatedBookingDto> error = result.GetError();
            Assert.Equal(guestException, error.Exception);

            // Verify mock were called
            resourceIdQuery.Verify(query => query.GetResourceByIdAsync(bookingDto.ResourceId), Times.Once);
            guestCreateCommand.Verify(command => command.CreateGuestAsync(bookingDto.Guest), Times.Once);

            // Verify mocks weren't called
            bookingFactory.Verify(factory => factory.Create(It.IsAny<CreatedBookingDto>()), Times.Never);
            repository.Verify(x => x.CreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CreateBookingAsync_ReturnsError_WhenRepositoryCreateFails()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IResourceIdQuery> resourceIdQuery = new Mock<IResourceIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();

            BookingCreateDto bookingDto = new BookingCreateDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),

                Guest = new GuestCreateDto
                {
                    FirstName = "Dansker",
                    LastName = "1",
                    Email = "test@example.com"
                }
            };

            Resource resource = new Resource(1, "Hytten med parasol", "Hytte", 100);

            Guest guest = new Guest("Dansker 1", null, 0, null, null, null, null);

            Booking booking = new Booking(guest.Id, bookingDto.ResourceId, bookingDto.StartDate, bookingDto.EndDate, 300);

            Exception repoException = new Exception("Database error");

            resourceIdQuery
                .Setup(x => x.GetResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<Resource>.Success(resource));

            guestCreateCommand
                .Setup(x => x.CreateGuestAsync(bookingDto.Guest))
                .ReturnsAsync(Result<Guest>.Success(guest));

            bookingFactory
                .Setup(x => x.Create(It.IsAny<CreatedBookingDto>()))
                .Returns(Result<Booking>.Success(booking));

            repository
                .Setup(x => x.CreateBookingAsync(booking))
                .ReturnsAsync(Result<Booking>.Error(booking, repoException));

            BookingCreateCommand sut = new BookingCreateCommand(
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestCreateCommand.Object
            );

            // Act
            IResult<CreatedBookingDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsError());
            IResultError<CreatedBookingDto> error = result.GetError();
            Assert.Equal(booking.StartDate, error.OriginalType.StartDate);
            Assert.Equal(booking.EndDate, error.OriginalType.EndDate);
            Assert.Equal(booking.ResourceId, error.OriginalType.ResourceId);
            Assert.Equal(repoException, error.Exception);
        }
    }
}
