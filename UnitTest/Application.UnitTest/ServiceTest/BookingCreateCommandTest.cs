using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces.Command;
using Application.ServiceInterfaces.Query;
using Application.Services.Command;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
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

			var dto = new BookingWithGuestCreateDto
			{
				ResourceId = 1,
				StartDate = DateOnly.FromDateTime(DateTime.Now),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
				Guest = new GuestCreateDto()
			};

			var resource = new Resource(1,"Hytten med parasol", "Hytte", 100);

			var guest = new Guest("Dansker 1", null, 0, null, null, null, null);

			var booking = new Booking(guest.Id, dto.ResourceId, dto.StartDate, dto.EndDate, 300);

			resourceIdQuery
				.Setup(resourceIdQuery => resourceIdQuery.GetResourceByIdAsync(dto.ResourceId))
				.ReturnsAsync(Result<Resource>.Success(resource));

			guestCreateCommand
				.Setup(guestCreateCommand => guestCreateCommand.CreateGuestAsync(dto.Guest))
				.ReturnsAsync(Result<Guest>.Success(guest));

			bookingFactory
				.Setup(bookingFactory => bookingFactory.Create(guest.Id, dto.ResourceId, dto.StartDate, dto.EndDate, 200m))
				.Returns(Result<Booking>.Success(booking));

			repository
				.Setup(repo => repo.CreateBookingAsync(booking))
				.ReturnsAsync(Result<Booking>.Success(booking));

			var sut = new BookingCreateCommand(
				repository.Object,
				resourceIdQuery.Object,
				bookingFactory.Object,
				guestCreateCommand.Object);

			// Act
			var result = await sut.CreateBookingAsync(dto);

			// Assert
			Assert.True(result.IsSucces());
			IResultSuccess<Booking> success = result.GetSuccess();
			Assert.Equal(booking, success.OriginalType);

			guestCreateCommand.Verify(x => x.CreateGuestAsync(dto.Guest), Times.Once);
			bookingFactory.Verify(x => x.Create(guest.Id, dto.ResourceId, dto.StartDate, dto.EndDate, 200m), Times.Once);
			repository.Verify(x => x.CreateBookingAsync(booking), Times.Once);
		}

		[Fact]
		public async Task CreateBookingAsync_ReturnsError_WhenGuestCreationFails()
		{
			// Arrange
			Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
			Mock<IResourceIdQuery> resourceIdQuery = new Mock<IResourceIdQuery>();
			Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
			Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();

			BookingWithGuestCreateDto dto = new BookingWithGuestCreateDto {
				ResourceId = 1,
				StartDate = DateOnly.FromDateTime(DateTime.Now),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
				Guest = new GuestCreateDto()
			};

			Resource resource = new Resource(1, "Hytten med parasol", "Hytte", 100);

			Exception guestException = new Exception("Guest creation failed");

			resourceIdQuery
				.Setup(x => x.GetResourceByIdAsync(dto.ResourceId))
				.ReturnsAsync(Result<Resource>.Success(resource));

			guestCreateCommand
				.Setup(x => x.CreateGuestAsync(dto.Guest))
				.ReturnsAsync(Result<Guest>.Error(originalType: null, exception: guestException));

			BookingCreateCommand sut = new BookingCreateCommand(
				repository.Object,
				resourceIdQuery.Object,
				bookingFactory.Object,
				guestCreateCommand.Object
			);

			// Act
			IResult<Booking> result = await sut.CreateBookingAsync(dto);

			// Assert
			Assert.True(result.IsError());
			IResultError<Booking> error = result.GetError();
			Assert.Null(error.OriginalType);
			Assert.Equal(guestException, error.Exception);

			bookingFactory.Verify(x => x.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<decimal>()), Times.Never);
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

			BookingWithGuestCreateDto dto = new BookingWithGuestCreateDto
			{
				ResourceId = 1,
				StartDate = DateOnly.FromDateTime(DateTime.Now),
				EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
				Guest = new GuestCreateDto()
			};

			Resource resource = new Resource(1, "Hytten med parasol", "Hytte", 100);

			Guest guest = new Guest("Dansker 1", null, 0, null, null, null, null);

			Booking booking = new Booking(guest.Id, dto.ResourceId, dto.StartDate, dto.EndDate, 300);

			Exception repoException = new Exception("Database error");

			resourceIdQuery
				.Setup(x => x.GetResourceByIdAsync(dto.ResourceId))
				.ReturnsAsync(Result<Resource>.Success(resource));

			guestCreateCommand
				.Setup(x => x.CreateGuestAsync(dto.Guest))
				.ReturnsAsync(Result<Guest>.Success(guest));

			bookingFactory
				.Setup(x => x.Create(guest.Id, dto.ResourceId, dto.StartDate, dto.EndDate, 200m))
				.Returns(Result<Booking>.Success(booking));

			repository
				.Setup(x => x.CreateBookingAsync(booking))
				.ReturnsAsync(Result<Booking>.Error(originalType: booking, exception: repoException));

			BookingCreateCommand sut = new BookingCreateCommand(
				repository.Object,
				resourceIdQuery.Object,
				bookingFactory.Object,
				guestCreateCommand.Object
			);

			// Act
			IResult<Booking> result = await sut.CreateBookingAsync(dto);

			// Assert
			Assert.True(result.IsError());
			IResultError<Booking> error = result.GetError();
			Assert.Equal(booking, error.OriginalType);
			Assert.Equal(repoException, error.Exception);
		}
	}
}
