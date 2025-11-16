using Application.ApplicationDto.Command;
using Application.RepositoryInterfaces;
using Common.ResultInterfaces;
using Common;
using Domain.DomainInterfaces;
using Domain.Models;
using Moq;
using Application.Services.Command;

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

			GuestCreateDto dto = new GuestCreateDto
			{
				FirstName = "Dansker",
				LastName = "Et",
				PhoneNumber = 12345678,
				Email = "test@test.com",
				Country = "Denmark",
				Language = "Dansk",
				Address = "Testvej 1"
			};

			Guest guest = new Guest(
				dto.FirstName,
				dto.LastName,
				dto.PhoneNumber,
				dto.Email,
				dto.Country,
				dto.Language,
				dto.Address
			);

			guestFactory
				.Setup(guestFactory => guestFactory.Create(
					dto.FirstName,
					dto.LastName,
					dto.PhoneNumber,
					dto.Email,
					dto.Country,
					dto.Language,
					dto.Address
				))
				.Returns(Result<Guest>.Success(guest));

			repo
				.Setup(repo => repo.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Success(guest));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(dto);

			// Assert
			Assert.True(result.IsSucces());
			IResultSuccess<Guest> success = result.GetSuccess();
			Assert.Equal(guest, success.OriginalType);

			guestFactory.Verify(x => x.Create(
				dto.FirstName,
				dto.LastName,
				dto.PhoneNumber,
				dto.Email,
				dto.Country,
				dto.Language,
				dto.Address
			), Times.Once);

			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}

		[Fact]
		public async Task CreateGuestAsync_ReturnsError_WhenRepositoryCreateFails()
		{
			// Arrange
			Mock<IGuestFactory> guestFactory = new Mock<IGuestFactory>();
			Mock<IGuestRepository> repo = new Mock<IGuestRepository>();

			GuestCreateDto dto = new GuestCreateDto
			{
				FirstName = "Dansker",
				LastName = "Et",
				PhoneNumber = 12345678,
				Email = "test@test.com",
				Country = "Denmark",
				Language = "Dansk",
				Address = "Testvej 1"
			};

			Guest guest = new Guest(
				dto.FirstName,
				dto.LastName,
				dto.PhoneNumber,
				dto.Email,
				dto.Country,
				dto.Language,
				dto.Address
			);

			Exception repoException = new Exception("Database error");

			guestFactory
				.Setup(x => x.Create(
					dto.FirstName,
					dto.LastName,
					dto.PhoneNumber,
					dto.Email,
					dto.Country,
					dto.Language,
					dto.Address
				))
				.Returns(Result<Guest>.Success(guest));

			repo
				.Setup(x => x.CreateGuestAsync(guest))
				.ReturnsAsync(Result<Guest>.Error(originalType: guest, exception: repoException));

			GuestCreateCommand sut = new GuestCreateCommand(guestFactory.Object, repo.Object);

			// Act
			IResult<Guest> result = await sut.CreateGuestAsync(dto);

			// Assert
			Assert.True(result.IsError());
			IResultError<Guest> error = result.GetError();
			Assert.Equal(guest, error.OriginalType);
			Assert.Equal(repoException, error.Exception);

			guestFactory.Verify(x => x.Create(
				dto.FirstName,
				dto.LastName,
				dto.PhoneNumber,
				dto.Email,
				dto.Country,
				dto.Language,
				dto.Address
			), Times.Once);

			repo.Verify(x => x.CreateGuestAsync(guest), Times.Once);
		}
	}
}
