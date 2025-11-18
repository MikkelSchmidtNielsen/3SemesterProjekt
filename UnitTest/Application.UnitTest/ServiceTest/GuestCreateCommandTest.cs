using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Common.ResultInterfaces;
using Common;
using Domain.DomainInterfaces;
using Domain.Models;
using Moq;
using Application.Services.Command;
using Domain.ModelsDto;

namespace UnitTest.Application.UnitTest.ServiceTest
{
    public class GuestCreateCommandTest
    {
		[Fact]
		public async Task CreateGuestAsync_ReturnsSuccess_WhenFactoryAndRepositorySucceed()
		{
			// Arrange
			Mock<IGuestFactory> guestFactory = new Mock<IGuestFactory>();
			Mock<IGuestRepository> repo = new Mock<IGuestRepository>();

            GuestCreateDto createDto = new GuestCreateDto();
            Guest guest = new Guest("Mikkel", null, null, null, null, null, null);
            Exception repoException = new Exception("Database error");

            guestFactory
				.Setup(guestFactory => guestFactory.Create(It.IsAny<CreatedGuestDto>()))
				.Returns(Result<Guest>.Success(guest));

			repo
				.Setup(repo => repo.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Success(guest));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(createDto);

			// Assert
			Assert.True(result.IsSucces());
			IResultSuccess<Guest> success = result.GetSuccess();
			Assert.Equal(guest, success.OriginalType);

			guestFactory.Verify(x => x.Create(It.IsAny<CreatedGuestDto>()), Times.Once);

			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}

		[Fact]
		public async Task CreateGuestAsync_ReturnsError_WhenRepositoryCreateFails()
		{
			// Arrange
			Mock<IGuestFactory> guestFactory = new Mock<IGuestFactory>();
			Mock<IGuestRepository> repo = new Mock<IGuestRepository>();

			GuestCreateDto createDto = new GuestCreateDto();
			Guest guest = new Guest("Mikkel", null, null, null, null, null, null);
			Exception repoException = new Exception("Database error");

			guestFactory
				.Setup(x => x.Create(It.IsAny<CreatedGuestDto>()))
				.Returns(Result<Guest>.Success(guest));

			repo
				.Setup(x => x.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Error(guest, repoException));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(createDto);

			// Assert
			Assert.True(result.IsError());
			IResultError<Guest> error = result.GetError();
			Assert.Equal(guest, error.OriginalType);
			Assert.Equal(repoException, error.Exception);

			guestFactory.Verify(x => x.Create(It.IsAny<CreatedGuestDto>()), Times.Once);
			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}
	}
}
