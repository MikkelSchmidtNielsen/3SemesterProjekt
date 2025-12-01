using Application.ApplicationDto.Query;
using Application.RepositoryInterfaces;
using Application.Services.Query;
using Common;
using Common.ResultInterfaces;
using Moq;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.UnitTestHelpingTools;

namespace UnitTest.Application.UnitTest.ServiceTest
{
	public class ReadGuestCheckIfEmailIsAvailableQueryHandlerTest
	{
		[Fact]
		public async Task CheckIfEmailIsAvailable_ShouldReturnSuccess_WhenEmailIsAvailable()
		{
			// Arrange
			string userEmail = "noreply@foxtrox.dk";
			var readGuestCheckIfEmailExistQueryDto = Impression.Of<ReadGuestCheckIfEmailIsAvailableQueryDto>()
															.With("Email", userEmail)
															.WithDefaults()
															.Create();

			Mock<IGuestRepository> mockGuestRepo = new Mock<IGuestRepository>();
			mockGuestRepo.
				Setup(repo => repo.CheckIfEmailIsAvailableAsync(It.IsAny<string>())).
				ReturnsAsync(Result<string>.Success(userEmail));

			ReadGuestCheckIfEmailIsAvailableQueryHandler sut = new ReadGuestCheckIfEmailIsAvailableQueryHandler
				(guestRepository: mockGuestRepo.Object);

			// Act
			IResult<ReadGuestCheckIfEmailIsAvailableResponseDto> result = await sut.HandleAsync(readGuestCheckIfEmailExistQueryDto);


			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(userEmail, result.GetSuccess().OriginalType.Email);
		}

		[Fact]
		public async Task CheckIfEmailIsAvailable_ShouldReturnError_WhenEmailAlreadyExistInDatabase()
		{
			// Arrange
			string userEmail = "noreply@foxtrox.dk";
			Exception expectedError = new Exception("Email already exist");
			var readGuestCheckIfEmailExistQueryDto = Impression.Of<ReadGuestCheckIfEmailIsAvailableQueryDto>()
															.With("Email", userEmail)
															.WithDefaults()
															.Create();

			Mock<IGuestRepository> mockGuestRepo = new Mock<IGuestRepository>();
			mockGuestRepo.
				Setup(repo => repo.CheckIfEmailIsAvailableAsync(It.IsAny<string>())).
				ReturnsAsync(Result<string>.Conflict(userEmail, userEmail, expectedError));

			ReadGuestCheckIfEmailIsAvailableQueryHandler sut = new ReadGuestCheckIfEmailIsAvailableQueryHandler
				(guestRepository: mockGuestRepo.Object);

			// Act
			IResult<ReadGuestCheckIfEmailIsAvailableResponseDto> result = await sut.HandleAsync(readGuestCheckIfEmailExistQueryDto);


			// Assert
			Assert.True(result.IsError());
			Assert.Equal(expectedError.GetType(), result.GetError().Exception!.GetType());
			Assert.Equal(expectedError.Message, result.GetError().Exception!.Message);
		}


	}
}
