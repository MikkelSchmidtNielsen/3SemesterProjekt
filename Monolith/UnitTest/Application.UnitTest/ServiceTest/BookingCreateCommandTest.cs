using Application.ApplicationDto.Command;
using Application.ApplicationDto.Query;
using Application.InfrastructureInterfaces;
using Application.InfrastructureInterfaces.SendEmailSpecifications;
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
using System.Data.Common;
using UnitTest.UnitTestHelpingTools;
using Xunit.Sdk;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class BookingCreateCommandTest
    {
        [Fact]
        public async Task CreateBookingAsync_ReturnsSuccess_WhenAllStepsSucceed()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IReadResourceByIdQuery> resourceIdQuery = new Mock<IReadResourceByIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();
            Mock<IReadGuestByEmailQuery> guestByEmailQuery = new Mock<IReadGuestByEmailQuery>();
            Mock<ISendEmail> sendEmail = new Mock<ISendEmail>();
            Mock<ISendEmailSpecification> emailSpec = new Mock<ISendEmailSpecification>();

            BookingCreateRequestDto bookingDto = new BookingCreateRequestDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),

                Guest = new GuestCreateRequestDto
                {
                    FirstName = "Dansker",
                    LastName = "1",
                    Email = "Email@email.dk"
                }
            };

            ReadResourceByIdQueryResponseDto resource = Impression.Of<ReadResourceByIdQueryResponseDto>()
                .Randomize()
                .Create();

            Guest guest = new Guest
            (
                firstName: "FirstName",
                lastName: "LastName",
                phoneNumber: 25252525,
                email: "Email@email.dk",
                country: "Country",
                language: "Language",
                address: "Address"
            );

            // Guest creation by email exception
            Exception emailException = new Exception("Email not found");

            Booking booking = new Booking(guest.Id, bookingDto.ResourceId, bookingDto.StartDate, bookingDto.EndDate, totalPrice: 300);

            // What we expect as final
            BookingRequestResultDto expectedDto = new BookingRequestResultDto
            {
                ResourceId = bookingDto.ResourceId,
                GuestId = guest.Id,
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                TotalPrice = 300
            };

            // Mock Resource query
            resourceIdQuery
                .Setup(query => query.ReadResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<ReadResourceByIdQueryResponseDto>.Success(resource));

            guestByEmailQuery
                .Setup(query => query.ReadGuestByEmailAsync(bookingDto.Guest.Email))
                .ReturnsAsync(Result<Guest>.Error(guest, emailException));

            // Mock Guest creation
            guestCreateCommand
                .Setup(command => command.CreateGuestAsync(bookingDto.Guest))
                .ReturnsAsync(Result<Guest>.Success(guest));

            // Mock Booking factory
            bookingFactory
                .Setup(factory => factory.Create(It.IsAny<CreateBookingFactoryDto>()))
                .Returns(Result<Booking>.Success(booking));

            // Mock Repository
            repository
                .Setup(repo => repo.CreateBookingAsync(booking))
                .ReturnsAsync(Result<Booking>.Success(booking));

            // Mock ISendEmail
            sendEmail
                .Setup(send => send.SendEmail(It.IsAny<ISendEmailSpecification>()))
                .Returns(Result<ISendEmailSpecification>.Success(emailSpec.Object));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestByEmailQuery.Object,
                guestCreateCommand.Object,
                sendEmail.Object
            );

            // Act
            IResult<BookingRequestResultDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsSucces());

            BookingRequestResultDto success = result.GetSuccess().OriginalType;
            Assert.Equal(expectedDto.ResourceId, success.ResourceId);
            Assert.Equal(expectedDto.GuestId, success.GuestId);
            Assert.Equal(expectedDto.TotalPrice, success.TotalPrice);
            Assert.Equal(expectedDto.StartDate, success.StartDate);
            Assert.Equal(expectedDto.EndDate, success.EndDate);

            // Verify mocks were called
            resourceIdQuery.Verify(query => query.ReadResourceByIdAsync(bookingDto.ResourceId), Times.Once);
            guestCreateCommand.Verify(command => command.CreateGuestAsync(bookingDto.Guest), Times.Once);
            bookingFactory.Verify(factory => factory.Create(It.IsAny<CreateBookingFactoryDto>()), Times.Once);
            repository.Verify(repo => repo.CreateBookingAsync(booking), Times.Once);
        }

        [Fact]
        public async Task CreateBookingAsync_ReturnsError_WhenGetResourceFails()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IReadResourceByIdQuery> resourceIdQuery = new Mock<IReadResourceByIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();
            Mock<IReadGuestByEmailQuery> guestByEmailQuery = new Mock<IReadGuestByEmailQuery>();
            Mock<ISendEmail> sendEmail = new Mock<ISendEmail>();
            Mock<ISendEmailSpecification> emailSpec = new Mock<ISendEmailSpecification>();

            BookingCreateRequestDto bookingDto = new BookingCreateRequestDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            };

            Exception resourceException = new Exception("Resource not found");

            // Mock Resource query
            resourceIdQuery
                .Setup(query => query.ReadResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<ReadResourceByIdQueryResponseDto>.Error(null!, resourceException));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestByEmailQuery.Object,
                guestCreateCommand.Object,
                sendEmail.Object
            );

            // Act
            IResult<BookingRequestResultDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsError());
            IResultError<BookingRequestResultDto> error = result.GetError();
            Assert.Equal(resourceException, error.Exception);

            // Verify mock were called
            resourceIdQuery.Verify(query => query.ReadResourceByIdAsync(bookingDto.ResourceId), Times.Once);

            // Verify mocks weren't called
            guestCreateCommand.Verify(command => command.CreateGuestAsync(It.IsAny<GuestCreateRequestDto>()), Times.Never());
            bookingFactory.Verify(factory => factory.Create(It.IsAny<CreateBookingFactoryDto>()), Times.Never);
            repository.Verify(repo => repo.CreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CreateBookingAsync_ReturnsError_WhenGuestCreationFails()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IReadResourceByIdQuery> resourceIdQuery = new Mock<IReadResourceByIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();
            Mock<IReadGuestByEmailQuery> guestByEmailQuery = new Mock<IReadGuestByEmailQuery>();
            Mock<ISendEmail> sendEmail = new Mock<ISendEmail>();
            Mock<ISendEmailSpecification> emailSpec = new Mock<ISendEmailSpecification>();

            BookingCreateRequestDto bookingDto = new BookingCreateRequestDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                Guest = new GuestCreateRequestDto()
            };

            ReadResourceByIdQueryResponseDto resource = Impression.Of<ReadResourceByIdQueryResponseDto>()
                .WithDefaults()
                .Create();

            Exception guestException = new Exception("Guest creation failed");

            // Mock Resource query
            resourceIdQuery
                .Setup(x => x.ReadResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<ReadResourceByIdQueryResponseDto>.Success(resource));

            // Mock Guest creation
            guestCreateCommand
                .Setup(command => command.CreateGuestAsync(bookingDto.Guest))
                .ReturnsAsync(Result<Guest>.Error(null!, guestException));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestByEmailQuery.Object,
                guestCreateCommand.Object,
                sendEmail.Object
            );

            // Act
            IResult<BookingRequestResultDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsError());
            IResultError<BookingRequestResultDto> error = result.GetError();
            Assert.Equal(guestException, error.Exception);

            // Verify mock were called
            resourceIdQuery.Verify(query => query.ReadResourceByIdAsync(bookingDto.ResourceId), Times.Once);
            guestCreateCommand.Verify(command => command.CreateGuestAsync(bookingDto.Guest), Times.Once);

            // Verify mocks weren't called
            bookingFactory.Verify(factory => factory.Create(It.IsAny<CreateBookingFactoryDto>()), Times.Never);
            repository.Verify(repo => repo.CreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task CreateBookingAsync_ReturnsError_WhenRepositoryCreateFails()
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IReadResourceByIdQuery> resourceIdQuery = new Mock<IReadResourceByIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();
            Mock<IReadGuestByEmailQuery> guestByEmailQuery = new Mock<IReadGuestByEmailQuery>();
            Mock<ISendEmail> sendEmail = new Mock<ISendEmail>();
            Mock<ISendEmailSpecification> emailSpec = new Mock<ISendEmailSpecification>();

            BookingCreateRequestDto bookingDto = new BookingCreateRequestDto
            {
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),

                Guest = new GuestCreateRequestDto
                {
                    FirstName = "Dansker",
                    LastName = "1",
                    Email = "Email@email.dk"
                }
            };

            ReadResourceByIdQueryResponseDto resource = Impression.Of<ReadResourceByIdQueryResponseDto>()
                .WithDefaults()
                .Create();

            Guest guest = Impression.Of<Guest>()
                .WithDefaults()
                .Create();

            Booking booking = new Booking(guest.Id, bookingDto.ResourceId, bookingDto.StartDate, bookingDto.EndDate, totalPrice: 300);

            // Guest creation by email exception
            Exception emailException = new Exception("Email not found");

            // Repo (test) exception
            Exception repoException = new Exception("Database error");

            resourceIdQuery
                .Setup(query => query.ReadResourceByIdAsync(bookingDto.ResourceId))
                .ReturnsAsync(Result<ReadResourceByIdQueryResponseDto>.Success(resource));

            guestByEmailQuery
                .Setup(query => query.ReadGuestByEmailAsync(bookingDto.Guest.Email))
                .ReturnsAsync(Result<Guest>.Error(guest, emailException));

            guestCreateCommand
                .Setup(command => command.CreateGuestAsync(bookingDto.Guest))
                .ReturnsAsync(Result<Guest>.Success(guest));

            bookingFactory
                .Setup(factory => factory.Create(It.IsAny<CreateBookingFactoryDto>()))
                .Returns(Result<Booking>.Success(booking));

            repository
                .Setup(repo => repo.CreateBookingAsync(booking))
                .ReturnsAsync(Result<Booking>.Error(booking, repoException));

            BookingCreateCommand sut = new BookingCreateCommand
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestByEmailQuery.Object,
                guestCreateCommand.Object,
                sendEmail.Object
            );

            // Act
            IResult<BookingRequestResultDto> result = await sut.CreateBookingAsync(bookingDto);

            // Assert
            Assert.True(result.IsError());
            IResultError<BookingRequestResultDto> error = result.GetError();
            Assert.Equal(booking.StartDate, error.OriginalType.StartDate);
            Assert.Equal(booking.EndDate, error.OriginalType.EndDate);
            Assert.Equal(booking.ResourceId, error.OriginalType.ResourceId);
            Assert.Equal(repoException, error.Exception);
        }

        [Theory]
        [InlineData(1, 3, 300)]
        [InlineData(1, 5, 500)]
        [InlineData(1, 30, 3000)]
        [InlineData(3, 4, 200)]
        public void AddPriceToDto_CalculatesTotalPriceCorrectly(int start, int end, decimal expected)
        {
            // Arrange
            Mock<IBookingRepository> repository = new Mock<IBookingRepository>();
            Mock<IReadResourceByIdQuery> resourceIdQuery = new Mock<IReadResourceByIdQuery>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IGuestCreateCommand> guestCreateCommand = new Mock<IGuestCreateCommand>();
            Mock<IReadGuestByEmailQuery> guestByEmailQuery = new Mock<IReadGuestByEmailQuery>();
            Mock<ISendEmail> sendEmail = new Mock<ISendEmail>();

            BookingCreateCommandTestClass testClass = new BookingCreateCommandTestClass
            (
                repository.Object,
                resourceIdQuery.Object,
                bookingFactory.Object,
                guestByEmailQuery.Object,
                guestCreateCommand.Object,
                sendEmail.Object
            );

            BookingRequestResultDto dto = new BookingRequestResultDto
            {
                StartDate = new DateOnly(2025, 11, start),
                EndDate = new DateOnly(2025, 11, end)
            };

            ReadResourceByIdQueryResponseDto resource = Impression.Of<ReadResourceByIdQueryResponseDto>()
                .With("BasePrice", 100.00m)
                .WithDefaults()
                .Create();

            // Act
            testClass.AddPriceToDto(dto, resource);

            // Assert
            Assert.Equal(expected, dto.TotalPrice);
        }
    }
}
