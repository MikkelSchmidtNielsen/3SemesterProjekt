using Application.Factories;
using Application.RepositoryInterfaces;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using Moq;

namespace UnitTest.Application.UnitTest.FactoryTest
{
	public class GuestFactoryTest
	{
		[Fact]
		public async Task FactoryCreation_ShouldReturnSuccess_WhenGivenCorrectGuestAndEmailDoesntExist()
		{
			// Arrange
			CreatedGuestDto dto = new CreatedGuestDto
			{
				FirstName = "Mikkel",
				LastName = "Schmidt",
				PhoneNumber = 12345678,
				Email = "test@test.com",
				Country = "Danmark",
				Language = "Dansk",
				Address = "Vejvej 12"
			};
			Mock<IGuestRepository> guestRepository = new Mock<IGuestRepository>();
			guestRepository
				.Setup(repo => repo.CheckIfEmailIsAvailableAsync(It.IsAny<string>())).ReturnsAsync(Result<string>.Success(dto.Email));

			GuestFactory factory = new GuestFactory(guestRepository.Object);

			// Act
			IResult<Guest> result = await factory.CreateAsync(dto);
			IResultSuccess<Guest> success = result.GetSuccess();

			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(dto.FirstName, success.OriginalType.FirstName);
			Assert.Equal(dto.LastName, success.OriginalType.LastName);
			Assert.Equal(dto.PhoneNumber, success.OriginalType.PhoneNumber);
			Assert.Equal(dto.Email, success.OriginalType.Email);
			Assert.Equal(dto.Country, success.OriginalType.Country);
			Assert.Equal(dto.Language, success.OriginalType.Language);
			Assert.Equal(dto.Address, success.OriginalType.Address);
		}

		[Fact]
		public async Task FactoryCreation_ShouldReturnError_WhenGivenEmailWhichAlreadyExist()
		{
			// Arrange
			CreatedGuestDto dto = new CreatedGuestDto
			{
				FirstName = "Mikkel",
				LastName = "Schmidt",
				PhoneNumber = 12345678,
				Email = "test@test.com",
				Country = "Danmark",
				Language = "Dansk",
				Address = "Vejvej 12"
			};
			Exception expectedException = new Exception("Email already exist");

			Mock<IGuestRepository> guestRepository = new Mock<IGuestRepository>();
			guestRepository
				.Setup(repo => repo.CheckIfEmailIsAvailableAsync(It.IsAny<string>())).ReturnsAsync(Result<string>.Error(null, expectedException));

			GuestFactory factory = new GuestFactory(guestRepository.Object);

			// Act
			IResult<Guest> result = await factory.CreateAsync(dto);

			// Assert
			Assert.True(result.IsError());
			var ex = Assert.IsType<Exception>(result.GetError().Exception);
			Assert.Equal(expectedException.Message, ex.Message);
		}
	}
}
