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
		public async Task CheckIfEmailIsAvailable_ShouldReturnSuccess_WhenEmailExistInDatabase()
		{
			// Arrange
			string userEmail = "noreply@foxtrox.dk";
			var readGuestCheckIfEmailExistQueryDto = Impression.Of<ReadGuestCheckIfEmailIsAvailableQueryDto>()
															.With("Email", userEmail)
															.WithDefaults()
															.Create();

			Mock<IGuestRepository> mockGuestRepo = new Mock<IGuestRepository>();
			mockGuestRepo.
				Setup(repo => repo.CheckIfEmailIsAvailable(It.IsAny<string>())).
				ReturnsAsync(Result<string>.Success(userEmail));

			ReadGuestCheckIfEmailIsAvailableQueryHandler sut = new ReadGuestCheckIfEmailIsAvailableQueryHandler
				(guestRepository: mockGuestRepo.Object);

			// Act
			IResult<ReadGuestCheckIfEmailIsAvailableResponseDto> result = await sut.HandleAsync(readGuestCheckIfEmailExistQueryDto);


			// Assert
			Assert.True(result.IsSucces());
			Assert.Equal(userEmail, result.GetSuccess().OriginalType.Email);
		}
	}
}
