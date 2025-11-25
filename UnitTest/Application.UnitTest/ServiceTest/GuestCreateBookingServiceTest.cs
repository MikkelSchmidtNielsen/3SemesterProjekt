using Application.ApplicationDto.Command;
using Application.Factories;
using Application.RepositoryInterfaces;
using Application.Services.Command;
using Castle.Core.Resource;
using Common;
using Common.ResultInterfaces;
using Domain.DomainInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Application.UnitTest.ServiceTest
{

    public class GuestCreateBookingServiceTest
    {
        [Fact]
        public async Task GuestBookingCreation_Passes_WhenAllServiceStepsSucceed()
        {
            // Arrange
            Mock<IGuestRepository> guestRepo = new Mock<IGuestRepository>();
            Mock<IResourceRepository> resourceRepo = new Mock<IResourceRepository>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();

            int guestId = 1;
            int resourceId = 1;
            Guest guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Danmark", "Danish", "Allanvej 11");
            Resource resource = new Resource("Paradis", "Hytte", 150, 5, "");

            GuestInputDto dto = new GuestInputDto
            {
                Email = "test@test.dk",
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TotalPrice = 100,
                Guest = guest,
                Resource = resource,
            };

            Booking booking = new Booking(
                guestId,
                resourceId,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                300);

            // Mock Guest Repository
            guestRepo
                .Setup(repo => repo.GetGuestByEmailAsync(dto.Email))
                .ReturnsAsync(Result<Guest>.Success(guest));

            // Mock Resource Repository
            resourceRepo
                .Setup(repo => repo.GetResourceByIdAsync(dto.ResourceId))
                .ReturnsAsync(Result<Resource>.Success(resource));

            GuestInputDomainDto domainDto = Mapper.Map<GuestInputDomainDto>(dto);

            // Mock Booking Factory
            bookingFactory
                .Setup(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()))
                .Returns(Result<Booking>.Success(booking));

            // Mock Booking Repository
            bookingRepo
                .Setup(repo => repo.GuestCreateBookingAsync(booking))
                .ReturnsAsync(Result<Booking>.Success(booking));

            GuestCreateBookingService sut = new GuestCreateBookingService
                (
                guestRepo.Object,
                resourceRepo.Object,
                bookingFactory.Object,
                bookingRepo.Object
                );

            // ExpectedDto
            BookingCreatedDto expectedDto = new BookingCreatedDto
            {
                Guest = domainDto.Guest,
                Resource = domainDto.Resource,
                StartDate = domainDto.StartDate,
                EndDate = domainDto.EndDate,
                TotalPrice = 300
            };

            // Act
            IResult<BookingCreatedDto> result = await sut.HandleAsync(dto);

            // Assert
            Assert.True(result.IsSucces());

            BookingCreatedDto success = result.GetSuccess().OriginalType;
            Assert.Equal(expectedDto.Guest, success.Guest);
            Assert.Equal(expectedDto.Resource, success.Resource);
            Assert.Equal(expectedDto.StartDate, success.StartDate);
            Assert.Equal(expectedDto.EndDate, success.EndDate);
            Assert.Equal(expectedDto.TotalPrice, success.TotalPrice);
        }

        [Fact]
        public async Task GuestBookingCreation_Fails_WhenGuestEmailDoesntExist()
        {
            // Arrange
            Mock<IGuestRepository> guestRepo = new Mock<IGuestRepository>();
            Mock<IResourceRepository> resourceRepo = new Mock<IResourceRepository>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();

            int guestId = 1;
            int resourceId = 1;
            Guest guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Danmark", "Danish", "Allanvej 11");
            Resource resource = new Resource("Paradis", "Hytte", 150, 5, "");

            GuestInputDto dto = new GuestInputDto
            {
                Email = "test@test.dk",
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TotalPrice = 100,
                Guest = guest,
                Resource = resource,
            };

            Booking booking = new Booking(
                guestId,
                resourceId,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                300);

            Exception emailException = new Exception("Email findes ikke");

            // Mock Guest Repository
            guestRepo
                .Setup(repo => repo.GetGuestByEmailAsync(dto.Email))
                .ReturnsAsync(Result<Guest>.Error(null!, emailException));

            GuestCreateBookingService sut = new GuestCreateBookingService
                (
                guestRepo.Object,
                resourceRepo.Object,
                bookingFactory.Object,
                bookingRepo.Object
                );

            // Act
            IResult<BookingCreatedDto> result = await sut.HandleAsync(dto);

            // Assert
            Assert.True(result.IsError());
            IResultError<BookingCreatedDto> error = result.GetError();
            Assert.Equal(emailException, error.Exception);

            // Verify mock were called
            guestRepo.Verify(query => query.GetGuestByEmailAsync(dto.Email), Times.Once);

            // Verify mocks weren't called
            resourceRepo.Verify(command => command.GetResourceByIdAsync(It.IsAny<int>()), Times.Never());
            bookingFactory.Verify(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()), Times.Never);
            bookingRepo.Verify(repo => repo.GuestCreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task GuestBookingCreation_Fails_WhenResourceFails()
        {
            // Arrange
            Mock<IGuestRepository> guestRepo = new Mock<IGuestRepository>();
            Mock<IResourceRepository> resourceRepo = new Mock<IResourceRepository>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();

            int guestId = 1;
            int resourceId = 1;
            Guest guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Danmark", "Danish", "Allanvej 11");
            Resource resource = new Resource("Paradis", "Hytte", 150, 5, "");

            GuestInputDto dto = new GuestInputDto
            {
                Email = "test@test.dk",
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TotalPrice = 100,
                Guest = guest,
                Resource = resource,
            };


            Booking booking = new Booking(
                guestId,
                resourceId,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                300);

            // Mock Guest Repository
            guestRepo
                .Setup(repo => repo.GetGuestByEmailAsync(dto.Email))
                .ReturnsAsync(Result<Guest>.Success(guest));

            Exception resourceException = new Exception("Resourcen findes ikke");

            // Mock Resource Repository
            resourceRepo
                .Setup(repo => repo.GetResourceByIdAsync(dto.ResourceId))
                .ReturnsAsync(Result<Resource>.Error(null!, resourceException));

            GuestCreateBookingService sut = new GuestCreateBookingService
                (
                guestRepo.Object,
                resourceRepo.Object,
                bookingFactory.Object,
                bookingRepo.Object
                );

            // Act
            IResult<BookingCreatedDto> result = await sut.HandleAsync(dto);

            // Assert
            Assert.True(result.IsError());
            IResultError<BookingCreatedDto> error = result.GetError();
            Assert.Equal(resourceException, error.Exception);

            // Verify mock were called
            guestRepo.Verify(query => query.GetGuestByEmailAsync(dto.Email), Times.Once);
            resourceRepo.Verify(command => command.GetResourceByIdAsync(It.IsAny<int>()), Times.Once());

            // Verify mocks weren't called
            bookingFactory.Verify(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()), Times.Never);
            bookingRepo.Verify(repo => repo.GuestCreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task GuestBookingCreation_Fails_WhenBookingFactoryFails()
        {
            // Arrange
            Mock<IGuestRepository> guestRepo = new Mock<IGuestRepository>();
            Mock<IResourceRepository> resourceRepo = new Mock<IResourceRepository>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();

            int guestId = 1;
            int resourceId = 1;
            Guest guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Danmark", "Danish", "Allanvej 11");
            Resource resource = new Resource("Paradis", "Hytte", 150, 5, "");

            GuestInputDto dto = new GuestInputDto
            {
                Email = "test@test.dk",
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                TotalPrice = 100,
                Guest = guest,
                Resource = resource,
            };

            Booking booking = new Booking(
                guestId,
                resourceId,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                300);

            // Mock Guest Repository
            guestRepo
                .Setup(repo => repo.GetGuestByEmailAsync(dto.Email))
                .ReturnsAsync(Result<Guest>.Success(guest));

            // Mock Resource Repository
            resourceRepo
                .Setup(repo => repo.GetResourceByIdAsync(dto.ResourceId))
                .ReturnsAsync(Result<Resource>.Success(resource));

            Exception bookingException = new Exception("Fejl");

            // Mock Booking Factory
            bookingFactory
                .Setup(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()))
                .Returns(Result<Booking>.Error(booking, bookingException));

            GuestCreateBookingService sut = new GuestCreateBookingService
                 (
                 guestRepo.Object,
                 resourceRepo.Object,
                 bookingFactory.Object,
                 bookingRepo.Object
                 );

            // Act
            IResult<BookingCreatedDto> result = await sut.HandleAsync(dto);

            // Assert
            Assert.True(result.IsError());
            IResultError<BookingCreatedDto> error = result.GetError();
            Assert.Equal(bookingException, error.Exception);

            // Verify mock were called
            guestRepo.Verify(query => query.GetGuestByEmailAsync(dto.Email), Times.Once);
            resourceRepo.Verify(command => command.GetResourceByIdAsync(It.IsAny<int>()), Times.Once());
            bookingFactory.Verify(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()), Times.Once());
            
            // Verify mocks weren't called

            bookingRepo.Verify(repo => repo.GuestCreateBookingAsync(It.IsAny<Booking>()), Times.Never);
        }

        [Fact]
        public async Task GuestBookingCreation_Fails_WhenBookingDoesntSave()
        {
            // Arrange
            Mock<IGuestRepository> guestRepo = new Mock<IGuestRepository>();
            Mock<IResourceRepository> resourceRepo = new Mock<IResourceRepository>();
            Mock<IBookingFactory> bookingFactory = new Mock<IBookingFactory>();
            Mock<IBookingRepository> bookingRepo = new Mock<IBookingRepository>();

            int guestId = 1;
            int resourceId = 1;
            Guest guest = new Guest("Allan", "Allansen", 12345678, "test@test.dk", "Danmark", "Danish", "Allanvej 11");
            Resource resource = new Resource("Paradis", "Hytte", 150, 5, "");

            GuestInputDto dto = new GuestInputDto
            {
                Email = "test@test.dk",
                ResourceId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TotalPrice = 100,
                Guest = guest,
                Resource = resource,
            };


            Booking booking = new Booking(
                guestId,
                resourceId,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                300);

            // Mock Guest Repository
            guestRepo
                .Setup(repo => repo.GetGuestByEmailAsync(dto.Email))
                .ReturnsAsync(Result<Guest>.Success(guest));

            // Mock Resource Repository
            resourceRepo
                .Setup(repo => repo.GetResourceByIdAsync(dto.ResourceId))
                .ReturnsAsync(Result<Resource>.Success(resource));

            // Mock Booking Factory
            bookingFactory
                .Setup(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()))
                .Returns(Result<Booking>.Success(booking));

            Exception bookingRepoException = new Exception("Fejl");

            // Mock Booking Repository
            bookingRepo
                .Setup(repo => repo.GuestCreateBookingAsync(booking))
                .ReturnsAsync(Result<Booking>.Error(booking, bookingRepoException));

            GuestCreateBookingService sut = new GuestCreateBookingService
                 (
                 guestRepo.Object,
                 resourceRepo.Object,
                 bookingFactory.Object,
                 bookingRepo.Object
                 );

            // Act
            IResult<BookingCreatedDto> result = await sut.HandleAsync(dto);

            // Arrange
            Assert.True(result.IsError());
            IResultError<BookingCreatedDto> error = result.GetError();
            Assert.Equal(bookingRepoException, error.Exception);

            // Verify mock were called
            guestRepo.Verify(query => query.GetGuestByEmailAsync(dto.Email), Times.Once);
            resourceRepo.Verify(command => command.GetResourceByIdAsync(It.IsAny<int>()), Times.Once());
            bookingFactory.Verify(factory => factory.GuestCreate(It.IsAny<GuestInputDomainDto>()), Times.Once());
            bookingRepo.Verify(repo => repo.GuestCreateBookingAsync(It.IsAny<Booking>()), Times.Once());
        }
    }
}
